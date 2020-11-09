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
    /// Manager to access a settings store file.  Objects are primarily accessed by DisplayName rather than unique key.
    /// Settings store files are located under the current user's AppData\Local\AWSToolkit folder.
    /// Select keys in these files are encrypted on a per user, per machine basis using the Windows Data Protection API;
    /// the encrypted values cannot be used by any other user or on any other machine.
    ///
    /// This class is not threadsafe.
    /// </summary>
    public class NamedSettingsManager
    {
        /// <summary>
        /// True if the encrypted store is availble, false otherwise.
        /// </summary>
        public static bool IsAvailable
        {
            get { return UserCrypto.IsUserCryptAvailable; }
        }

        private SettingsManager settingsManager;

        /// <summary>
        /// Construct an NamedSettingsManager.
        /// </summary>
        /// <param name="settingsType">The base filename to read/write.</param>
        public NamedSettingsManager(string settingsType)
        {
            settingsManager = new SettingsManager(settingsType);
        }

        /// <summary>
        /// Register an object.  Let the uniqueKey be assigned automatically.
        /// </summary>
        /// <param name="displayName">The display name for the object.</param>
        /// <param name="properties">The property names and values for the object.</param>
        /// <returns>The unique key that was assigned to the object.</returns>
        public string RegisterObject(string displayName, Dictionary<string, string> properties)
        {
            if (string.IsNullOrEmpty(displayName))
                throw new ArgumentException("displayName cannot be null or empty.");

            var propertiesPlusName = new Dictionary<string, string>(properties);
            propertiesPlusName[SettingsConstants.DisplayNameField] = displayName;

            string uniqueKey;
            if (settingsManager.TryGetObjectByProperty(SettingsConstants.DisplayNameField, displayName, out uniqueKey, out properties))
                return settingsManager.RegisterObject(uniqueKey, propertiesPlusName);
            else
                return settingsManager.RegisterObject(propertiesPlusName);
        }

        /// <summary>
        /// Try to get an object form the store.
        /// </summary>
        /// <param name="displayName">The display name for the object.</param>
        /// <param name="properties">The property names and values for the object.</param>
        /// <returns>True if the object was found, false otherwise.</returns>
        public bool TryGetObject(string displayName, out Dictionary<string, string> properties)
        {
            string uniqueKey;
            return TryGetObject(displayName, out uniqueKey, out properties);
        }

        /// <summary>
        /// Try to get an object form the store.
        /// </summary>
        /// <param name="displayName">The display name for the object.</param>
        /// <param name="uniqueKey">The uniqueKey of the object in the store</param>
        /// <param name="properties">The property names and values for the object.</param>
        /// <returns>True if the object was found, false otherwise.</returns>
        public bool TryGetObject(string displayName, out string uniqueKey, out Dictionary<string, string> properties)
        {
            return settingsManager.TryGetObjectByProperty(SettingsConstants.DisplayNameField, displayName, out uniqueKey, out properties);
        }

        /// <summary>
        /// Unregister an object from the store.
        /// </summary>
        /// <param name="displayName">The display name for the object.</param>
        public void UnregisterObject(string displayName)
        {
            string uniqueKey;
            Dictionary<string, string> properties;
            if (settingsManager.TryGetObjectByProperty(SettingsConstants.DisplayNameField, displayName, out uniqueKey, out properties))
            {
                settingsManager.UnregisterObject(uniqueKey);
            }
        }

        /// <summary>
        /// Rename an object in the store.
        /// </summary>
        /// <param name="oldDisplayName"></param>
        /// <param name="newDisplayName"></param>
        /// <param name="force">if true and destination object already exists overwrite it</param>
        public void RenameObject(string oldDisplayName, string newDisplayName, bool force)
        {
            Dictionary<string, string> fromObject;
            string fromUniqueKey;
            Dictionary<string, string> toObject;
            string toUniqueKey;

            if (TryGetObject(oldDisplayName, out fromUniqueKey, out fromObject))
            {
                if (TryGetObject(newDisplayName, out toUniqueKey, out toObject))
                {
                    //  if oldDisplayName == newDisplayName it's a no op
                    if (!string.Equals(oldDisplayName, newDisplayName, StringComparison.Ordinal))
                    {
                        if (force)
                        {
                            settingsManager.UnregisterObject(toUniqueKey);
                            // recursive call with force == false now that the destination object is gone
                            RenameObject(oldDisplayName, newDisplayName, false);
                        }
                        else
                            throw new ArgumentException("Cannot rename object. The destination object '" + newDisplayName + "' already exists.");
                    }
                }
                else
                {
                    fromObject[SettingsConstants.DisplayNameField] = newDisplayName;
                    settingsManager.RegisterObject(fromUniqueKey, fromObject);
                }
            }
            else
                throw new ArgumentException("Cannot rename object. The source object '" + oldDisplayName + "' does not exist.");

        }

        /// <summary>
        /// Copy an object in the store.
        /// The new object will be a copy of the original, except it will be assigned a new unique key.
        /// </summary>
        /// <param name="fromDisplayName"></param>
        /// <param name="toDisplayName"></param>
        /// <param name="force">if true and destination object already exists overwrite it</param>
        public void CopyObject(string fromDisplayName, string toDisplayName, bool force)
        {
            Dictionary<string, string> fromObject;
            Dictionary<string, string> toObject;
            string toUniqueKey;

            if (TryGetObject(fromDisplayName, out fromObject))
            {
                if (TryGetObject(toDisplayName, out toUniqueKey, out toObject))
                {
                    //  if fromDisplayName == toDisplayName it's a no op
                    if (!string.Equals(fromDisplayName, toDisplayName, StringComparison.Ordinal))
                    {
                        if (force)
                        {
                            settingsManager.UnregisterObject(toUniqueKey);
                            // recursive call with force == false now that the destination object is gone
                            CopyObject(fromDisplayName, toDisplayName, force);
                        }
                        else
                            throw new ArgumentException("Cannot copy object. The destination object '" + toDisplayName + "' already exists.");
                    }
                }
                else
                {
                    // Register the copy.  A new unique key will be automatically assigned.
                    RegisterObject(toDisplayName, fromObject);
                }
            }
            else
                throw new ArgumentException("Cannot copy object. The source object '" + fromDisplayName + "' does not exist.");
        }

        /// <summary>
        /// Get a list of the display names for the objects in the store.
        /// </summary>
        /// <returns>A list of display names.</returns>
        public List<string> ListObjectNames()
        {
            return settingsManager.SelectProperty(SettingsConstants.DisplayNameField);
        }
    }
}
