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

namespace Amazon.Runtime.CredentialManagement
{
    /// <summary>
    /// Interface to define the necessary operations for a CredentialProfile storage mechanism.
    /// </summary>
    public interface ICredentialProfileStore : ICredentialProfileSource
    {
        /// <summary>
        /// Rename the profile with oldProfileName to newProfileName.
        /// </summary>
        /// <param name="oldProfileName">The profile to rename.</param>
        /// <param name="newProfileName">The new name for the profile.</param>
        void RenameProfile(string oldProfileName, string newProfileName);

        /// <summary>
        /// Rename the profile with oldProfileName to newProfileName.
        /// </summary>
        /// <param name="oldProfileName">The profile to rename.</param>
        /// <param name="newProfileName">The new name for the profile.</param>
        /// <param name="force">If true and the destination profile exists it will be overwritten.</param>
        void RenameProfile(string oldProfileName, string newProfileName, bool force);

        /// <summary>
        /// Make a copy of the profile with fromProfileName called toProfileName.
        /// </summary>
        /// <param name="fromProfileName">The name of the profile to copy from.</param>
        /// <param name="toProfileName">The name of the new profile.</param>
        void CopyProfile(string fromProfileName, string toProfileName);

        /// <summary>
        /// Make a copy of the profile with fromProfileName called toProfileName.
        /// </summary>
        /// <param name="fromProfileName">The name of the profile to copy from.</param>
        /// <param name="toProfileName">The name of the new profile.</param>
        /// <param name="force">If true and the destination profile exists it will be overwritten.</param>
        void CopyProfile(string fromProfileName, string toProfileName, bool force);

        /// <summary>
        /// Add the given profile to the store, or update it if one with the same name already exists.
        /// </summary>
        /// <param name="profile"></param>
        void RegisterProfile(CredentialProfile profile);

        /// <summary>
        /// Delete the profile with profileName if it exists.
        /// </summary>
        /// <param name="profileName">The name of the profile to delete.</param>
        void UnregisterProfile(string profileName);

        /// <summary>
        /// Get a list of valid profile names from this store.
        /// Invalid profiles are ignored.
        /// See <see cref="CredentialProfileOptions"/> for more information
        /// about valid profiles.
        /// </summary>
        /// <returns></returns>
        List<string> ListProfileNames();

        /// <summary>
        /// Get a list of valid profiles from this store.
        /// Invalid profiles are ignored.
        /// See <see cref="CredentialProfileOptions"/> for more information
        /// about valid profiles.
        /// </summary>
        /// <returns></returns>
        List<CredentialProfile> ListProfiles();
    }
}
