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
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Util;
using Amazon.Runtime.SharedInterfaces;
using Amazon.Util;
using System;
using System.Globalization;
using System.Net;

namespace Amazon.Runtime
{
    /// <summary>
    /// Temporary credentials that are created following successful authentication with
    /// a federated endpoint supporting SAML.
    /// </summary>
    /// <remarks>
    /// 1. Currently only the SDK store supports profiles that contain the necessary data to support
    /// authentication and role-based credential generation. The ini-format files used by the AWS CLI
    /// and some other SDKs are not supported at this time.
    /// <br/>
    /// 2. In order to use the StoredProfileFederatedCredentials class the AWSSDK.SecurityToken assembly
    /// must be available to your application at runtime.
    /// </remarks>
    [Obsolete("This class is obsolete and will be removed in a future release. Please use Amazon.Runtime.FederatedAWSCredentials.  Visit http://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html for further details.")]
    public class StoredProfileFederatedCredentials : RefreshingAWSCredentials
    {
        #region Private data

        private object _synclock = new object();

        private RegionEndpoint DefaultSTSClientRegion = RegionEndpoint.USEast1;

        private const int MaxAuthenticationRetries = 3;

        private static readonly TimeSpan _preemptExpiryTime = TimeSpan.FromMinutes(5);
        private TimeSpan _credentialDuration = MaximumCredentialTimespan;

        private RequestUserCredential _credentialRequestCallback = null;
        private object _customCallbackState = null;

        private WebProxy _proxySettings = null;

        #endregion

        #region Public properties

        /// <summary>
        /// Custom state to return to the registered callback to handle credential requests.
        /// The data will be contained in the CredentialRequestCallbackArgs instance supplied 
        /// to the callback.
        /// </summary>
        public object CustomCallbackState
        {
            get
            {
                lock (_synclock)
                {
                    return _customCallbackState;
                }
            }
        }

        /// <summary>
        /// The minimum allowed timespan for generated credentials, per STS documentation.
        /// </summary>
        public static readonly TimeSpan MinimumCredentialTimespan = TimeSpan.FromMinutes(15);

        /// <summary>
        /// The maximum allowed timespan for generated credentials, per STS documentation.
        /// </summary>
        public static readonly TimeSpan MaximumCredentialTimespan = TimeSpan.FromHours(1);

        /// <summary>
        /// Name of the profile being used.
        /// </summary>
        public string ProfileName { get; private set; }

        /// <summary>
        /// Location of the profiles, if used.
        /// </summary>
        public string ProfilesLocation { get; private set; }

        /// <summary>
        /// The data about the SAML endpoint and any required user credentials parsed from the
        /// profile.
        /// </summary>
        public SAMLRoleProfile ProfileData { get; private set; }

        /// <summary>
        /// Callback signature for obtaining user credentials that may be needed for authentication.
        /// </summary>
        /// <param name="args">
        /// Data about the credential demand including any custom state data that was supplied
        /// when the callback was registered.
        /// </param>
        /// <returns>
        /// The network credential to use in user authentication. Return null to signal the user
        /// declined to provide credentials and authentication should not proceed.
        /// </returns>
        public delegate NetworkCredential RequestUserCredential(CredentialRequestCallbackArgs args);

        #endregion

        #region Public constructors

        /// <summary>
        /// Constructs an instance of StoredProfileFederatedCredentials using the profile name specified
        /// in the App.config. If no profile name is specified then the default credentials are used.
        /// </summary>
        public StoredProfileFederatedCredentials()
            : this(AWSConfigs.AWSProfileName)
        {

        }

        /// <summary>
        /// Constructs an instance of StoredProfileFederatedCredentials. Credentials will be searched
        /// for using the profileName parameter.
        /// </summary>
        /// <param name="profileName">The profile name to search for credentials for</param>
        public StoredProfileFederatedCredentials(string profileName)
            : this(profileName, AWSConfigs.AWSProfilesLocation)
        {

        }

        /// <summary>
        /// <para>
        /// Constructs an instance of StoredProfileFederatedCredentials. After construction call one of the Authenticate
        /// methods to authenticate the user/process and obtain temporary AWS credentials.
        /// </para>
        /// <para>
        /// For users who are domain joined (the role profile does not contain user identity information) the temporary
        /// credentials will be refreshed automatically as needed. Non domain-joined users (those with user identity
        /// data in the profile) are required to re-authenticate when credential refresh is required. An exception is
        /// thrown when attempt is made to refresh credentials in this scenario. The consuming code of this class
        /// should catch the exception and prompt the user for credentials, then call Authenticate to re-initialize
        /// with a new set of temporary AWS credentials.
        /// </para>
        /// </summary>
        /// <param name="profileName">
        /// The name of the profile holding the necessary role data to enable authentication and credential generation.
        /// </param>
        /// <param name="profilesLocation">Reserved for future use.</param>
        /// <remarks>
        /// The ini-format credentials file is not currently supported for SAML role profiles.
        /// </remarks>
        public StoredProfileFederatedCredentials(string profileName, string profilesLocation)
            : this(profileName, profilesLocation, null)
        {

        }

