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
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Amazon.Runtime.CredentialManagement
{
    /// <summary>
    /// A named group of options that are persisted and used to obtain AWSCredentials.
    /// </summary>
    public class CredentialProfile
    {

        private Dictionary<string, string> _properties;

        /// <summary>
        /// The name of the CredentialProfile
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The options to be used to create AWSCredentials.
        /// </summary>
        public CredentialProfileOptions Options { get; private set; }

        /// <summary>
        /// The region to be used with this CredentialProfile
        /// </summary>
        public RegionEndpoint Region { get; set; }

        /// <summary>
        /// The unique key for this CredentialProfile.
        /// This key is used by the Visual Studio Tooklit to associate external artifacts with this profile.
        /// </summary>
        internal Guid? UniqueKey { get; set; }

        /// <summary>
        /// The endpoint discovery enabled value for this CredentialProfile
        /// </summary>
        public bool? EndpointDiscoveryEnabled { get; set; }

        /// <summary>
        /// If true the region identified in the S3 access point arn will be used when making requests.
        /// </summary>
        public bool? S3UseArnRegion { get; set; }
        
        /// <summary>
        /// The Sts Regional Endpoints Value as either legacy or regional
        /// </summary>
        public StsRegionalEndpointsValue? StsRegionalEndpoints { get; set; }

        /// <summary>
        /// The S3 Regional Endpoint Value as either legacy or regional
        /// </summary>
        public S3UsEast1RegionalEndpointValue? S3RegionalEndpoint { get; set; }

        /// <summary>
        /// The request retry mode  as legacy, standard, or adaptive
        /// </summary>
        public RequestRetryMode? RetryMode { get; set; }

        /// <summary>
        /// Specified how many HTTP requests an SDK should make for a single
        /// SDK operation invocation before giving up.
        /// </summary>
        public int? MaxAttempts { get; set; }

        /// <summary>
        /// An optional dictionary of name-value pairs stored with the CredentialProfile
        /// </summary>
        internal Dictionary<string, string> Properties
        {
            get => _properties ?? (_properties = new Dictionary<string, string>());
            set => _properties = value;
        }

        /// <summary>
        /// True if the properties of the Options object can be converted into AWSCredentials, false otherwise.
        /// See <see cref="CredentialProfileOptions"/> for more details about which options are available.
        /// </summary>
        public bool CanCreateAWSCredentials => ProfileType.HasValue;

        /// <summary>
        /// The <see cref="ICredentialProfileStore"/> that this <see cref="CredentialProfile"/> is associated with.
        /// Null if this <see cref="CredentialProfile"/> is not associated with a <see cref="ICredentialProfileStore"/>.
        /// </summary>
        public ICredentialProfileStore CredentialProfileStore
        {
            get;
            internal set;
        }

        /// <summary>
        /// If CanCreateAWSCredentials is true, returns a short description of the type of
        /// credentials that would be created.
        /// If CanCreateAWSCredentials is false, return null.
        /// </summary>
        public string CredentialDescription => CredentialProfileTypeDetector.GetUserFriendlyCredentialType(ProfileType);

        /// <summary>
        /// The CredentialProfileType of this CredentialProfile, if one applies.
        /// </summary>
        internal CredentialProfileType? ProfileType => CredentialProfileTypeDetector.DetectProfileType(Options);

        /// <summary>
        /// Determine this CredentialProfile will generate AWSCredentials that require a callback to be set on them.
        /// </summary>
        internal bool IsCallbackRequired => AWSCredentialsFactory.IsCallbackRequired(ProfileType);

        /// <summary>
        /// Construct a new CredentialProfile.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="profileOptions"></param>
        public CredentialProfile(string name, CredentialProfileOptions profileOptions)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name must not be null or empty.");
            }

            Options = profileOptions ?? throw new ArgumentNullException("profileOptions");   
            Name = name;       
        }

        /// <summary>
        /// Gets the AWSCredentials for this profile if CanCreateAWSCredentials is true
        /// and AWSCredentials can be created.  Throws an exception otherwise.
        ///
        /// See <see cref="CredentialProfileOptions"/> for a list of AWSCredentials returned by this method.
        /// </summary>
        /// <param name="profileSource">The profile source, for profiles that reference other profiles.</param>
        /// <returns>AWSCredentials for this profile.</returns>
        public AWSCredentials GetAWSCredentials(ICredentialProfileSource profileSource)
        {
            return GetAWSCredentials(profileSource, false);
        }

        /// <summary>
        /// Gets the AWSCredentials for this profile if CanCreateAWSCredentials is true
        /// and AWSCredentials can be created.  Throws an exception otherwise.
        ///
        /// See <see cref="CredentialProfileOptions"/> for a list of AWSCredentials returned by this method.
        /// </summary>
        /// <param name="profileSource">The profile source, for profiles that reference other profiles.</param>
        /// <param name="nonCallbackOnly">If true, throw a descriptive exception for any credentials that would not operate as-is.
        /// In other words, any credentials that require programmatic callbacks at runtime.</param>
        /// <returns>AWSCredentials for this profile.</returns>
        internal AWSCredentials GetAWSCredentials(ICredentialProfileSource profileSource, bool nonCallbackOnly)
        {
            return AWSCredentialsFactory.GetAWSCredentials(this, profileSource, nonCallbackOnly);
        }

        private string GetPropertiesString()
        {
            return "{" + string.Join(",", Properties.OrderBy(p=>p.Key).Select(p => p.Key + "=" + p.Value).ToArray()) + "}";
        }

        public override string ToString()
        {
            return "[Name=" + Name + "," +
                "Options = " + Options + "," +
                "Region = " + (Region == null ? "" : Region.SystemName) + "," +
                "Properties = " + GetPropertiesString() + "," +
                "ProfileType = " + ProfileType + "," +
                "UniqueKey = " + UniqueKey + "," +
                "CanCreateAWSCredentials = " + CanCreateAWSCredentials + "," +
                "RetryMode= " + RetryMode + "," +
                "MaxAttempts= " + MaxAttempts +
                "]";
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            var p = obj as CredentialProfile;
            if (p == null)
                return false;

            return AWSSDKUtils.AreEqual(
                new object[] { Name, Options, Region, ProfileType, CanCreateAWSCredentials, UniqueKey },
                new object[] { p.Name, p.Options, p.Region, p.ProfileType, p.CanCreateAWSCredentials, p.UniqueKey }) &&
                AWSSDKUtils.DictionariesAreEqual(Properties, p.Properties);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(Name, Options, Region, ProfileType, CanCreateAWSCredentials, GetPropertiesString(), UniqueKey);
        }
    }
}
