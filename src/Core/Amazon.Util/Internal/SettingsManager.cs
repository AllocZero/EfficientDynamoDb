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
using Amazon.Runtime;
using Amazon.Runtime.Internal.Settings;
using Amazon.Runtime.Internal.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Amazon.Util.Internal
{
    /// <summary>
    /// Manager to access a settings store file.
    /// Settings store files are located under the current user's AppData\Local\AWSToolkit folder.
    /// Select keys in these files are encrypted on a per user, per machine basis using the Windows Data Protection API;
    /// the encrypted values cannot be used by any other user or on any other machine.
    ///
    /// This class is not threadsafe.
    /// </summary>
    public class SettingsManager
    {
        /// <summary>
        /// True if the encrypted store is availble, false otherwise.
        /// </summary>
        public static bool IsAvailable
        {
            get { return UserCrypto.IsUserCryptAvailable; }
        }

        /// <summary>
        /// Construct a SettingsManager.
        /// </summary>
        /// <param name="settingsType">The base filename to read/write.</param>
        public SettingsManager(string settingsType)
        {
            EnsureAvailable();
            SettingsType = settingsType;
        }

        /// <summary>
        /// The base filename to read/write.
        /// </summary>
        public string SettingsType { get; private set; }

        /// <summary>
        /// Register an object.  Let the uniqueKey be assigned automatically.
        /// </summary>
        /// <param name="properties">The property names and values for the object.</param>
        /// <returns>The unique key that was assigned to the object.</returns>
        public string RegisterObject(Dictionary<string, string> properties)
        {
            return RegisterObject(null, properties);
        }

        /// <summary>
        /// Register an object.
        /// </summary>
        /// <param name="uniqueKey">The uniqueKey for the object, or null if one should be assigned automatically.</param>
        /// <param name="properties">The property names and values for the object.</param>
        /// <returns>The unique key that was assigned to the object.</returns>
        public string RegisterObject(string uniqueKey, Dictionary<string, string> properties)
        {
            var settings = GetSettings();
            SettingsCollection.ObjectSettings objectSettings;
            if (!TryGetObjectSettings(uniqueKey, settings, out objectSettings))
            {
                string actualUniqueKey;
                if (string.IsNullOrEmpty(uniqueKey))
                    actualUniqueKey = Guid.NewGuid().ToString("D");
                else
                    actualUniqueKey = uniqueKey;

                objectSettings = settings.NewObjectSettings(actualUniqueKey);
            }

            foreach (var pair in properties)
            {
                if (pair.Value == null)
                    objectSettings.Remove(pair.Key);
                else
                    objectSettings[pair.Key] = pair.Value;
            }

            SaveSettings(settings);
            return objectSettings.UniqueKey;
        }

        /// <summary>
        /// Try to get an object form the store.
        /// </summary>
        /// <param name="uniqueKey">The uniqueKey of the object in the store</param>
        /// <param name="properties">The property names and values for the object.</param>
        /// <returns>True if the object was found, false otherwise.</returns>
        public bool TryGetObject(string uniqueKey, out Dictionary<string, string> properties)
        {
            var settings = GetSettings();
            SettingsCollection.ObjectSettings objectSettings;
            properties = null;
            if (TryGetObjectSettings(uniqueKey, settings, out objectSettings))
            {
                uniqueKey = objectSettings.UniqueKey;
                properties = new Dictionary<string, string>();
                foreach (var key in objectSettings.Keys)
                {
                    properties.Add(key, objectSettings[key]);
                }
            }
            return properties != null;
        }

        /// <summary>
        /// Try to get an object form the store based on the value of a specific property.
        /// If multiple objects with the same property value exist, return the first one.
        /// </summary>
        /// <param name="propertyName">The name of the property to search on.</param>
        /// <param name="value">The value of the property to search on.</param>
        /// <param name="uniqueKey">The unique key of the object.</param>
        /// <param name="properties">The properties of the object.</param>
        /// <returns></returns>
        public bool TryGetObjectByProperty(string propertyName, string value, out string uniqueKey, out Dictionary<string, string> properties)
        {
            var settings = GetSettings();
            SettingsCollection.ObjectSettings objectSettings;
            properties = null;
            uniqueKey = null;
            if (TryGetObjectSettings(propertyName, value, settings, out objectSettings))
            {
                uniqueKey = objectSettings.UniqueKey;
                properties = new Dictionary<string, string>();
                foreach (var key in objectSettings.Keys)
                {
                    properties.Add(key, objectSettings[key]);
                }
            }
            return properties != null;
        }

        /// <summary>
        /// Get a list of the unique keys of all the objects in the store.
        /// </summary>
        /// <returns></returns>
        public List<string> ListUniqueKeys()
        {
            var settings = GetSettings();
            return new List<string>(settings.Select(x => x.UniqueKey));
        }

        /// <summary>
        /// Get the value of a specific property for each object in the store.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public List<string> SelectProperty(string propertyName)
        {
            var settings = GetSettings();
            return new List<string>(settings.Select(x => x[propertyName]));
        }

        /// <summary>
        /// Unregister an object from the store.
        /// </summary>
        /// <param name="uniqueKey">The unique key for the object.</param>
        public void UnregisterObject(string uniqueKey)
        {
            var settings = GetSettings();
            SettingsCollection.ObjectSettings objectSettings = null;
            if (TryGetObjectSettings(uniqueKey, settings, out objectSettings))
            {
                settings.Remove(objectSettings.UniqueKey);
                SaveSettings(settings);
            }
        }

        private SettingsCollection GetSettings()
        {
            return PersistenceManager.Instance.GetSettings(SettingsType);
        }

        private void SaveSettings(SettingsCollection settings)
        {
            PersistenceManager.Instance.SaveSettings(SettingsType, settings);
        }
        
        private static bool TryGetObjectSettings(string propertyName, string value, SettingsCollection settings, out SettingsCollection.ObjectSettings objectSettings)
        {
            objectSettings = settings.FirstOrDefault(x => string.Equals(x[propertyName], value, StringComparison.OrdinalIgnoreCase));
            return objectSettings != null;
        }

        private static bool TryGetObjectSettings(string uniqueKey, SettingsCollection settings, out SettingsCollection.ObjectSettings objectSettings)
        {
            objectSettings = settings.FirstOrDefault(x => string.Equals(x.UniqueKey, uniqueKey, StringComparison.OrdinalIgnoreCase));
            return objectSettings != null;
        }

        private static void EnsureAvailable()
        {
            if (!IsAvailable)
            {
                throw new AmazonClientException("The encrypted store is not available.  This may be due to use of a non-Windows operating system or Windows Nano Server, or the current user account may not have its profile loaded.");
            }
        }

    }
}
