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
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using System;
using System.IO;

namespace Amazon.Runtime
{
    /// <summary>
    /// <para>
    /// Credentials that are retrieved using a stored profile. 
    /// </para>
    /// <para>
    /// Unless otherwise specified priority is given to loading credentials from the SDK credential store 
    /// file which is shared between the SDK, PowerShell CLI and Toolkit. Credentials in profiles in this 
    /// file are encrypted and can only be used by the user account on the current machine that stored the 
    /// profile. Credentials can also be loaded from the plain-text ini-format credential file which is 
    /// shared with other AWS SDKs. This file is expected to exist in a '.aws' folder in the user's home 
    /// directory but alternate locations can be specified using either the AWSProfilesLocation setting in 
    /// the application configuration file, or by using the AWS_SHARED_CREDENTIALS_FILE environment variable.
    /// </para>
    /// <para>
    /// The profile name can be specified in the App.config using the AWSProfileName setting.
    /// </para>
    /// </summary>
    [Obsolete("This class is obsolete and will be removed in a future release. Please use Amazon.Runtime.CredentialManagement.NetSDKCredentialsFile or SharedCredentialsFile..  Visit http://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/net-dg-config-creds.html for further details.")]
    public class StoredProfileAWSCredentials : AWSCredentials
    {
        private AWSCredentials _wrappedCredentials;

        public AWSCredentials WrappedCredentials
        {
            get
            {
                return _wrappedCredentials;
            }
        }

        #region Public constructors

        /// <summary>
        /// Constructs an instance for credentials stored in a profile. This constructor searches for credentials 
        /// using the account name specified using the AWSProfileName setting, if set, in the application configuration 
        /// file. If the configuration file setting is not set the SDK will attempt to locate a profile with the name 
        /// 'default'.
        /// </summary>
        public StoredProfileAWSCredentials()
            : this(AWSConfigs.AWSProfileName)
        {

        }

        /// <summary>
        /// Constructs an instance for credentials stored in a profile with the specified name. The SDK will
        /// check the SDK credential store file first before looking for the shared ini-format credentials
        /// file unless the application configuration file contains a setting for the 'AWSProfilesLocation' 
        /// indicating the search should be constrained to the shared credentials file at the specified
        /// location.
        /// </summary>
        /// <param name="profileName">The name of the profile in which the credentials were stored.</param>
        public StoredProfileAWSCredentials(string profileName)
            : this(profileName, AWSConfigs.AWSProfilesLocation)
        {

        }

