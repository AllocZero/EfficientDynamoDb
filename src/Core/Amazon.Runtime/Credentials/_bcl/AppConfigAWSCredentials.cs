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
using Amazon.Runtime.CredentialManagement.Internal;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;

namespace Amazon.Runtime
{
    /// <summary>
    /// Obtains credentials from access key/secret key or AWSProfileName settings
    /// in the application's app.config or web.config file.
    /// </summary>
    public class AppConfigAWSCredentials : AWSCredentials
    {
        private const string ACCESSKEY = "AWSAccessKey";
        private const string SECRETKEY = "AWSSecretKey";

        private AWSCredentials _wrappedCredentials;

        #region Public constructors 

        public AppConfigAWSCredentials()
        {
            NameValueCollection appConfig = ConfigurationManager.AppSettings;
            var logger = Logger.GetLogger(typeof(AppConfigAWSCredentials));

            // Attempt hardcoded key credentials first, then look for an explicit profile name
            // in either the SDK credential store or the shared credentials file. When using a profile
            // name, if a location is not given the search will use the default locations and name for
            // the credential file (assuming the profile is not found in the SDK store first)
            if (!string.IsNullOrEmpty(appConfig[ACCESSKEY]) && !string.IsNullOrEmpty(appConfig[SECRETKEY]))
            {
                var accessKey = appConfig[ACCESSKEY];
                var secretKey = appConfig[SECRETKEY];
                this._wrappedCredentials = new BasicAWSCredentials(accessKey, secretKey);
                logger.InfoFormat("Credentials found with {0} and {1} app settings", ACCESSKEY, SECRETKEY);
            }
            else if (!string.IsNullOrEmpty(AWSConfigs.AWSProfileName))
            {
                CredentialProfileStoreChain chain = new CredentialProfileStoreChain(AWSConfigs.AWSProfilesLocation);
                CredentialProfile profile;
                if (chain.TryGetProfile(AWSConfigs.AWSProfileName, out profile))
                {
                    // Will throw a descriptive exception if profile.CanCreateAWSCredentials is false.
                    _wrappedCredentials = profile.GetAWSCredentials(profile.CredentialProfileStore, true);
                }
            }

            if (this._wrappedCredentials == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "The app.config/web.config files for the application did not contain credential information"));
            }
        }
        #endregion

        #region Abstract class overrides

        /// <summary>
        /// Returns an instance of ImmutableCredentials for this instance
        /// </summary>
        /// <returns></returns>
        public override ImmutableCredentials GetCredentials()
        {
            return this._wrappedCredentials.GetCredentials();
        }

        #endregion
    }
}
