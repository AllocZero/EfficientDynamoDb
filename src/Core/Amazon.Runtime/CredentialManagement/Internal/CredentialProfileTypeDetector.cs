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
using System.Collections.Generic;
using System.Reflection;

namespace Amazon.Runtime.CredentialManagement.Internal
{
    public enum CredentialProfileType
    {
        AssumeRole,
        AssumeRoleCredentialSource,
        AssumeRoleExternal,
        AssumeRoleExternalMFA,
        AssumeRoleMFA,
        Basic,
#if !NETSTANDARD13
        SAMLRole,
        SAMLRoleUserIdentity,
#endif
        Session,
        CredentialProcess,
        AssumeRoleWithWebIdentity,
        AssumeRoleWithWebIdentitySessionName,
        AssumeRoleSessionName,
        AssumeRoleCredentialSourceSessionName,
        AssumeRoleExternalSessionName,
        AssumeRoleExternalMFASessionName,
        AssumeRoleMFASessionName,
    }

    public enum CredentialSourceType
    {
        Ec2InstanceMetadata,
        Environment,
        EcsContainer
    }

    public static class CredentialProfileTypeDetector
    {
        private const string BasicCredentials = "Basic";
        private const string SessionCredentials = "Session";
        private const string AssumeRoleCredentials = "Assume Role";
        private const string AssumeRoleWithWebIdentityCredentials = "Assume Role with OIDC Web Identity";
        private const string SAMLCredentials = "SAML";

        private const string AccessKey = "AccessKey";
        private const string CredentialSource = "CredentialSource";
        private const string EndpointName = "EndpointName";
        private const string ExternalID = "ExternalID";
        private const string MfaSerial = "MfaSerial";
        private const string RoleArn = "RoleArn";
        private const string RoleSessionName = "RoleSessionName";
        private const string SecretKey = "SecretKey";
        private const string SourceProfile = "SourceProfile";        
        private const string Token = "Token";
        private const string WebIdentityTokenFile = "WebIdentityTokenFile";
        private const string UserIdentity = "UserIdentity";
        private const string CredentialProcess = "CredentialProcess";

        private static Dictionary<CredentialProfileType, HashSet<string>> TypePropertyDictionary =
            new Dictionary<CredentialProfileType, HashSet<string>>()
            {
                {
                    CredentialProfileType.CredentialProcess, new HashSet<string>()
                    {
                        CredentialProcess
                    }
                },
                {
                    CredentialProfileType.AssumeRole, new HashSet<string>()
                    {
                        RoleArn,
                        SourceProfile,
                    }
                },
                {
                    CredentialProfileType.AssumeRoleCredentialSource, new HashSet<string>()
                    {
                        RoleArn,
                        CredentialSource,
                    }
                },
                {
                    CredentialProfileType.AssumeRoleExternal, new HashSet<string>()
                    {
                        ExternalID,
                        RoleArn,
                        SourceProfile,
                    }
                },
                {
                    CredentialProfileType.AssumeRoleExternalMFA, new HashSet<string>()
                    {
                        ExternalID,
                        MfaSerial,
                        RoleArn,
                        SourceProfile,
                    }
                },
                {
                    CredentialProfileType.AssumeRoleWithWebIdentity, new HashSet<string>()
                    {
                        RoleArn,
                        WebIdentityTokenFile,
                    }
                },
                {
                    CredentialProfileType.AssumeRoleWithWebIdentitySessionName, new HashSet<string>()
                    {
                        RoleArn,
                        WebIdentityTokenFile,
                        RoleSessionName
                    }
                },
                {
                    CredentialProfileType.AssumeRoleMFA, new HashSet<string>()
                    {
                        MfaSerial,
                        RoleArn,
                        SourceProfile,
                    }
                },
                {
                    CredentialProfileType.Basic, new HashSet<string>()
                    {
                        AccessKey,
                        SecretKey,
                    }
                },
#if !NETSTANDARD13
                {
                    CredentialProfileType.SAMLRole, new HashSet<string>()
                    {
                        EndpointName,
                        RoleArn,
                    }
                },
                {
                    CredentialProfileType.SAMLRoleUserIdentity, new HashSet<string>()
                    {
                        EndpointName,
                        RoleArn,
                        UserIdentity,
                    }
                },
#endif
                {
                    CredentialProfileType.Session, new HashSet<string>()
                    {
                        AccessKey,
                        SecretKey,
                        Token,
                    }
                },
                {
                    CredentialProfileType.AssumeRoleSessionName, new HashSet<string>()
                    {
                        RoleArn,
                        SourceProfile,
                        RoleSessionName
                    }
                },
                {
                    CredentialProfileType.AssumeRoleCredentialSourceSessionName, new HashSet<string>()
                    {
                        RoleArn,
                        CredentialSource,
                        RoleSessionName
                    }
                },
                {
                    CredentialProfileType.AssumeRoleExternalSessionName, new HashSet<string>()
                    {
                        ExternalID,
                        RoleArn,
                        SourceProfile,
                        RoleSessionName
                    }
                },
                {
                    CredentialProfileType.AssumeRoleExternalMFASessionName, new HashSet<string>()
                    {
                        ExternalID,
                        MfaSerial,
                        RoleArn,
                        SourceProfile,
                        RoleSessionName
                    }
                },
                {
                    CredentialProfileType.AssumeRoleMFASessionName, new HashSet<string>()
                    {
                        MfaSerial,
                        RoleArn,
                        SourceProfile,
                        RoleSessionName
                    }
                }
            };