        /// <summary>
        /// Constructs an instance for credentials stored in a profile with the specified name.
        /// </summary>
        /// <param name="profileName">The profile name to search for credentials for</param>
        /// <param name="profilesLocation">
        /// Optional; instructs the SDK to check for the profile in the shared credentials file at the 
        /// specified location. If not set, the SDK will inspect its own credential store file first before
        /// attempting to locate a shared credential file using either the default location beneath the user's
        /// home profile folder or the location specified in the AWS_SHARED_CREDENTIALS_FILE environment 
        /// variable.
        /// </param>
        /// <remarks>
        /// If credential materials cannot be read or are invalid due to missing data an InvalidDataException 
        /// is thrown. If no credentials can be located with the specified profile name, an ArgumentException 
        /// is thrown.
        /// </remarks>
        public StoredProfileAWSCredentials(string profileName, string profilesLocation)
        {
            var lookupName = string.IsNullOrEmpty(profileName)
                    ? StoredProfileCredentials.DEFAULT_PROFILE_NAME
                    : profileName;

            ProfileName = lookupName;
            ProfilesLocation = null;

            // If not overriding the credentials lookup location check the SDK Store for credentials. If an override is being used then
            // assume the intent is to use the credentials file.
            if (string.IsNullOrEmpty(profilesLocation) && ProfileManager.IsProfileKnown(lookupName) && ProfileManager.IsAvailable)
            {
                if (ProfileManager.IsProfileKnown(lookupName) && AWSCredentialsProfile.CanCreateFrom(lookupName))
                {
                    _wrappedCredentials = ProfileManager.GetAWSCredentials(lookupName);
                    var logger = Logger.GetLogger(typeof(StoredProfileAWSCredentials));
                    logger.InfoFormat("Credentials found using account name {0} and looking in SDK account store.", lookupName);
                }
            }

            // If credentials weren't found in the SDK store then search the shared credentials file.
            if (this._wrappedCredentials == null)
            {
                var credentialsFilePath = StoredProfileCredentials.ResolveSharedCredentialFileLocation(profilesLocation);
                if (!string.IsNullOrEmpty(credentialsFilePath))
                {
                    var sharedCredentialsFile = new SharedCredentialsFile(credentialsFilePath);
                    CredentialProfile profile;
                    if (sharedCredentialsFile.TryGetProfile(lookupName, out profile)
                        && AWSCredentialsFactory.TryGetAWSCredentials(profile, sharedCredentialsFile, out _wrappedCredentials))
                    {
                        var logger = Logger.GetLogger(typeof(StoredProfileAWSCredentials));
                        logger.InfoFormat("Credentials found using account name {0} and looking in {1}.", lookupName, credentialsFilePath);
                    }

                    ProfilesLocation = credentialsFilePath;
                }
            }

            // No credentials found so error out.
            if (this._wrappedCredentials == null)
            {
                throw new ArgumentException("App.config does not contain credentials information. Either add the AWSAccessKey and AWSSecretKey properties or the AWSProfileName property.");
            }
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Name of the profile being used.
        /// </summary>
        public string ProfileName { get; private set; }

        /// <summary>
        /// Location of the profiles, if used.
        /// </summary>
        public string ProfilesLocation { get; private set; }

        #endregion

        /// <summary>
        /// Tests if a profile has been registered in either the SDK store or the specified credential 
        /// file.
        /// </summary>
        /// <param name="profileName">The name of the profile to test</param>
        /// <param name="profilesLocation">
        /// Optional; instructs the SDK to check for the profile in the shared credentials file at the 
        /// specified location. If not set, the SDK will inspect its own credential store file first before
        /// attempting to locate a shared credential file using either the default location beneath the user's
        /// home profile folder or the location specified in the AWS_SHARED_CREDENTIALS_FILE environment 
        /// variable.
        /// </param>
        /// <returns>True if a profile with the specified name has been registered.</returns>
        public static bool IsProfileKnown(string profileName, string profilesLocation)
        {
            if (string.IsNullOrEmpty(profilesLocation) && ProfileManager.IsProfileKnown(profileName))
            {
                return true;
            }
            else
            {
                return ValidCredentialsExistInSharedFile(profilesLocation, profileName);
            }
        }

        /// <summary>
        /// Tests if an instance can be created from the persisted profile data.
        /// If profilesLocation is null/empty, the SDK store is searched for the
        /// profile data before probing for the profile in the shared the ini-format
        /// credential file.
        /// </summary>
        /// <param name="profileName">The name of the profile to test</param>
        /// <param name="profilesLocation">
        /// If null/empty, the SDK store is searched for the named profile otherwise
        /// the ini-format credential file at the specified location is inspected.
        /// </param>
        /// <returns>True if the persisted data would yield a valid credentials instance.</returns>
        public static bool CanCreateFrom(string profileName, string profilesLocation)
        {
            if (string.IsNullOrEmpty(profilesLocation) && ProfileManager.IsProfileKnown(profileName))
            {
                return AWSCredentialsProfile.CanCreateFrom(profileName);
            }
            else
            {
                return ValidCredentialsExistInSharedFile(profilesLocation, profileName);
            }
        }

        private static bool ValidCredentialsExistInSharedFile(string profilesLocation, string profileName)
        {
            var credentialsFilePath = StoredProfileCredentials.ResolveSharedCredentialFileLocation(profilesLocation);
            if (!string.IsNullOrEmpty(credentialsFilePath))
            {
                var doLog = false;
                try
                {
                    var file = new SharedCredentialsFile(credentialsFilePath);
                    CredentialProfile profile = null;
                    if (file.TryGetProfile(profileName, out profile) && profile.CanCreateAWSCredentials)
                    {
                        return true;
                    }
                    else
                    {
                        doLog = true;
                    }
                }
                catch (InvalidDataException)
                {
                    doLog = true;
                }

                if (doLog)
                {
                    var logger = Logger.GetLogger(typeof(StoredProfileAWSCredentials));
                    logger.InfoFormat("Credentials file {0} does not contain a valid profile named {1}.", credentialsFilePath, profileName);
                }
            }
            else
            {
                var logger = Logger.GetLogger(typeof(StoredProfileAWSCredentials));
                logger.InfoFormat("Credentials file not found {0}.", credentialsFilePath);
            }
            return false;
        }

        #region Abstract class overrides

        /// <summary>
        /// Returns an instance of ImmutableCredentials for this instance
        /// </summary>
        /// <returns></returns>
        public override ImmutableCredentials GetCredentials()
        {
            return _wrappedCredentials.GetCredentials();
        }

        #endregion
    }
}
