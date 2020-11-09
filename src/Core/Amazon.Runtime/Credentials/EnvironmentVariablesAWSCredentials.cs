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
using System.Globalization;

namespace Amazon.Runtime
{
    /// <summary>
    /// Uses aws credentials stored in environment variables to construct the credentials object.
    /// AWS_ACCESS_KEY_ID and AWS_SECRET_ACCESS_KEY are used for the access key id and secret key. 
    /// If the variable AWS_SESSION_TOKEN exists then it will be used to create temporary session 
    /// credentials.
    /// </summary>
    /// <remarks>
    /// For backwards compatibility the class will also attempt to discover the secret key from
    /// the AWS_SECRET_KEY variable, if a value cannot be obtained using the standard 
    /// AWS_SECRET_ACCESS_KEY variable.
    /// </remarks>
    public class EnvironmentVariablesAWSCredentials : AWSCredentials
    {
        // these variable names are standard across all AWS SDKs that support reading keys from
        // environment variables
        public const string ENVIRONMENT_VARIABLE_ACCESSKEY = "AWS_ACCESS_KEY_ID";
        public const string ENVIRONMENT_VARIABLE_SECRETKEY = "AWS_SECRET_ACCESS_KEY";
        public const string ENVIRONMENT_VARIABLE_SESSION_TOKEN = "AWS_SESSION_TOKEN";

        // this legacy key was used by previous versions of the AWS SDK for .NET and is
        // used if no value exists for the standard key for backwards compatibility.
        public const string LEGACY_ENVIRONMENT_VARIABLE_SECRETKEY = "AWS_SECRET_KEY";

        private Logger logger;

        #region Public constructors

        /// <summary>
        /// Constructs an instance of EnvironmentVariablesAWSCredentials. If no credentials are found in 
        /// the environment variables then an InvalidOperationException is thrown.
        /// </summary>
        public EnvironmentVariablesAWSCredentials()
        {
            logger = Logger.GetLogger(typeof(EnvironmentVariablesAWSCredentials));

            // We need to do an initial fetch to validate that we can use environment variables to get the credentials.
            FetchCredentials();
        }

        #endregion

        /// <summary>
        /// Creates immutable credentials from environment variables.
        /// </summary>
        /// <returns></returns>
        public ImmutableCredentials FetchCredentials()
        {
            string accessKeyId = Environment.GetEnvironmentVariable(ENVIRONMENT_VARIABLE_ACCESSKEY);
            string secretKey = Environment.GetEnvironmentVariable(ENVIRONMENT_VARIABLE_SECRETKEY);
            if (string.IsNullOrEmpty(secretKey))
            {
                secretKey = Environment.GetEnvironmentVariable(LEGACY_ENVIRONMENT_VARIABLE_SECRETKEY);
                if (!string.IsNullOrEmpty(secretKey))
                    logger.InfoFormat("AWS secret key found using legacy and non-standard environment variable '{0}', consider updating to the cross-SDK standard variable '{1}'.",
                                      LEGACY_ENVIRONMENT_VARIABLE_SECRETKEY, ENVIRONMENT_VARIABLE_SECRETKEY);
            }

            if (string.IsNullOrEmpty(accessKeyId) || string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "The environment variables {0}/{1}/{2} were not set with AWS credentials.",
                    ENVIRONMENT_VARIABLE_ACCESSKEY, ENVIRONMENT_VARIABLE_SECRETKEY, ENVIRONMENT_VARIABLE_SESSION_TOKEN));
            }

            string sessionToken = Environment.GetEnvironmentVariable(ENVIRONMENT_VARIABLE_SESSION_TOKEN);

            logger.InfoFormat("Credentials found using environment variables.");

            return new ImmutableCredentials(accessKeyId, secretKey, sessionToken);
        }

        /// <summary>
        /// Returns an instance of ImmutableCredentials for this instance
        /// </summary>
        /// <returns></returns>
        public override ImmutableCredentials GetCredentials()
        {
            return FetchCredentials();
        }
    }
}
