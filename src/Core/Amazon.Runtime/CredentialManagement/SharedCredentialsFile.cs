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
using Amazon.Runtime.CredentialManagement.Internal;
using Amazon.Runtime.Internal.Util;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace Amazon.Runtime.CredentialManagement
{
    /// <summary>
    /// Provides access to read and write to the shared credentials INI file.
    /// The file is read, parsed, and validated at construction time.
    /// Changes can be made using the RegisterProfile() and
    /// UnregisterProfile() methods.
    ///
    /// This class is not threadsafe.
    /// </summary>
    public class SharedCredentialsFile : ICredentialProfileStore
    {
        public const string DefaultProfileName = "default";

        private const string ToolkitArtifactGuidField = "toolkit_artifact_guid";
        private const string RegionField = "region";
        private const string EndpointDiscoveryEnabledField = "endpoint_discovery_enabled";
        private const string ConfigFileName = "config";
        private const string DefaultDirectoryName = ".aws";
        private const string DefaultFileName = "credentials";
        private const string CredentialProcess = "credential_process";
        private const string StsRegionalEndpointsField = "sts_regional_endpoints";
        private const string S3UseArnRegionField = "s3_use_arn_region";
        private const string S3RegionalEndpointField = "s3_us_east_1_regional_endpoint";
        private const string RetryModeField = "retry_mode";
        private const string MaxAttemptsField = "max_attempts";
        private readonly Logger _logger = Logger.GetLogger(typeof(SharedCredentialsFile));

        private static readonly HashSet<string> ReservedPropertyNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ToolkitArtifactGuidField,
            RegionField,
            EndpointDiscoveryEnabledField,
            CredentialProcess,
            StsRegionalEndpointsField,
            S3UseArnRegionField,
            S3RegionalEndpointField,
            RetryModeField,
            MaxAttemptsField
        };

        /// <summary>
        /// To maintain compatibility with the CLI,
        /// SharedCredentialsFile doesn't support the SAML profileTypes.
        /// </summary>
        private static readonly HashSet<CredentialProfileType> ProfileTypeWhitelist =
            new HashSet<CredentialProfileType>()
            {
                CredentialProfileType.AssumeRole,
                CredentialProfileType.AssumeRoleCredentialSource,
                CredentialProfileType.AssumeRoleExternal,
                CredentialProfileType.AssumeRoleExternalMFA,
                CredentialProfileType.AssumeRoleMFA,
                CredentialProfileType.AssumeRoleWithWebIdentity,
                CredentialProfileType.AssumeRoleWithWebIdentitySessionName,
                CredentialProfileType.Basic,
                CredentialProfileType.Session,
                CredentialProfileType.CredentialProcess,
                CredentialProfileType.AssumeRoleSessionName,
                CredentialProfileType.AssumeRoleCredentialSourceSessionName,
                CredentialProfileType.AssumeRoleExternalSessionName,
                CredentialProfileType.AssumeRoleExternalMFASessionName,
                CredentialProfileType.AssumeRoleMFASessionName
            };

        private static readonly CredentialProfilePropertyMapping PropertyMapping =
            new CredentialProfilePropertyMapping(
                new Dictionary<string, string>()
                {
                    { "AccessKey", "aws_access_key_id" },
                    { "CredentialSource", "credential_source" },
#if !NETSTANDARD13
                    { "EndpointName", null },
#endif
                    { "ExternalID", "external_id" },
                    { "MfaSerial", "mfa_serial" },
                    { "RoleArn", "role_arn" },
                    { "RoleSessionName", "role_session_name" },
                    { "SecretKey", "aws_secret_access_key" },
                    { "SourceProfile", "source_profile" },
                    { "Token", "aws_session_token" },
#if !NETSTANDARD13
                    { "UserIdentity", null },
#endif
                    { "CredentialProcess" , "credential_process" },
                    { "WebIdentityTokenFile", "web_identity_token_file" }
                }
            );

        public static readonly string DefaultDirectory;
        public static readonly string DefaultFilePath;

        static SharedCredentialsFile()
        {
            var baseDirectory = Environment.GetEnvironmentVariable("HOME");

            if (string.IsNullOrEmpty(baseDirectory))
                baseDirectory = Environment.GetEnvironmentVariable("USERPROFILE");

            if (string.IsNullOrEmpty(baseDirectory))
#if NETSTANDARD
                baseDirectory = Directory.GetCurrentDirectory();
#else
                baseDirectory = Environment.CurrentDirectory;
#endif
            DefaultDirectory = Path.Combine(baseDirectory, DefaultDirectoryName);
            DefaultFilePath = Path.Combine(DefaultDirectory, DefaultFileName);
        }

        private ProfileIniFile _credentialsFile;
        private ProfileIniFile _configFile;

        public string FilePath { get; private set; }

        /// <summary>
        /// Construct a new SharedCredentialsFile in the default location.
        /// </summary>
        public SharedCredentialsFile()
        {
            SetUpFilePath(null);
            Refresh();
        }

        /// <summary>
        /// Construct a new SharedCredentialsFile.
        /// </summary>
        /// <param name="filePath">The path of the shared credentials file.</param>
        public SharedCredentialsFile(string filePath)
        {
            SetUpFilePath(filePath);
            Refresh();
        }

        private void SetUpFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                if (string.IsNullOrEmpty(AWSConfigs.AWSProfilesLocation))
                    FilePath = DefaultFilePath;
                else
                    FilePath = AWSConfigs.AWSProfilesLocation;
            }
            else
            {
                FilePath = filePath;
            }
        }

        public List<string> ListProfileNames()
        {
            Refresh();
            return ListProfiles().Select(p => p.Name).ToList();
        }

        public List<CredentialProfile> ListProfiles()
        {
            Refresh();
            var profiles = new List<CredentialProfile>();
            foreach (var profileName in ListAllProfileNames())
            {
                CredentialProfile profile = null;
                if (TryGetProfile(profileName, false, out profile) && profile.CanCreateAWSCredentials)
                {
                    profiles.Add(profile);
                }
            }
            return profiles;
        }

        public bool TryGetProfile(string profileName, out CredentialProfile profile)
        {
            return TryGetProfile(profileName, true, out profile);
        }

        /// <summary>
        /// Add the profile given. If the profile already exists, update it.
        /// </summary>
        /// <param name="profile">The profile to be written.</param>
        public void RegisterProfile(CredentialProfile profile)
        {
            Refresh();
            if (profile.CanCreateAWSCredentials || profile.Options.IsEmpty)
            {
                if (!IsSupportedProfileType(profile.ProfileType))
                {
                    throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                        "Unable to update profile {0}. The CredentialProfile object provided represents a " +
                        "{1} profile but {2} does not support the {1} profile type.",
                        profile.Name, profile.ProfileType, GetType().Name));
                }

                RegisterProfileInternal(profile);
            }
            else
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture,
                    "Unable to update profile {0}.  The CredentialProfile provided is not a valid profile.", profile.Name));
            }
        }

        /// <summary>
        /// Update the profile on disk regardless of the profile type.
        /// </summary>
        /// <param name="profile"></param>
        [SuppressMessage("Microsoft.Globalization", "CA1308", Justification = "Value is not surfaced to user. Booleans have been lowercased by SDK precedent.")]
        private void RegisterProfileInternal(CredentialProfile profile)
        {
            var reservedProperties = new Dictionary<string, string>();

            if (profile.UniqueKey != null)
                reservedProperties[ToolkitArtifactGuidField] = profile.UniqueKey.Value.ToString("D");

            if (profile.Region != null)
                reservedProperties[RegionField] = profile.Region.SystemName;

            if (profile.EndpointDiscoveryEnabled != null)
                reservedProperties[EndpointDiscoveryEnabledField] = profile.EndpointDiscoveryEnabled.Value.ToString().ToLowerInvariant();

            if (profile.StsRegionalEndpoints != null)
                reservedProperties[StsRegionalEndpointsField] = profile.StsRegionalEndpoints.ToString().ToLowerInvariant();

            if (profile.S3UseArnRegion != null)
                reservedProperties[S3UseArnRegionField] = profile.S3UseArnRegion.Value.ToString().ToLowerInvariant();
                
            if (profile.S3RegionalEndpoint != null)
                reservedProperties[S3RegionalEndpointField] = profile.S3RegionalEndpoint.ToString().ToLowerInvariant();

            if (profile.RetryMode != null)
                reservedProperties[RetryModeField] = profile.RetryMode.ToString().ToLowerInvariant();

            if (profile.MaxAttempts != null)
                reservedProperties[MaxAttemptsField] = profile.MaxAttempts.ToString().ToLowerInvariant();

            var profileDictionary = PropertyMapping.CombineProfileParts(
                profile.Options, ReservedPropertyNames, reservedProperties, profile.Properties);

            _credentialsFile.EditSection(profile.Name, new SortedDictionary<string, string>(profileDictionary));
            _credentialsFile.Persist();
            profile.CredentialProfileStore = this;
        }

        /// <summary>
        /// Deletes the section with the given ProfileName from the SharedCredentialsFile, if one exists.
        /// </summary>
        /// <param name="profileName">The ProfileName of the section to delete.</param>
        public void UnregisterProfile(string profileName)
        {
            Refresh();
            _credentialsFile.DeleteSection(profileName);
            _credentialsFile.Persist();
        }

        /// <summary>
        /// Rename the profile with oldProfileName to newProfileName.
        /// </summary>
        /// <param name="oldProfileName">The profile to rename.</param>
        /// <param name="newProfileName">The new name for the profile.</param>
        public void RenameProfile(string oldProfileName, string newProfileName)
        {
            RenameProfile(oldProfileName, newProfileName, false);
        }

        /// <summary>
        /// Rename the profile with oldProfileName to newProfileName.
        /// </summary>
        /// <param name="oldProfileName">The profile to rename.</param>
        /// <param name="newProfileName">The new name for the profile.</param>
        /// <param name="force">If true and the destination profile exists it will be overwritten.</param>
        public void RenameProfile(string oldProfileName, string newProfileName, bool force)
        {
            Refresh();
            _credentialsFile.RenameSection(oldProfileName, newProfileName, force);
            _credentialsFile.Persist();
        }

        /// <summary>
        /// Make a copy of the profile with fromProfileName called toProfileName.
        /// </summary>
        /// <param name="fromProfileName">The name of the profile to copy from.</param>
        /// <param name="toProfileName">The name of the new profile.</param>
        public void CopyProfile(string fromProfileName, string toProfileName)
        {
            CopyProfile(fromProfileName, toProfileName, false);
        }

        /// <summary>
        /// Make a copy of the profile with fromProfileName called toProfileName.
        /// </summary>
        /// <param name="fromProfileName">The name of the profile to copy from.</param>
        /// <param name="toProfileName">The name of the new profile.</param>
        /// <param name="force">If true and the destination profile exists it will be overwritten.</param>
        public void CopyProfile(string fromProfileName, string toProfileName, bool force)
        {
            Refresh();
            // Do the copy but make sure to replace the toolkitArtifactGuid with a new one, if it's there.
            _credentialsFile.CopySection(fromProfileName, toProfileName,
                new Dictionary<string, string> { {ToolkitArtifactGuidField, Guid.NewGuid().ToString()} }, force);
            _credentialsFile.Persist();
        }

        private void Refresh()
        {
            _credentialsFile = new ProfileIniFile(FilePath,false);

            // If a config file exists in the same location as the credentials file
            // load it for use as a read-only source of profile properties.
            var configPath = Path.Combine(Path.GetDirectoryName(FilePath), ConfigFileName);
            if (File.Exists(configPath))
            {
                _configFile = new ProfileIniFile(configPath,true);
            }
        }

        private HashSet<string> ListAllProfileNames()
        {
            var profileNames = _credentialsFile.ListSectionNames();
            
            if (_configFile != null)
            {
                profileNames.UnionWith(_configFile.ListSectionNames());
            }
            return profileNames;
        }

        private bool TryGetProfile(string profileName, bool doRefresh, out CredentialProfile profile)
        {
            if (doRefresh)
            {
                Refresh();
            }

            Dictionary<string, string> profileDictionary = null;
            if (TryGetSection(profileName, out profileDictionary))
            {
                CredentialProfileOptions profileOptions;
                Dictionary<string, string> reservedProperties;
                Dictionary<string, string> userProperties;
                PropertyMapping.ExtractProfileParts(profileDictionary, ReservedPropertyNames,
                    out profileOptions, out reservedProperties, out userProperties);

                string toolkitArtifactGuidStr;
                Guid? toolkitArtifactGuid = null;
                if (reservedProperties.TryGetValue(ToolkitArtifactGuidField, out toolkitArtifactGuidStr))
                {
                    if (!GuidUtils.TryParseNullableGuid(toolkitArtifactGuidStr, out toolkitArtifactGuid))
                    {
                        Logger.GetLogger(GetType()).InfoFormat("Invalid value {0} for {1} in profile {2}. GUID expected.", toolkitArtifactGuidStr, ToolkitArtifactGuidField, profileName);
                        profile = null;
                        return false;
                    }
                }

                string regionString;
                RegionEndpoint region = null;
                if (reservedProperties.TryGetValue(RegionField, out regionString))
                {
                    region = RegionEndpoint.GetBySystemName(regionString);
                }

                string endpointDiscoveryEnabledString;
                bool? endpointDiscoveryEnabled = null;
                if (reservedProperties.TryGetValue(EndpointDiscoveryEnabledField, out endpointDiscoveryEnabledString))
                {
                    bool endpointDiscoveryEnabledOut;
                    if(!bool.TryParse(endpointDiscoveryEnabledString, out endpointDiscoveryEnabledOut))
                    {
                        Logger.GetLogger(GetType()).InfoFormat("Invalid value {0} for {1} in profile {2}. A boolean true/false is expected.", endpointDiscoveryEnabledString, EndpointDiscoveryEnabledField, profileName);
                        profile = null;
                        return false;
                    }

                    endpointDiscoveryEnabled = endpointDiscoveryEnabledOut;
                }

                StsRegionalEndpointsValue? stsRegionalEndpoints = null;
                if (reservedProperties.TryGetValue(StsRegionalEndpointsField, out var stsRegionalEndpointsString))
                {
#if BCL35
                    try
                    {
                        stsRegionalEndpoints = (StsRegionalEndpointsValue) Enum.Parse(typeof(StsRegionalEndpointsValue), stsRegionalEndpointsString, true);
                    }
                    catch (Exception)
                    {
                        _logger.InfoFormat("Invalid value {0} for {1} in profile {2}. A string regional/legacy is expected.", stsRegionalEndpointsString, StsRegionalEndpointsField, profileName);
                        profile = null;
                        return false;
                    }
#else 
                    if (!Enum.TryParse<StsRegionalEndpointsValue>(stsRegionalEndpointsString, true, out var stsRegionalEndpointsTemp))
                    {
                        _logger.InfoFormat("Invalid value {0} for {1} in profile {2}. A string regional/legacy is expected.", stsRegionalEndpointsString, StsRegionalEndpointsField, profileName);
                        profile = null;
                        return false;
                    }
                    stsRegionalEndpoints = stsRegionalEndpointsTemp;
#endif
                }

                string s3UseArnRegionString;
                bool? s3UseArnRegion = null;
                if (reservedProperties.TryGetValue(S3UseArnRegionField, out s3UseArnRegionString))
                {
                    bool s3UseArnRegionOut;
                    if (!bool.TryParse(s3UseArnRegionString, out s3UseArnRegionOut))
                    {
                        profile = null;
                        return false;
                    }
                    s3UseArnRegion = s3UseArnRegionOut;
                }

                S3UsEast1RegionalEndpointValue? s3RegionalEndpoint = null;
                if (reservedProperties.TryGetValue(S3RegionalEndpointField, out var s3RegionalEndpointString))
                {
#if BCL35
                    try
                    {
                        s3RegionalEndpoint = (S3UsEast1RegionalEndpointValue) Enum.Parse(typeof(S3UsEast1RegionalEndpointValue), s3RegionalEndpointString, true);
                    }
                    catch (Exception)
                    {
                        _logger.InfoFormat("Invalid value {0} for {1} in profile {2}. A string regional/legacy is expected.", s3RegionalEndpointString, S3RegionalEndpointField, profileName);
                        profile = null;
                        return false;
                    }
#else 
                    if (!Enum.TryParse<S3UsEast1RegionalEndpointValue>(s3RegionalEndpointString, true, out var s3RegionalEndpointTemp))
                    {
                        _logger.InfoFormat("Invalid value {0} for {1} in profile {2}. A string regional/legacy is expected.", s3RegionalEndpointString, S3RegionalEndpointField, profileName);
                        profile = null;
                        return false;
                    }
                    s3RegionalEndpoint = s3RegionalEndpointTemp;
#endif
                }

                RequestRetryMode? requestRetryMode = null;
                if (reservedProperties.TryGetValue(RetryModeField, out var retryModeString))
                {
#if BCL35
                    try
                    {
                        requestRetryMode = (RequestRetryMode) Enum.Parse(typeof(RequestRetryMode), retryModeString, true);
                    }
                    catch (Exception)
                    {
                        _logger.InfoFormat("Invalid value {0} for {1} in profile {2}. A string legacy/standard/adaptive is expected.", retryModeString, RetryModeField, profileName);
                        profile = null;
                        return false;
                    }
#else 
                    if (!Enum.TryParse<RequestRetryMode>(retryModeString, true, out var retryModeTemp))
                    {
                        _logger.InfoFormat("Invalid value {0} for {1} in profile {2}. A string legacy/standard/adaptive is expected.", retryModeString, RetryModeField, profileName);
                        profile = null;
                        return false;
                    }
                    requestRetryMode = retryModeTemp;
#endif
                }
                                
                int? maxAttempts = null;
                if (reservedProperties.TryGetValue(MaxAttemptsField, out var maxAttemptsString))
                {                    
                    if (!int.TryParse(maxAttemptsString, out var maxAttemptsTemp) || maxAttemptsTemp <= 0)
                    {
                        Logger.GetLogger(GetType()).InfoFormat("Invalid value {0} for {1} in profile {2}. A positive integer is expected.", maxAttemptsString, MaxAttemptsField, profileName);
                        profile = null;
                        return false;
                    }

                    maxAttempts = maxAttemptsTemp;
                }

                profile = new CredentialProfile(profileName, profileOptions)
                {
                    UniqueKey = toolkitArtifactGuid,
                    Properties = userProperties,
                    Region = region,
                    CredentialProfileStore = this,
                    EndpointDiscoveryEnabled = endpointDiscoveryEnabled,
                    StsRegionalEndpoints = stsRegionalEndpoints,
                    S3UseArnRegion = s3UseArnRegion,
                    S3RegionalEndpoint = s3RegionalEndpoint,
                    RetryMode = requestRetryMode,
                    MaxAttempts = maxAttempts
                };

                if (!IsSupportedProfileType(profile.ProfileType))
                {
                    _logger.InfoFormat("The profile type {0} is not supported by SharedCredentialsFile.", profile.ProfileType);
                    profile = null;
                    return false;
                }

                return true;

            }

            profile = null;
            return false;
        }

        /// <summary>
        /// Try to get a profile that may be partially in the credentials file and partially in the config file.
        /// If there are identically named properties in both files, the properties in the credentials file take precedence.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="iniProperties"></param>
        /// <returns></returns>
        private bool TryGetSection(string sectionName, out Dictionary<string, string> iniProperties)
        {
            Dictionary<string, string> credentialsProperties = null;
            Dictionary<string, string> configProperties = null;
            var hasCredentialsProperties = _credentialsFile.TryGetSection(sectionName, out credentialsProperties);
           
            var hasConfigProperties = false;
            if (_configFile != null)
            {
                _configFile.ProfileMarkerRequired = sectionName != DefaultProfileName;
                hasConfigProperties = _configFile.TryGetSection(sectionName, out configProperties);
            }

            if (hasConfigProperties)
            {
                iniProperties = configProperties;
                if (hasCredentialsProperties)
                {
                    // Add all the properties from the credentials file.
                    // If a property exits in both, the one from the credentials
                    // file takes precedence and overwrites the one from
                    // the config file.
                    foreach (var pair in credentialsProperties)
                    {
                        iniProperties[pair.Key] = pair.Value;
                    }
                }
                return true;
            }

            iniProperties = credentialsProperties;
            return hasCredentialsProperties;
        }

        private static bool IsSupportedProfileType(CredentialProfileType? profileType)
        {
            return !profileType.HasValue || ProfileTypeWhitelist.Contains(profileType.Value);
        }
    }
}