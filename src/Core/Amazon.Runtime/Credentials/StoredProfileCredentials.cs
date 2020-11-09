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
using System.IO;
using System.Text;

namespace Amazon.Runtime
{
    /// <summary>
    /// Helper routiners for AWS and Federated credential profiles. Probes the
    /// profile type for the supplied profile name and returns the appropriate profile 
    /// instance.
    /// </summary>
    [Obsolete("This class is obsolete and will be removed in a future release. Please use Amazon.Runtime.CredentialManagement.NetSDKCredentialsFile or SharedCredentialsFile.  Visit http://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html for further details.")]
    public abstract class StoredProfileCredentials
    {
        public const string DEFAULT_PROFILE_NAME = "default";
        public const string SHARED_CREDENTIALS_FILE_ENVVAR = "AWS_SHARED_CREDENTIALS_FILE";

        private static string[] PotentialEnvironmentPathsToCredentialsFile = new string[]{
            "HOME",
            "USERPROFILE",
        };

        public const string DefaultSharedCredentialFilename = "credentials";
        public const string DefaultSharedCredentialLocation = ".aws/" + DefaultSharedCredentialFilename;

        /// <summary>
        /// Determines the type of the requested profile and returns the
        /// appropriate profile instance.
        /// </summary>
        /// <param name="profileName">The name of the profile (AWS or federated) to be loaded.</param>
        /// <returns>Instantiated profile type.</returns>
        public static AWSCredentials GetProfile(string profileName)
        {
            return GetProfile(profileName, AWSConfigs.AWSProfilesLocation);
        }

        /// <summary>
        /// Determines the type of the requested profile and returns the
        /// appropriate profile instance.
        /// </summary>
        /// <param name="profileName">The name of the profile (AWS or federated) to be loaded.</param>
        /// <param name="profileLocation">
        /// The location of the shared credentials (.ini) file, for profiles that are not stored in the
        /// SDK credential store.
        /// </param>
        /// <returns>Instantiated profile type.</returns>
        public static AWSCredentials GetProfile(string profileName, string profileLocation)
        {
            if (StoredProfileAWSCredentials.CanCreateFrom(profileName, profileLocation))
                return new StoredProfileAWSCredentials(profileName, profileLocation);

#if !NETSTANDARD
            if (StoredProfileFederatedCredentials.CanCreateFrom(profileName, profileLocation))
                return new StoredProfileFederatedCredentials(profileName, profileLocation);
#endif
            var sb = new StringBuilder();
            sb.AppendFormat(CultureInfo.InvariantCulture, "Profile {0} was not found in the SDK credential store", profileName);
            if (!string.IsNullOrEmpty(profileLocation))
                sb.AppendFormat(CultureInfo.InvariantCulture, " or at location '{0}'.", profileLocation);

            throw new ArgumentException(sb.ToString());
        }

        /// <summary>
        /// Probes for and returns the fully qualified name of the shared ini-format credentials
        /// file. 
        /// </summary>
        /// <param name="profileLocation">
        /// Contains the file or folder name of the credential file. If not specified, the 
        /// routine will first check the application configuration file for a setting indicating
        /// the file location or filename. If the configuration file does not yield a credential
        /// file location then an environment variable is examined. Finally the routine will 
        /// inspect the fallback default location beneath the user's home folder location.
        /// </param>
        /// <returns>
        /// The fully qualified name to the credential file that was located, or null
        /// if no credential file could be found.
        /// </returns>
        public static string ResolveSharedCredentialFileLocation(string profileLocation)
        {
            var logger = Logger.GetLogger(typeof(StoredProfileCredentials));

            string credentialFile = TestSharedCredentialFileExists(profileLocation);
            if (!string.IsNullOrEmpty(credentialFile))
            {
                logger.InfoFormat("Credentials file found at supplied location: {0}", credentialFile);
                return credentialFile;
            }

            credentialFile = TestSharedCredentialFileExists(AWSConfigs.AWSProfilesLocation);
            if (!string.IsNullOrEmpty(credentialFile))
            {
                logger.InfoFormat("Credentials file found using application configuration setting: {0}", credentialFile);
                return credentialFile;
            }

            credentialFile = TestSharedCredentialFileExists(Environment.GetEnvironmentVariable(SHARED_CREDENTIALS_FILE_ENVVAR));
            if (!string.IsNullOrEmpty(credentialFile))
            {
                logger.InfoFormat("Credentials file found using environment variable '{0}': {1}", SHARED_CREDENTIALS_FILE_ENVVAR, credentialFile);
                return credentialFile;
            }

            foreach (string environmentVariable in PotentialEnvironmentPathsToCredentialsFile)
            {
                string envPath = Environment.GetEnvironmentVariable(environmentVariable);
                if (!string.IsNullOrEmpty(envPath))
                {
                    credentialFile = TestSharedCredentialFileExists(Path.Combine(envPath, DefaultSharedCredentialLocation));
                    if (!string.IsNullOrEmpty(credentialFile))
                    {
                        logger.InfoFormat("Credentials file found using environment variable '{0}': {1}", environmentVariable, credentialFile);
                        return credentialFile;
                    }
                }
            }
#if BCL45 || (NETSTANDARD && !NETSTANDARD13)
            var profileFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (!string.IsNullOrEmpty(profileFolder))
            {
                credentialFile = TestSharedCredentialFileExists(Path.Combine(profileFolder, DefaultSharedCredentialLocation));
                if (!string.IsNullOrEmpty(credentialFile))
                {
                    logger.InfoFormat("Credentials file found in user profile location: {0}", credentialFile);
                    return credentialFile;
                }
            }
#elif BCL35

            var profileFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            if (!string.IsNullOrEmpty(profileFolder))
            {
                var parent = Directory.GetParent(profileFolder);
                if (parent != null)
                {
                    credentialFile = TestSharedCredentialFileExists(Path.Combine(parent.FullName, DefaultSharedCredentialLocation));
                    if (!string.IsNullOrEmpty(credentialFile))
                    {
                        logger.InfoFormat("Credentials file found in user profile location: {0}", credentialFile);
                        return credentialFile;
                    }
                }
            }
#endif

            logger.InfoFormat("No credentials file found using location probing.");
            return null;
        }

        private static string TestSharedCredentialFileExists(string pathOrFilename)
        {
            if (!string.IsNullOrEmpty(pathOrFilename))
            {
                string testLocation = Directory.Exists(pathOrFilename)
                        ? Path.Combine(pathOrFilename, DefaultSharedCredentialFilename)
                        : pathOrFilename;

                if (File.Exists(testLocation))
                    return testLocation;
            }

            return null;
        }
    }
}
