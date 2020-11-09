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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Amazon.Runtime.CredentialManagement.Internal
{
    /// <summary>
    /// Class to easily convert from Dictionary&lt;string, string&gt; to ProfileOptions and back.
    /// </summary>
    public class CredentialProfilePropertyMapping
    {
        private static readonly HashSet<string> TypePropertySet =
            new HashSet<string>(typeof(CredentialProfileOptions).GetProperties().Select((p) => p.Name), StringComparer.OrdinalIgnoreCase);

        private static readonly PropertyInfo[] CredentialProfileReflectionProperties = typeof(CredentialProfileOptions).GetProperties();

        private readonly Dictionary<string, string> _nameMapping;
        private readonly HashSet<string> _mappedNames;

        public CredentialProfilePropertyMapping(Dictionary<string, string> nameMapping)
        {
            if (!TypePropertySet.SetEquals(new HashSet<string>(nameMapping.Keys, StringComparer.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("The nameMapping Dictionary must contain a name mapping for each ProfileOptions property, and no additional keys.");
            }

            this._nameMapping = nameMapping;
            _mappedNames = new HashSet<string>(nameMapping.Values.Where(v => !string.IsNullOrEmpty(v)), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Separate the profileDictionary into its parts.
        /// profileDictionary = profileOptions + reservedProperties + userProperties
        /// </summary>
        /// <param name="profileDictionary">Dictionary with everything in it</param>
        /// <param name="reservedKeys">Keys to ignore</param>
        /// <param name="profileOptions">The resulting CredentialProfileOptions</param>
        /// <param name="userProperties">The properties that are left</param>
        public void ExtractProfileParts(Dictionary<string, string> profileDictionary, HashSet<string> reservedKeys,
            out CredentialProfileOptions profileOptions, out Dictionary<string, string> userProperties)
        {
            Dictionary<string, string> reservedProperties;
            ExtractProfileParts(profileDictionary, reservedKeys, out profileOptions,
                out reservedProperties, out userProperties);
        }

        /// <summary>
        /// Separate the profileDictionary into its parts.
        /// profileDictionary = profileOptions + reservedProperties + userProperties
        /// </summary>
        /// <param name="profileDictionary">Dictionary with everything in it</param>
        /// <param name="reservedKeys">Keys for the reservedKeys dictionary</param>
        /// <param name="profileOptions">The resulting CredentialProfileOptions</param>
        /// <param name="reservedProperties">The resulting reserved properties</param>
        /// <param name="userProperties">The properties that are left</param>
        public void ExtractProfileParts(Dictionary<string, string> profileDictionary, HashSet<string> reservedKeys,
            out CredentialProfileOptions profileOptions, out Dictionary<string, string> reservedProperties,
            out Dictionary<string, string> userProperties)
        {
            // profileDictionary = profileOptions + reservedProperties + userProperties
            // algorithm: userProperties = profileDictionary - profileOptions - reservedProperties

            // userProperties = profileDictionary
            userProperties = new Dictionary<string, string>(profileDictionary);

            // userProperties -= profileOptions
            profileOptions = new CredentialProfileOptions();
            foreach (var reflectionProperty in CredentialProfileReflectionProperties)
            {
                string value = null;
                var mappedName = _nameMapping[reflectionProperty.Name];
                if (mappedName != null)
                {
                    if (userProperties.TryGetValue(mappedName, out value))
                    {
                        reflectionProperty.SetValue(profileOptions, value, null);
                        userProperties.Remove(mappedName);
                    }
                }
            }

            // userProperties -= reservedProperties
            if (reservedKeys == null)
            {
                reservedProperties = null;
            }
            else
            {
                reservedProperties = new Dictionary<string, string>();
                foreach (var key in reservedKeys)
                {
                    string value = null;
                    if (userProperties.TryGetValue(key, out value))
                    {
                        reservedProperties.Add(key, value);
                        userProperties.Remove(key);
                    }
                }
            }
        }

        /// <summary>
        /// Validate the userProperties and then combine profileOptions, reservedProperties, and userProperties into one Dictionary.
        /// </summary>
        /// <param name="profileOptions"></param>
        /// <param name="reservedPropertyNames"></param>
        /// <param name="reservedProperties"></param>
        /// <param name="userProperties"></param>
        /// <returns></returns>
        public Dictionary<string, string> CombineProfileParts(CredentialProfileOptions profileOptions,
            HashSet<string> reservedPropertyNames, Dictionary<string, string> reservedProperties, Dictionary<string, string> userProperties)
        {
            ValidateNoProfileOptionsProperties(userProperties);
            ValidateNoReservedProperties(reservedPropertyNames, userProperties);

            var profileDictionary = new Dictionary<string, string>();
            foreach (var pair in Convert(profileOptions).Concat(reservedProperties).Concat(userProperties))
            {
                profileDictionary.Add(pair.Key, pair.Value);
            }
            return profileDictionary;

        }

        /// <summary>
        /// Make sure the userProperties dictionary doesn't contain any keys that are reserved.
        /// Check is case-insensitive for added safety.
        /// </summary>
        /// <param name="reservedPropertyNames"></param>
        /// <param name="userProperties"></param>
        private static void ValidateNoReservedProperties(HashSet<string> reservedPropertyNames, Dictionary<string, string> userProperties)
        {
            if (userProperties != null)
            {
                List<string> reservedKeys = new List<string>();
                foreach (var key in reservedPropertyNames)
                {
                    if (userProperties.Keys.Contains(key, StringComparer.OrdinalIgnoreCase))
                    {
                        reservedKeys.Add(key);
                    }
                }

                if (reservedKeys.Count > 0)
                    throw new ArgumentException("The profile properties cannot contain reserved names as keys: " +
                        string.Join(" or ", reservedKeys.ToArray()));
            }
        }

        /// <summary>
        /// Make sure the userProperties dictionary doesn't contain any keys that
        /// overlap with the names of mapped names for CredentialProfileOptions property names.
        /// Check is case-insensitive for added safety.
        /// </summary>
        /// <param name="userProperties"></param>
        private void ValidateNoProfileOptionsProperties(Dictionary<string, string> userProperties)
        {
            if (userProperties != null)
            {
                foreach (var key in userProperties.Keys)
                {
                    if (_mappedNames.Contains(key, StringComparer.OrdinalIgnoreCase))
                    {
                        throw new ArgumentException("The profile properties dictionary cannot contain a key named " + key +
                            " because it is in the name mapping dictionary.");
                    }
                }
            }
        }

        private Dictionary<string, string> Convert(CredentialProfileOptions profileOptions)
        {
            var dictionary = new Dictionary<string, string>();

            // if profileOptions.IsEmpty then leave all the credentials keys untouched
            if (!profileOptions.IsEmpty)
            {
                var properties = typeof(CredentialProfileOptions).GetProperties();

                // ensure repeatable order
                Array.Sort(properties.Select((p) => p.Name).ToArray(), properties);

                foreach (var property in properties)
                {
                    var value = (string)property.GetValue(profileOptions, null);
                    if (string.IsNullOrEmpty(value))
                        value = null;

                    if (_nameMapping[property.Name] != null)
                    {
                        dictionary.Add(_nameMapping[property.Name], value);
                    }
                }
            }
            return dictionary;
        }


    }
}