        /// <summary>
        /// <para>
        /// Constructs an instance of StoredProfileFederatedCredentials. After construction call one of the Authenticate
        /// methods to authenticate the user/process and obtain temporary AWS credentials.
        /// </para>
        /// <para>
        /// For users who are domain joined (the role profile does not contain user identity information) the temporary
        /// credentials will be refreshed automatically as needed. Non domain-joined users (those with user identity
        /// data in the profile) are required to re-authenticate when credential refresh is required. An exception is
        /// thrown when attempt is made to refresh credentials in this scenario. The consuming code of this class
        /// should catch the exception and prompt the user for credentials, then call Authenticate to re-initialize
        /// with a new set of temporary AWS credentials.
        /// </para>
        /// </summary>
        /// <param name="profileName">
        /// The name of the profile holding the necessary role data to enable authentication and credential generation.
        /// </param>
        /// <param name="profilesLocation">Reserved for future use.</param>
        /// <param name="proxySettings">
        /// Null or proxy settings to be used during the HHTPS authentication calls when generating credentials.
        /// /// </param>
        /// <remarks>The ini-format credentials file is not currently supported for SAML role profiles.</remarks>
        public StoredProfileFederatedCredentials(string profileName, string profilesLocation, WebProxy proxySettings)
        {
            this._proxySettings = proxySettings;
            this.PreemptExpiryTime = _preemptExpiryTime;

            var lookupName = string.IsNullOrEmpty(profileName)
                ? StoredProfileCredentials.DEFAULT_PROFILE_NAME
                : profileName;

            ProfileName = lookupName;
            ProfilesLocation = null;

            // If not overriding the credentials lookup location check the SDK Store for credentials. If
            // an override location is specified, assume we should only use the shared credential file.
            if (string.IsNullOrEmpty(profilesLocation))
            {
                if (ProfileManager.IsProfileKnown(lookupName) && SAMLRoleProfile.CanCreateFrom(lookupName))
                {
                    var profileData = ProfileManager.GetProfile<SAMLRoleProfile>(lookupName);
                    ProfileData = profileData;
                    var logger = Logger.GetLogger(typeof(StoredProfileFederatedCredentials));
                    logger.InfoFormat("SAML role profile found using account name {0} and looking in SDK account store.", lookupName);
                }
            }

            // we currently do not support the shared ini-format credential file for SAML role profile data
            // so end the search now if not found
            if (ProfileData == null)
            {
                var msg = string.Format(CultureInfo.InvariantCulture,
                                        "Profile '{0}' was not found or could not be loaded from the SDK credential store. Verify that the profile name and data are correct.",
                                        profileName);
                throw new ArgumentException(msg);
            }
        }

        #endregion

        /// <summary>
        /// <para>
        /// Registers a callback handler for scenarios where credentials need to be supplied
        /// during user authentication (primarily the non-domain-joined use case). Custom data,
        /// which will be supplied in the CredentialRequestCallbackArgs instance passed to the
        /// callback, can also be supplied.
        /// </para>
        /// <para>
        /// The callback will only be invoked if the underlying SAML role profile indicates it
        /// was set up for use with a specific identity. For profiles that do not contain any user
        /// identity the SDK will default to using the identity of the current process during
        /// authentication. Additionally, if the profile contain user identity information but no
        /// callback has been registered, the SDK will also attempt to use the current process
        /// identity during authentication.
        /// </para>
        /// </summary>
        /// <param name="callback">The handler to be called</param>
        /// <param name="customData">
        /// Custom state data to be supplied in the arguments to the callback.
        /// </param>
        /// <remarks>
        /// Only one callback handler can be registered. The call to the handler will be made on
        /// whatever thread is executing at the time a demand to provide AWS credentials is made.
        /// If the handler code requires that UI need to be displayed, the handler should 
        /// transition to the UI thread as appropriate.
        /// </remarks>
        public void SetCredentialCallbackData(RequestUserCredential callback, object customData)
        {
            lock (_synclock)
            {
                _credentialRequestCallback = callback;
                _customCallbackState = customData;
            }
        }

        /// <summary>
        /// Tests if an instance can be created from the persisted profile data.
        /// </summary>
        /// <param name="profileName">The name of the profile to test.</param>
        /// <param name="profilesLocation">The location of the shared ini-format credential file.</param>
        /// <returns>True if the persisted data would yield a valid credentials instance.</returns>
        /// <remarks>
        /// This profile type is currently only supported in the SDK credential store file.
        /// The shared ini-format file is not currently supported; any value supplied
        /// for the profilesLocation value is ignored.
        /// </remarks>
        public static bool CanCreateFrom(string profileName, string profilesLocation)
        {
            if (string.IsNullOrEmpty(profilesLocation) && ProfileManager.IsProfileKnown(profileName))
                return SAMLRoleProfile.CanCreateFrom(profileName);

            return false;
        }


