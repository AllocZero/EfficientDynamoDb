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
using System.Collections.Generic;

namespace Amazon.Runtime.CredentialManagement
{
    /// <summary>
    /// Interface to define the necessary operations for a CredentialProfile lookup mechanism.
    /// </summary>
    public interface ICredentialProfileSource
    {
        /// <summary>
        /// Get the profile with the given name, if one exists.
        /// </summary>
        /// <param name="profileName">The name of the profile to get.</param>
        /// <param name="profile">The profile, if it was found, null otherwise.</param>
        /// <returns>True if the profile was found, false otherwise.</returns>
        bool TryGetProfile(string profileName, out CredentialProfile profile);
    }
}
