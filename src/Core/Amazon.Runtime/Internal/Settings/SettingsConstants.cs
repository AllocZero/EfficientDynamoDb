/*******************************************************************************
 *  Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *  this file except in compliance with the License. A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 *  or in the "license" file accompanying this file.
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *  specific language governing permissions and limitations under the License.
 * *****************************************************************************
 *    __  _    _  ___
 *   (  )( \/\/ )/ __)
 *   /__\ \    / \__ \
 *  (_)(_) \/\/  (___/
 *
 *  AWS SDK for .NET
 */

namespace Amazon.Runtime.Internal.Settings
{
    public static class SettingsConstants
    {
        public const string UserPreferences = "UserPreferences";

        public const string MiscSettings = "MiscSettings";

        public const string RegisteredProfiles = "RegisteredAccounts";
        public const string RegisteredSAMLEndpoints = "SAMLEndpoints";
        public const string RegisteredRoleSessions = "RoleSessions";

        public const string RecentUsages = "RecentUsages";

        public const string ProfileTypeField = "ProfileType";
        public const string DisplayNameField = "DisplayName";
        public const string AccountNumberField = "AccountNumber";
        public const string Restrictions = "Restrictions";

        // present in profiles for AWS credentials
        // some keys are used for multiple types of profiles
        public const string AccessKeyField = "AWSAccessKey";
        public const string SecretKeyField = "AWSSecretKey";
        public const string SessionTokenField = "SessionToken";
        public const string ExternalIDField = "ExternalId";
        public const string MfaSerialField = "MfaSerial";
        public const string EndpointNameField = "EndpointName";
        public const string RoleArnField = "RoleArn";
        public const string RoleSessionName = "RoleSessionName";
        public const string UserIdentityField = "UserIdentity";
        public const string Region = "Region";
        public const string AuthenticationTypeField = "AuthenticationType";
        public const string SourceProfileField = "SourceProfile";
        public const string CredentialSourceField = "CredentialSource";
        public const string CredentialProcess = "credential_process";
        public const string WebIdentityTokenFile = "WebIdentityTokenFile";

        // present in endpoint definitions in SAMLEndpoints.json file
        public const string EndpointField = "Endpoint";

        // session data for a role profile, persisted in RoleSessions.json file.
        public const string RoleSession = "RoleSession";

        public const string SecretKeyRepository = "SecretKeyRepository";

        public const string LastAcountSelectedKey = "LastAcountSelectedKey";

        public const string VersionCheck = "VersionCheck";
        public const string LastVersionDoNotRemindMe = "LastVersionDoNotRemindMe";

        public const string HostedFilesLocation = "HostedFilesLocation";

        public const string EC2ConnectSettings = "EC2ConnectSettings";

        public const string EC2InstanceUseKeyPair = "EC2RDPUseKeyPair";
        public const string EC2InstanceMapDrives = "EC2RDPMapDrives";

        public const string EC2InstanceSaveCredentials = "EC2InstanceSaveCredentials";
        public const string EC2InstanceUserName = "EC2InstanceUserName";
        public const string EC2InstancePassword = "EC2InstancePassword";


        public const string ProxySettings = "ProxySettings";
        public const string ProxyHost = "ProxyHost";
        public const string ProxyPort = "ProxyPort";

        public const string ProxyUsernameObsolete = "ProxyUsername";
        public const string ProxyPasswordObsolete = "ProxyPassword";

        public const string ProxyUsernameEncrypted = "ProxyUsernameEncrypted";
        public const string ProxyPasswordEncrypted = "ProxyPasswordEncrypted";
    }
}
