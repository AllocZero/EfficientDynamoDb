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
using Amazon.Runtime.CredentialManagement.Internal;
using Amazon.Runtime.Internal.Settings;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using Amazon.Util.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace Amazon.Runtime.CredentialManagement
{
    /// <summary>
    /// This class allows profiles supporting AWSCredentials to be registered with
    /// the SDK so that they can later be reference by a profile name. The credential profiles will be available
    /// for use in the AWS Toolkit for Visual Studio and the AWS Tools for Windows PowerShell.
    /// <para>
    /// The credentials are stored under the current users AppData folder encrypted using Windows Data Protection API.
    /// </para>
    /// <para>
    /// This class is not threadsafe.
    /// </para>
    /// </summary>
    public class NetSDKCredentialsFile : ICredentialProfileStore
    {
        public const string DefaultProfileName = "Default";

        // Values kept from ProfileManager to support backward compatibility.
        private const string AWSCredentialsProfileType = "AWS";
        private const string SAMLRoleProfileType = "SAML";

        private const string RegionField = "Region";

        private const string EndpointDiscoveryEnabledField = "EndpointDiscoveryEnabled";
        private const string S3UseArnRegionField = "S3UseArnRegion";

        private const string StsRegionalEndpointsField = "StsRegionalEndpoints";

        private const string S3RegionalEndpointField = "S3RegionalEndpoint";

        private const string RetryModeField = "RetryMode";
        private const string MaxAttemptsField = "MaxAttempts";

        private static readonly HashSet<string> ReservedPropertyNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            SettingsConstants.DisplayNameField,
            SettingsConstants.ProfileTypeField,
            RegionField,
            EndpointDiscoveryEnabledField,
            S3UseArnRegionField,
            StsRegionalEndpointsField,
            S3RegionalEndpointField,
            RetryModeField,
            MaxAttemptsField
        };

        private static readonly CredentialProfilePropertyMapping PropertyMapping =
            new CredentialProfilePropertyMapping(
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "AccessKey", SettingsConstants.AccessKeyField },
                    { "CredentialSource", SettingsConstants.CredentialSourceField },
#if !NETSTANDARD13
                    { "EndpointName", SettingsConstants.EndpointNameField },
#endif
                    { "ExternalID", SettingsConstants.ExternalIDField},
                    { "MfaSerial", SettingsConstants.MfaSerialField},
                    { "RoleArn", SettingsConstants.RoleArnField },
                    { "RoleSessionName", SettingsConstants.RoleSessionName},
                    { "SecretKey", SettingsConstants.SecretKeyField },
                    { "SourceProfile", SettingsConstants.SourceProfileField },
                    { "Token", SettingsConstants.SessionTokenField },
#if !NETSTANDARD13
                    { "UserIdentity", SettingsConstants.UserIdentityField },
#endif
                    // Not implemented for NetSDKCredentials. Applicable only
                    // for SharedCredentials
                    { "CredentialProcess" , SettingsConstants.CredentialProcess },
                    { "WebIdentityTokenFile", SettingsConstants.WebIdentityTokenFile }
                }
            );

        private readonly NamedSettingsManager _settingsManager;

        public NetSDKCredentialsFile()
        {
            _settingsManager = new NamedSettingsManager(SettingsConstants.RegisteredProfiles);
        }

        public List<string> ListProfileNames()
        {
            return ListProfiles().Select(p => p.Name).ToList();
        }

        public List<CredentialProfile> ListProfiles()
        {
            var profiles = new List<CredentialProfile>();
            foreach (var profileName in _settingsManager.ListObjectNames())
            {
                CredentialProfile profile = null;
                if (TryGetProfile(profileName, out profile) && profile.CanCreateAWSCredentials)
                {
                    profiles.Add(profile);
                }
            }
            return profiles;
        }

        /// <summary>
        /// Get the profile with the name given, if it exists in this store.
        /// </summary>
        /// <param name="profileName">The name of the profile to find.</param>
        /// <param name="profile">The profile, if it was found, null otherwise</param>
        /// <returns>True if the profile was found, false otherwise.</returns>
        public bool TryGetProfile(string profileName, out CredentialProfile profile)
        {
            Dictionary<string, string> properties;
            string uniqueKeyStr;
            if (_settingsManager.TryGetObject(profileName, out uniqueKeyStr, out properties))
            {
                try
                {
                    CredentialProfileOptions profileOptions;
                    Dictionary<string, string> userProperties;
                    Dictionary<string, string> reservedProperties;
                    PropertyMapping.ExtractProfileParts(properties, ReservedPropertyNames,out profileOptions, out reservedProperties, out userProperties);

                    string regionString;
                    RegionEndpoint region = null;
                    if (reservedProperties.TryGetValue(RegionField, out regionString))
                    {
                        region = RegionEndpoint.GetBySystemName(regionString);
                    }

                    Guid? uniqueKey = null;
                    if (!GuidUtils.TryParseNullableGuid(uniqueKeyStr, out uniqueKey))
                    {
                        profile = null;
                        return false;
                    }

                    string endpointDiscoveryEnabledString;
                    bool? endpointDiscoveryEnabled = null;
                    if (reservedProperties.TryGetValue(EndpointDiscoveryEnabledField, out endpointDiscoveryEnabledString))
                    {
                        bool endpointDiscoveryEnabledOut;
                        if (!bool.TryParse(endpointDiscoveryEnabledString, out endpointDiscoveryEnabledOut))
                        {                            
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
                            stsRegionalEndpoints = (StsRegionalEndpointsValue)Enum.Parse(typeof(StsRegionalEndpointsValue), stsRegionalEndpointsString, true);
                        }
                        catch (Exception)
                        {
                            profile = null;
                            return false;
                        }
#else
                        if (!Enum.TryParse<StsRegionalEndpointsValue>(stsRegionalEndpointsString, true, out var tempStsRegionalEndpoints))
                        {
                            profile = null;
                            return false;
                        }
                        stsRegionalEndpoints = tempStsRegionalEndpoints;
#endif
                    }

                    string s3UseArnRegionString;
                    bool? s3UseArnRegion = null;
                    if(reservedProperties.TryGetValue(S3UseArnRegionField, out s3UseArnRegionString))
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
                            s3RegionalEndpoint = (S3UsEast1RegionalEndpointValue)Enum.Parse(typeof(S3UsEast1RegionalEndpointValue), s3RegionalEndpointString, true);
                        }
                        catch (Exception)
                        {
                            profile = null;
                            return false;
                        }
#else
                        if (!Enum.TryParse<S3UsEast1RegionalEndpointValue>(s3RegionalEndpointString, true, out var tempS3RegionalEndpoint))
                        {
                            profile = null;
                            return false;
                        }
                        s3RegionalEndpoint = tempS3RegionalEndpoint;
#endif
                    }

                    RequestRetryMode? requestRetryMode = null;
                    if (reservedProperties.TryGetValue(RetryModeField, out var retryModeString))
                    {
#if BCL35
                        try
                        {
                            requestRetryMode = (RequestRetryMode)Enum.Parse(typeof(RequestRetryMode), retryModeString, true);
                        }
                        catch (Exception)
                        {
                            profile = null;
                            return false;
                        }
#else
                        if (!Enum.TryParse<RequestRetryMode>(retryModeString, true, out var tempRetryMode))
                        {
                            profile = null;
                            return false;
                        }
                        requestRetryMode = tempRetryMode;
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
                        UniqueKey = uniqueKey,
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
                    return true;
                }
                catch (ArgumentException)
                {
                    profile = null;
                    return false;
                }
            }
            else
            {
                profile = null;
                return false;
            }
        }

        /// <summary>
        /// Add the profile to this store, if it's valid.
        /// </summary>
        /// <param name="profile">The profile to add.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1308", Justification = "Value is not surfaced to user. Booleans have been lowercased by SDK precedent.")]
        public void RegisterProfile(CredentialProfile profile)
        {
            if (profile.CanCreateAWSCredentials || profile.Options.IsEmpty)
            {
                var reservedProperties = new Dictionary<string, string>();
                if (profile.CanCreateAWSCredentials)
                {
                    // set profile type field for backward compatibility
                    SetProfileTypeField(reservedProperties, profile.ProfileType.Value);
                }

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

                // Set the UniqueKey.  It might change if the unique key is set by the objectManger,
                // or if this is an update to an existing profile.
                string newUniqueKeyStr = _settingsManager.RegisterObject(profile.Name, profileDictionary);
                Guid? newUniqueKey;
                if (GuidUtils.TryParseNullableGuid(newUniqueKeyStr, out newUniqueKey))
                    profile.UniqueKey = newUniqueKey;
                profile.CredentialProfileStore = this;
            }
            else
            {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture,
                    "Unable to register profile {0}.  The CredentialProfileOptions provided is not valid.", profile.Name));
            }
        }

        /// <summary>
        /// If there is a profile in the store with the given name, delete it.
        /// </summary>
        /// <param name="profileName">The name of the profile to delete.</param>
        public void UnregisterProfile(string profileName)
        {
            _settingsManager.UnregisterObject(profileName);
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
            _settingsManager.RenameObject(oldProfileName, newProfileName, force);
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
            _settingsManager.CopyObject(fromProfileName, toProfileName, force);
        }

        /// <summary>
        /// Set the ProfileType field to maintain backward compatibility with ProfileManager.
        /// The value is ignored when it's read back in.
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="profileType"></param>
        private static void SetProfileTypeField(IDictionary<string, string> properties, CredentialProfileType profileType)
        {
            switch (profileType)
            {
                case CredentialProfileType.Basic:
                    properties[SettingsConstants.ProfileTypeField] = AWSCredentialsProfileType;
                    break;
#if !NETSTANDARD13
                case CredentialProfileType.SAMLRole:
                case CredentialProfileType.SAMLRoleUserIdentity:
                    properties[SettingsConstants.ProfileTypeField] = SAMLRoleProfileType;
                    break;
#endif
                default:
                    properties[SettingsConstants.ProfileTypeField] = profileType.ToString();
                    break;
            }
        }
    }
}