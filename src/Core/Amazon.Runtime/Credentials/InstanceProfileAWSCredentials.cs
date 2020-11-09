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
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using AWSSDK.Runtime.Internal.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace Amazon.Runtime
{
    /// <summary>
    /// Credentials that are retrieved from the Instance Profile service on an EC2 instance
    /// </summary>
    public class InstanceProfileAWSCredentials : URIBasedRefreshingCredentialHelper
    {
        #region Private members

        // Set preempt expiry to 15 minutes. New access keys are available at least 15 minutes before expiry time.
        // http://docs.aws.amazon.com/IAM/latest/UserGuide/role-usecase-ec2app.html
        private static TimeSpan _preemptExpiryTime = TimeSpan.FromMinutes(15);
        private static TimeSpan _refreshAttemptPeriod = TimeSpan.FromHours(1);

        private CredentialsRefreshState _currentRefreshState = null;
        private IWebProxy _proxy = null;

        #endregion

        #region Properties

        /// <summary>
        /// Role for which the credentials are retrieved
        /// </summary>
        public string Role { get; set; }

        #endregion


        #region Overrides

        protected override CredentialsRefreshState GenerateNewCredentials()
        {
            CredentialsRefreshState newState = null;
            var token = EC2InstanceMetadata.FetchApiToken();

            try
            {
                // Attempt to get early credentials. OK to fail at this point.
                newState = GetRefreshState(token);
            }
            catch (Exception e)
            {
                HttpStatusCode? httpStatusCode = ExceptionUtils.DetermineHttpStatusCode(e);

                if (httpStatusCode == HttpStatusCode.Unauthorized)
                {
                    EC2InstanceMetadata.ClearTokenFlag();
                    Logger.GetLogger(typeof(EC2InstanceMetadata)).Error(e, "EC2 Metadata service returned unauthorized for token based secure data flow.");
                    throw;
                }

                var logger = Logger.GetLogger(typeof(InstanceProfileAWSCredentials));
                logger.InfoFormat("Error getting credentials from Instance Profile service: {0}", e);
            }

            // If successful, save new credentials
            if (newState != null)
                _currentRefreshState = newState;

            // If still not successful (no credentials available at start), attempt once more to
            // get credentials, but now without swallowing exception
            if (_currentRefreshState == null)
            {
                try
                {
                    _currentRefreshState = GetRefreshState(token);
                }             
                catch (Exception e)
                {
                    HttpStatusCode? httpStatusCode = ExceptionUtils.DetermineHttpStatusCode(e);

                    if (httpStatusCode == HttpStatusCode.Unauthorized)
                    {
                        EC2InstanceMetadata.ClearTokenFlag();
                        Logger.GetLogger(typeof(EC2InstanceMetadata)).Error(e, "EC2 Metadata service returned unauthorized for token based secure data flow.");
                    }

                    throw;
                }
            }                

            // Return credentials that will expire in at most one hour
            CredentialsRefreshState state = GetEarlyRefreshState(_currentRefreshState);
            return state;
        }

        #endregion


        #region Constructors

        /// <summary>
        /// Constructs a InstanceProfileAWSCredentials object for specific role
        /// </summary>
        /// <param name="role">Role to use</param>
        public InstanceProfileAWSCredentials(string role)
            : this(role, null) { }

        /// <summary>
        /// Constructs a InstanceProfileAWSCredentials object for specific role
        /// </summary>
        /// <param name="role">Role to use</param>
        public InstanceProfileAWSCredentials(string role, IWebProxy proxy)
        {
            this._proxy = proxy;

            if (role == null)
                throw new ArgumentNullException(nameof(role));
            else if (IsNullOrWhiteSpace(role))
                throw new ArgumentException("The argument '" + nameof(role) + "' must contain a valid role name.");

            Role = role;
            this.PreemptExpiryTime = _preemptExpiryTime;
        }

        /// <summary>
        /// Constructs a InstanceProfileAWSCredentials object for the first found role
        /// </summary>
        public InstanceProfileAWSCredentials()
            : this(proxy: null) { }

        /// <summary>
        /// Constructs a InstanceProfileAWSCredentials object for the first found role
        /// </summary>
        public InstanceProfileAWSCredentials(IWebProxy proxy)
            : this(GetFirstRole(proxy), proxy) { }

        #endregion


        #region Public static methods

        /// <summary>
        /// Retrieves a list of all roles available through current InstanceProfile service
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetAvailableRoles()
        {
            return GetAvailableRoles(null);
        }

        /// <summary>
        /// Retrieves a list of all roles available through current InstanceProfile service
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetAvailableRoles(IWebProxy proxy)
        {
            var token = EC2InstanceMetadata.FetchApiToken();

            var allAliases = string.Empty;
            try
            {
                allAliases = GetContents(RolesUri, proxy, CreateMetadataTokenHeaders(token));
            }
            catch (Exception e)
            {
                HttpStatusCode? httpStatusCode = ExceptionUtils.DetermineHttpStatusCode(e);

                if (httpStatusCode == HttpStatusCode.Unauthorized)
                {
                    EC2InstanceMetadata.ClearTokenFlag();
                    Logger.GetLogger(typeof(EC2InstanceMetadata)).Error(e, "EC2 Metadata service returned unauthorized for token based secure data flow.");                    
                }

                throw;
            }
            if (string.IsNullOrEmpty(allAliases))
                yield break;

            string[] parts = allAliases.Split(AliasSeparators, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part in parts)
            {
                var trim = part.Trim();
                if (!string.IsNullOrEmpty(trim))
                    yield return trim;
            }
        }

        #endregion


        #region Private members

        private static string[] AliasSeparators = new string[] { "<br/>" };
        private static string Server = "http://169.254.169.254";
        private static string RolesPath = "/latest/meta-data/iam/security-credentials/";
        private static string InfoPath = "/latest/meta-data/iam/info";

        private static Uri RolesUri
        {
            get
            {
                return new Uri(Server + RolesPath);
            }
        }
        private Uri CurrentRoleUri
        {
            get
            {
                return new Uri(Server + RolesPath + Role);
            }
        }
        private static Uri InfoUri
        {
            get
            {
                return new Uri(Server + InfoPath);
            }
        }

        private CredentialsRefreshState GetEarlyRefreshState(CredentialsRefreshState state)
        {
            // New expiry time = Now + _refreshAttemptPeriod + PreemptExpiryTime
#pragma warning disable CS0612 // Type or member is obsolete
            var newExpiryTime = AWSSDKUtils.CorrectedUtcNow.ToLocalTime() + _refreshAttemptPeriod + PreemptExpiryTime;
#pragma warning restore CS0612 // Type or member is obsolete
                              // Use this only if the time is earlier than the default expiration time
            if (newExpiryTime.ToUniversalTime() > state.Expiration.ToUniversalTime())
                newExpiryTime = state.Expiration;

            return new CredentialsRefreshState(state.Credentials.Copy(), newExpiryTime);
        }

        private CredentialsRefreshState GetRefreshState(string token)
        {
            SecurityInfo info = GetServiceInfo(_proxy, token);
            if (!string.IsNullOrEmpty(info.Message))
            {
                throw new AmazonServiceException(string.Format(CultureInfo.InvariantCulture,
                    "Unable to retrieve credentials. Message = \"{0}\".",
                    info.Message));
            }
            SecurityCredentials credentials = GetRoleCredentials(token);

            CredentialsRefreshState refreshState = new CredentialsRefreshState(new ImmutableCredentials(credentials.AccessKeyId, credentials.SecretAccessKey, credentials.Token), credentials.Expiration);

            return refreshState;
        }

        private static SecurityInfo GetServiceInfo(IWebProxy proxy, string token)
        {
            CheckIsIMDSEnabled();            
            return GetObjectFromResponse<SecurityInfo>(InfoUri, proxy, CreateMetadataTokenHeaders(token));
        }

        private SecurityCredentials GetRoleCredentials(string token)
        {
            CheckIsIMDSEnabled();            
            return GetObjectFromResponse<SecurityCredentials>(CurrentRoleUri, _proxy, CreateMetadataTokenHeaders(token));
        }

        private static void CheckIsIMDSEnabled()
        {
            // keep this behavior consistent with GetObjectFromResponse case.
            if (!EC2InstanceMetadata.IsIMDSEnabled) throw new AmazonServiceException(string.Format(CultureInfo.InvariantCulture, "Unable to retrieve credentials."));
        }

        private static string GetFirstRole()
        {
            return GetFirstRole(null);
        }

        private static string GetFirstRole(IWebProxy proxy)
        {
            IEnumerable<string> roles = GetAvailableRoles(proxy);
            foreach (string role in roles)
            {
                return role;
            }

            // no roles found
            throw new InvalidOperationException("No roles found");
        }

        /// <summary>
        /// Return true if string is null or whitespace, false otherwise.
        /// We can't use String.IsNullOrWhitespace because it doesn't exist
        /// in all frameworks we support.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static bool IsNullOrWhiteSpace(string s)
        {
            if (s == null)
                return true;
            else if (s.Trim().Length == 0)
                return true;
            else
                return false;
        }

        private static Dictionary<string, string> CreateMetadataTokenHeaders(string token)
        {
            Dictionary<string, string> headers = null;            
            if (!string.IsNullOrEmpty(token))
            {
                headers = new Dictionary<string, string>();
                headers.Add(HeaderKeys.XAwsEc2MetadataToken, token);
            }

            return headers;
        }
                
        #endregion
    }
}
