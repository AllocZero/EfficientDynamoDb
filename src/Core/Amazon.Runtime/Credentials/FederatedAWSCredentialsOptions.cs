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
using System;
using System.Net;

namespace Amazon.Runtime
{
#if !NETSTANDARD13
    /// <summary>
    /// Options that control the behavior of <see cref="FederatedAWSCredentials"/>.
    /// </summary>
    public class FederatedAWSCredentialsOptions
    {
        private readonly object syncLock = new object();

        private Func<CredentialRequestCallbackArgs, NetworkCredential> credentialRequestCallback;
        private object customCallbackState;
        private string userIdentity;
        private string profileName;
        private RegionEndpoint stsRegion;
        private WebProxy proxySettings;

        /// <summary>
        /// The identity to use when authenticating.
        /// This is in the form domain\username, or just username.
        /// If this option is not set, the current user's identity will be used.
        /// </summary>
        public string UserIdentity
        {
            get
            {
                lock(syncLock)
                {
                    return userIdentity;
                }
            }
            set
            {
                lock(syncLock)
                {
                    userIdentity = value;
                }
            }
        }

        /// <summary>
        /// <see cref="FederatedAWSCredentials"/> uses this callback to get credentials if the UserIdentity is set.
        /// </summary>
        public Func<CredentialRequestCallbackArgs, NetworkCredential> CredentialRequestCallback
        {
            get
            {
                lock (syncLock)
                {
                    return credentialRequestCallback;
                }
            }
            set
            {
                lock (syncLock)
                {
                    credentialRequestCallback = value;
                }
            }
        }

        /// <summary>
        /// When <see cref="FederatedAWSCredentials"/> makes a callback for credentials,
        /// this state will be included for use by the callback handler.
        /// </summary>
        public object CustomCallbackState
        {
            get
            {
                lock(syncLock)
                {
                    return customCallbackState;
                }
            }
            set
            {
                lock(syncLock)
                {
                    customCallbackState = value;
                }
            }
        }

        /// <summary>
        /// The proxy settings to use when calling the
        /// Secure Token Service and the ADFS endpoint.
        /// </summary>
        public WebProxy ProxySettings
        {
            get
            {
                lock (syncLock)
                {
                    return proxySettings;
                }
            }
            set
            {
                lock (syncLock)
                {
                    proxySettings = value;
                }
            }
        }

        /// <summary>
        /// Set this property when a region-specific STS endpoint must be used (eg cn-north-1).
        /// If left empty/null, the global sts.amazonaws.com endpoint will be used when credentials are obtained.
        /// </summary>
        public RegionEndpoint STSRegion
        {
            get
            {
                lock (syncLock)
                {
                    return stsRegion;
                }
            }
            set
            {
                lock (syncLock)
                {
                    stsRegion = value;
                }
            }
        }

        /// <summary>
        /// The profile name, if this FederatedAWSCredentialsOptions
        /// is associated with a CredentialProfile.
        /// </summary>
        public string ProfileName
        {
            get
            {
                lock (syncLock)
                {
                    return profileName;
                }
            }
            set
            {
                lock (syncLock)
                {
                    profileName = value;
                }
            }
        }
    }
#endif
}
