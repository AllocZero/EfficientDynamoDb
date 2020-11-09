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
using Amazon.Runtime.Internal.Settings;
using Amazon.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Amazon.Runtime.CredentialManagement.Internal
{
    /// <summary>
    /// This class is used internally and should not be accessed by SDK users.
    ///
    /// Provide access to internal properties of CredentialProfile to
    /// PowerShell tools and the Visual Studio Toolkit.
    /// </summary>
    public static class CredentialProfileUtils
    {
        public static Dictionary<string, string> GetProperties(CredentialProfile profile)
        {
            return profile.Properties;
        }

        public static void SetProperty(CredentialProfile profile, string key, string value)
        {
            profile.Properties[key] = value;
        }

        public static string GetProperty(CredentialProfile profile, string key)
        {
            string value;
            profile.Properties.TryGetValue(key, out value);
            return value;
        }

        public static Guid EnsureUniqueKeyAssigned(CredentialProfile profile, ICredentialProfileStore profileStore)
        {
            if (!profile.UniqueKey.HasValue)
            {
                profile.UniqueKey = Guid.NewGuid();
                profileStore.RegisterProfile(profile);
            }
            return profile.UniqueKey.Value;
        }

        public static String GetUniqueKey(CredentialProfile profile)
        {
            return profile.UniqueKey.HasValue ? profile.UniqueKey.Value.ToString("D") : null;
        }

        public static void SetUniqueKey(CredentialProfile profile, Guid? uniqueKey)
        {
            profile.UniqueKey = uniqueKey;
        }

        public static CredentialProfileType? GetProfileType(CredentialProfile profile)
        {
            return profile.ProfileType;
        }

        public static bool IsCallbackRequired(CredentialProfile profile)
        {
            return profile.IsCallbackRequired;
        }
    }
}