        private static Dictionary<CredentialProfileType, string> CredentialTypeDictionary =
            new Dictionary<CredentialProfileType, string>()
            {
                { CredentialProfileType.AssumeRole, AssumeRoleCredentials },
                { CredentialProfileType.AssumeRoleExternal, AssumeRoleCredentials },
                { CredentialProfileType.AssumeRoleExternalMFA, AssumeRoleCredentials },
                { CredentialProfileType.AssumeRoleMFA, AssumeRoleCredentials },
                { CredentialProfileType.AssumeRoleWithWebIdentity, AssumeRoleWithWebIdentityCredentials },
                { CredentialProfileType.AssumeRoleWithWebIdentitySessionName, AssumeRoleWithWebIdentityCredentials },
                { CredentialProfileType.AssumeRoleSessionName, AssumeRoleCredentials },
                { CredentialProfileType.AssumeRoleExternalSessionName, AssumeRoleCredentials },
                { CredentialProfileType.AssumeRoleExternalMFASessionName, AssumeRoleCredentials },
                { CredentialProfileType.AssumeRoleMFASessionName, AssumeRoleCredentials },
                { CredentialProfileType.Basic, BasicCredentials },
#if !NETSTANDARD13
                { CredentialProfileType.SAMLRole, SAMLCredentials },
                { CredentialProfileType.SAMLRoleUserIdentity, SAMLCredentials },
#endif
                { CredentialProfileType.Session, SessionCredentials },
                { CredentialProfileType.CredentialProcess, CredentialProcess }
            };

        public static string GetUserFriendlyCredentialType(CredentialProfileType? profileType)
        {
            if (profileType.HasValue)
            {
                return CredentialTypeDictionary[profileType.Value];
            }
            else
            {
                return null;
            }
        }

        public static CredentialProfileType? DetectProfileType(CredentialProfileOptions profileOptions)
        {
            CredentialProfileType? profileType = null;

            HashSet<string> propertyNames = GetPropertyNames(profileOptions);

            // brute force algorithm - but it's a very small set
            foreach (var pair in TypePropertyDictionary)
            {
                if (pair.Value.SetEquals(propertyNames))
                {
                    // exact match
                    profileType = pair.Key;
                }
            }

            return profileType;
        }

        public static HashSet<string> GetPropertiesForProfileType(CredentialProfileType profileType)
        {
            return new HashSet<string>(TypePropertyDictionary[profileType]);
        }

        private static HashSet<string> GetPropertyNames(CredentialProfileOptions profileOptions)
        {
            HashSet<string> propertyNames = new HashSet<string>();
            foreach (var property in typeof(CredentialProfileOptions).GetProperties())
            {
                var value = (string)property.GetValue(profileOptions, null);
                if (!string.IsNullOrEmpty(value))
                {
                    propertyNames.Add(property.Name);
                }
            }
            return propertyNames;
        }
    }
}
