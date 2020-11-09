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
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Amazon.Runtime
{
    /// <summary>
    /// Credentials that are retrieved from ConfigurationManager.AppSettings
    /// </summary>
    [Obsolete("This class is obsolete and will be removed in a future release. Please update your code to use AppConfigAWSCredentials instead.")]
    public class EnvironmentAWSCredentials : AWSCredentials
    {
        private const string ACCESSKEY = "AWSAccessKey";
        private const string SECRETKEY = "AWSSecretKey";

        private ImmutableCredentials _wrappedCredentials;

        #region Public constructors

        /// <summary>
        /// Constructs an instance of EnvironmentAWSCredentials and attempts
        /// to load AccessKey and SecretKey from ConfigurationManager.AppSettings
        /// </summary>
        public EnvironmentAWSCredentials()
        {
            NameValueCollection appConfig = ConfigurationManager.AppSettings;

            // Use hardcoded credentials
            if (!string.IsNullOrEmpty(appConfig[ACCESSKEY]) && !string.IsNullOrEmpty(appConfig[SECRETKEY]))
            {
                var accessKey = appConfig[ACCESSKEY];
                var secretKey = appConfig[SECRETKEY];
                this._wrappedCredentials = new ImmutableCredentials(accessKey, secretKey, null);
                var logger = Logger.GetLogger(typeof(EnvironmentAWSCredentials));
                logger.InfoFormat("Credentials found with {0} and {1} app settings", ACCESSKEY, SECRETKEY);
            }
            // Fallback to the StoredProfileAWSCredentials provider
            else
            {
                this._wrappedCredentials = new StoredProfileAWSCredentials().GetCredentials();
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
            return this._wrappedCredentials.Copy();
        }

        #endregion
    }
}