        /// <summary>
        /// Performs any additional validation we may require on the profile content.
        /// </summary>
        protected override void Validate()
        {
        }

        /// <summary>
        /// Refresh credentials after expiry. If the role profile is configured with user identity
        /// information and a callback has been registered to obtain the user credential the callback
        /// will be invoked ahead of authentication. For role profiles configured with user identity
        /// but no callback registration, the SDK will fall back to attempting to use the default
        /// user identity of the current process.
        /// </summary>
        /// <returns></returns>
        protected override CredentialsRefreshState GenerateNewCredentials()
        {
            Validate();

            // lock across the entire process for generating credentials so multiple
            // threads don't attempt to invoke any registered callback at the same time
            // and if we do callback, we only do it once to get the user authentication
            // data
            lock (_synclock)
            {
                // If the profile indicates the user has already authenticated and received
                // credentials which are still valid, adopt them instead of requiring a fresh
                // authentication
                var currentSession = ProfileData.GetCurrentSession();
                if (currentSession != null)
                    return new CredentialsRefreshState(currentSession, currentSession.Expires);

                CredentialsRefreshState newState = null;
                var attempts = 0;
                do
                {
                    try
                    {
                        NetworkCredential userCredential = null;
                        if (!ProfileData.UseDefaultUserIdentity)
                        {
                            if (_credentialRequestCallback != null)
                            {
                                var callbackArgs = new CredentialRequestCallbackArgs
                                {
                                    ProfileName = ProfileData.Name,
                                    UserIdentity = ProfileData.UserIdentity,
                                    CustomState = CustomCallbackState,
                                    PreviousAuthenticationFailed = attempts > 0
                                };

                                userCredential = _credentialRequestCallback(callbackArgs);

                                if (userCredential == null) // user declined to authenticate
                                    throw new FederatedAuthenticationCancelledException("User cancelled credential request.");
                            }
                            else
                            {
                                var logger = Logger.GetLogger(typeof(StoredProfileFederatedCredentials));
                                logger.InfoFormat("Role profile {0} is configured for a specific user but no credential request callback registered; falling back to default identity.", ProfileName);
                            }
                        }

                        newState = Authenticate(userCredential, _credentialDuration);
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
        }

        private CredentialsRefreshState Authenticate(ICredentials userCredential, TimeSpan credentialDuration)
        {
            CredentialsRefreshState state;

            var configuredRegion = !string.IsNullOrEmpty(ProfileData.Region) ? ProfileData.Region : AWSConfigs.AWSRegion;
            var region = string.IsNullOrEmpty(configuredRegion)
                                ? DefaultSTSClientRegion
                                : RegionEndpoint.GetBySystemName(configuredRegion);

            ICoreAmazonSTS coreSTSClient = null;

            try
            {
                var stsConfig = ServiceClientHelpers.CreateServiceConfig(ServiceClientHelpers.STS_ASSEMBLY_NAME,
                                                                         ServiceClientHelpers.STS_SERVICE_CONFIG_NAME);
                stsConfig.RegionEndpoint = region;
                if (_proxySettings != null)
                    stsConfig.SetWebProxy(_proxySettings);

                coreSTSClient
                    = ServiceClientHelpers.CreateServiceFromAssembly<ICoreAmazonSTS>(ServiceClientHelpers.STS_ASSEMBLY_NAME,
                                                                                     ServiceClientHelpers.STS_SERVICE_CLASS_NAME,
                                                                                     new AnonymousAWSCredentials(),
                                                                                     stsConfig);
            }
            catch (Exception e)
            {
                var msg = string.Format(CultureInfo.CurrentCulture,
                                        "Assembly {0} could not be found or loaded. This assembly must be available at runtime to use this profile class.",
                                        ServiceClientHelpers.STS_ASSEMBLY_NAME);
                throw new InvalidOperationException(msg, e);
            }

            try
            {
                var credentials
                    = coreSTSClient.CredentialsFromSAMLAuthentication(ProfileData.EndpointSettings.Endpoint.ToString(),
                                                                      ProfileData.EndpointSettings.AuthenticationType,
                                                                      ProfileData.RoleArn,
                                                                      credentialDuration,
                                                                      userCredential);

                ProfileData.PersistSession(credentials);

                state = new CredentialsRefreshState(credentials, credentials.Expires);
            }
            catch (Exception e)
            {
                var wrappedException = new AmazonClientException("Credential generation from SAML authentication failed.", e);

                var logger = Logger.GetLogger(typeof(StoredProfileFederatedCredentials));
                logger.Error(wrappedException, wrappedException.Message);

                throw wrappedException;
            }

            return state;
        }
    }
}
