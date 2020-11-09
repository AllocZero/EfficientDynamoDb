/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */

using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime.Internal;
using Amazon.Runtime.CredentialManagement.Internal;
using Amazon.Runtime.Internal.Util;
using Amazon.Runtime.SharedInterfaces;
using Amazon.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace Amazon.Runtime
{
#if !NETSTANDARD13
    /// <summary>
    /// Temporary credentials that are created following successful authentication with
    /// a federated endpoint supporting SAML.
    /// </summary>
    /// <remarks>
    /// In order to use the FederatedAWSCredentials class the AWSSDK.SecurityToken assembly
    /// must be available to your application at runtime.
    /// </remarks>
    public class FederatedAWSCredentials : RefreshingAWSCredentials
    {
        private const int MaxAuthenticationRetries = 3;
        private static readonly RegionEndpoint DefaultSTSClientRegion = RegionEndpoint.USEast1;
        private static readonly TimeSpan MaximumCredentialTimespan = TimeSpan.FromHours(1);
        private static readonly TimeSpan DefaultPreemptExpiryTime = TimeSpan.FromMinutes(5);

        private readonly SAMLRoleSessionManager sessionManager = new SAMLRoleSessionManager();

        /// <summary>
        /// Constructs an instance of FederatedAWSCredentials. After construction call GetCredentials
        /// to authenticate the user/process and obtain temporary AWS credentials.
        /// </summary>
        /// <param name="samlEndpoint">The SAML endpoint used for authentication.</param>
        /// <param name="roleArn">The role ARN used for authentication.</param>
        public FederatedAWSCredentials(SAMLEndpoint samlEndpoint, string roleArn)
            : this(samlEndpoint, roleArn, new FederatedAWSCredentialsOptions())
        {
        }

        /// <summary>
        /// Constructs an instance of FederatedAWSCredentials. After construction call GetCredentials
        /// to authenticate the user/process and obtain temporary AWS credentials.
        /// </summary>
        /// <param name="samlEndpoint">The SAML endpoint used for authentication.</param>
        /// <param name="roleArn">The role ARN used for authentication.</param>
        /// <param name="options">The options used for authentication.
        /// See <see cref="FederatedAWSCredentialsOptions"/> for details about available options.</param>
        public FederatedAWSCredentials(SAMLEndpoint samlEndpoint, string roleArn,
            FederatedAWSCredentialsOptions options)
        {
            if (string.IsNullOrEmpty(roleArn))
            {
                throw new ArgumentException("RoleArn must not be null or empty.");
            }

            Options = options ?? throw new ArgumentNullException("options");
            SAMLEndpoint = samlEndpoint ?? throw new ArgumentNullException("samlEndpoint");
            RoleArn = roleArn;
            PreemptExpiryTime = DefaultPreemptExpiryTime;
        }

        /// <summary>
        /// The SAML Endpoint used for authentication.
        /// </summary>
        public SAMLEndpoint SAMLEndpoint { get; private set; }

        /// <summary>
        /// The role ARN used for authentication.
        /// </summary>
        public string RoleArn { get; private set; }

        /// <summary>
        /// The options used for authentication.
        /// See <see cref="FederatedAWSCredentialsOptions"/> for details about available options.
        /// </summary>
        public FederatedAWSCredentialsOptions Options { get; private set; }

        /// <summary>
        /// Refresh credentials after expiry. If the role profile is configured with user identity
        /// information and a callback has been registered to obtain the user credential, the callback
        /// will be invoked ahead of authentication. For role profiles configured with user identity
        /// but no callback registration, the SDK will fall back to attempting to use the default
        /// user identity of the current process.
        /// </summary>
        /// <returns></returns>
        protected override CredentialsRefreshState GenerateNewCredentials()
        {
            Validate();
            // If the profile indicates the user has already authenticated and received
            // credentials which are still valid, adopt them instead of requiring a fresh
            // authentication.
            SAMLImmutableCredentials currentSession;
            if (TryGetRoleSession(out currentSession))
            {
                // Since cached role session credentials can be obtained, stored credentials exist 
                // and they were not expired. However, since the stored credentials are actual 
                // expiration time and not preempt expiration time we must preempt the expiration
                // to check that they will not expire before this call completes.
                var cachedState = new CredentialsRefreshState(currentSession, currentSession.Expires);

                // Use the cached credentials as long as they are not within the preempt expiry window
                // or have since actually expired since the prior call to TryGetRoleSession.
                //Verify the actual expiration is not within the preempt expiration time.
                if (!cachedState.IsExpiredWithin(PreemptExpiryTime))
                {
                    // The credentials have plenty of time left before they expire so they can be used. After
                    // return the expiration time will be preempted for future checks using ShouldUpdate.
                    return cachedState;
                }
            }

            CredentialsRefreshState newState = null;
            var attempts = 0;
            do
            {
                try
                {
                    NetworkCredential userCredential = null;
                    if (Options.UserIdentity != null)
                    {
                        if (Options.CredentialRequestCallback != null)
                        {
                            var callbackArgs = new CredentialRequestCallbackArgs
                            {
                                ProfileName = Options.ProfileName,
                                UserIdentity = Options.UserIdentity,
                                CustomState = Options.CustomCallbackState,
                                PreviousAuthenticationFailed = attempts > 0
                            };

                            userCredential = Options.CredentialRequestCallback(callbackArgs);

                            if (userCredential == null) // user declined to authenticate
                            {
                                throw new FederatedAuthenticationCancelledException(
                                    "User cancelled credential request.");
                            }
                        }
                        else
                        {
                            var logger = Logger.GetLogger(typeof(FederatedAWSCredentials));
                            logger.InfoFormat(
                                "FederatedAWSCredentials configured for a specific user but no credential request callback registered; falling back to default identity.");
                        }
                    }

                    newState = Authenticate(userCredential);
                }
                catch (FederatedAuthenticationFailureException)
                {
                    if (attempts < MaxAuthenticationRetries)
                        attempts++;
                    else
                        throw;
                }
            } while (newState == null && attempts < MaxAuthenticationRetries);

            return newState;
        }

        private CredentialsRefreshState Authenticate(ICredentials userCredential)
        {
            CredentialsRefreshState state;

            var region = Options.STSRegion;
            if (region == null && !string.IsNullOrEmpty(AWSConfigs.AWSRegion))
                region = RegionEndpoint.GetBySystemName(AWSConfigs.AWSRegion);
            else
                region = DefaultSTSClientRegion;

            ICoreAmazonSTS coreSTSClient = null;
            try
            {
                var stsConfig = ServiceClientHelpers.CreateServiceConfig(
                    ServiceClientHelpers.STS_ASSEMBLY_NAME, ServiceClientHelpers.STS_SERVICE_CONFIG_NAME);

                stsConfig.RegionEndpoint = region;
                if (Options.ProxySettings != null)
                {
                    stsConfig.SetWebProxy(Options.ProxySettings);
                }

                coreSTSClient = ServiceClientHelpers.CreateServiceFromAssembly<ICoreAmazonSTS>(
                    ServiceClientHelpers.STS_ASSEMBLY_NAME, ServiceClientHelpers.STS_SERVICE_CLASS_NAME,
                    new AnonymousAWSCredentials(), stsConfig);
            }
            catch (Exception e)
            {
                var msg = string.Format(CultureInfo.CurrentCulture,
                    "Assembly {0} could not be found or loaded. This assembly must be available at runtime to use this profile class.",
                    ServiceClientHelpers.STS_ASSEMBLY_NAME);
                throw new InvalidOperationException(msg, e);
            }

            var samlCoreSTSClient
#if NETSTANDARD
                = coreSTSClient as ICoreAmazonSTS_SAML;
            if (coreSTSClient == null)
            {
                throw new NotImplementedException("The currently loaded version of AWSSDK.SecurityToken doesn't support SAML authentication.");
            }
#else
                = coreSTSClient;
#endif

            try
            {
                var credentials = samlCoreSTSClient.CredentialsFromSAMLAuthentication(
                    SAMLEndpoint.EndpointUri.ToString(),
                    SAMLEndpoint.AuthenticationType.ToString(), RoleArn, MaximumCredentialTimespan, userCredential);

                RegisterRoleSession(credentials);

                state = new CredentialsRefreshState(credentials, credentials.Expires);
            }
            catch (Exception e)
            {
                var wrappedException =
                    new AmazonClientException("Credential generation from SAML authentication failed.", e);

                var logger = Logger.GetLogger(typeof(FederatedAWSCredentials));
                logger.Error(wrappedException, wrappedException.Message);

                throw wrappedException;
            }

            return state;
        }

        private string GetRoleSessionName()
        {
            if (string.IsNullOrEmpty(Options.ProfileName))
            {
                return SAMLEndpoint.Name + "," + RoleArn + "," + Options.UserIdentity;
            }
            else
            {
                return Options.ProfileName;
            }
        }

        private bool TryGetRoleSession(out SAMLImmutableCredentials sessionCredentials)
        {
            if (SAMLRoleSessionManager.IsAvailable)
            {
                return sessionManager.TryGetRoleSession(GetRoleSessionName(), out sessionCredentials);
            }
            else
            {
                sessionCredentials = null;
                return false;
            }
        }

        private void RegisterRoleSession(SAMLImmutableCredentials sessionCredentials)
        {
            if (SAMLRoleSessionManager.IsAvailable)
            {
                sessionManager.RegisterRoleSession(GetRoleSessionName(), sessionCredentials);
            }
        }

        /// <summary>
        /// Clears currently-stored credentials, forcing the next GetCredentials call to generate new credentials.
        /// </summary>
        public override void ClearCredentials()
        {
            // For Federated AWS credentials, this method does not force the generation of new credentials. Even though 
            // it clears the credential state to null, any cached credentials will immediately be picked up and used. 
            // Changing this behavior is a possible breaking change. Once we decide to make this change, the proper course 
            // of action would likely be to ensure the cache is cleared before setting the credential state to null. 
            base.ClearCredentials();
        }
    }
#endif
}