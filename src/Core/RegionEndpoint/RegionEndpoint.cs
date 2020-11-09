/*******************************************************************************
 *  Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *  this file except in compliance with the License. A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 *  or in the "license" file accompanying this file.
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *  specific language governing permissions and limitations under the License.
 * *****************************************************************************
 *    __  _    _  ___
 *   (  )( \/\/ )/ __)
 *   /__\ \    / \__ \
 *  (_)(_) \/\/  (___/
 *
 *  AWS SDK for .NET
 *
 */
using Amazon.Internal;
using Amazon.Runtime.Internal.Auth;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Amazon
{
    /// <summary>
    /// This class contains region information used to lazily compute the service endpoints. The static constants representing the 
    /// regions can be used while constructing the AWS client instead of looking up the exact endpoint URL.
    /// </summary>
    public partial class RegionEndpoint
    {
        #region Statics
        private static Dictionary<string, RegionEndpoint> _hashBySystemName = new Dictionary<string, RegionEndpoint>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// The US East (Virginia) endpoint.
        /// </summary>
        public static readonly RegionEndpoint USEast1 = GetEndpoint("us-east-1", "US East (Virginia)");

        /// <summary>
        /// The US East (Virginia) regional endpoint.
        /// </summary>
        private static readonly RegionEndpoint USEast1Regional = GetEndpoint("us-east-1-regional", "US East (Virginia) regional");

        /// <summary>
        /// The US East (Ohio) endpoint.
        /// </summary>
        public static readonly RegionEndpoint USEast2 = GetEndpoint("us-east-2", "US East (Ohio)");

        /// <summary>
        /// The US West (N. California) endpoint.
        /// </summary>
        public static readonly RegionEndpoint USWest1 = GetEndpoint("us-west-1", "US West (N. California)");

        /// <summary>
        /// The US West (Oregon) endpoint.
        /// </summary>
        public static readonly RegionEndpoint USWest2 = GetEndpoint("us-west-2", "US West (Oregon)");

        /// <summary>
        /// The EU North (Stockholm) endpoint.
        /// </summary>
        public static readonly RegionEndpoint EUNorth1 = GetEndpoint("eu-north-1", "EU North (Stockholm)");

        /// <summary>
        /// The EU West (Ireland) endpoint.
        /// </summary>
        public static readonly RegionEndpoint EUWest1 = GetEndpoint("eu-west-1", "EU West (Ireland)");

        /// <summary>
        /// The EU West (London) endpoint.
        /// </summary>
        public static readonly RegionEndpoint EUWest2 = GetEndpoint("eu-west-2", "EU West (London)");

        /// <summary>
        /// The EU West (Paris) endpoint.
        /// </summary>
        public static readonly RegionEndpoint EUWest3 = GetEndpoint("eu-west-3", "EU West (Paris)");

        /// <summary>
        /// The EU Central (Frankfurt) endpoint.
        /// </summary>
        public static readonly RegionEndpoint EUCentral1 = GetEndpoint("eu-central-1", "EU Central (Frankfurt)");

        /// <summary>
        /// The Europe (Milan) endpoint.
        /// </summary>
        public static readonly RegionEndpoint EUSouth1 = GetEndpoint("eu-south-1", "Europe (Milan)");

        /// <summary>
        /// The Asia Pacific (Hong Kong) endpoint.
        /// </summary>
        public static readonly RegionEndpoint APEast1 = GetEndpoint("ap-east-1", "Asia Pacific (Hong Kong)");

        /// <summary>
        /// The Asia Pacific (Tokyo) endpoint.
        /// </summary>
        public static readonly RegionEndpoint APNortheast1 = GetEndpoint("ap-northeast-1", "Asia Pacific (Tokyo)");

        /// <summary>
        /// The Asia Pacific (Seoul) endpoint.
        /// </summary>
        public static readonly RegionEndpoint APNortheast2 = GetEndpoint("ap-northeast-2", "Asia Pacific (Seoul)");

        /// <summary>
        /// The Asia Pacific (Osaka-Local) endpoint.
        /// </summary>
        public static readonly RegionEndpoint APNortheast3 = GetEndpoint("ap-northeast-3", "Asia Pacific (Osaka-Local)");

        /// <summary>
        /// The Asia Pacific (Mumbai) endpoint.
        /// </summary>
        public static readonly RegionEndpoint APSouth1 = GetEndpoint("ap-south-1", "Asia Pacific (Mumbai)");

        /// <summary>
        /// The Asia Pacific (Singapore) endpoint.
        /// </summary>
        public static readonly RegionEndpoint APSoutheast1 = GetEndpoint("ap-southeast-1", "Asia Pacific (Singapore)");

        /// <summary>
        /// The Asia Pacific (Sydney) endpoint.
        /// </summary>
        public static readonly RegionEndpoint APSoutheast2 = GetEndpoint("ap-southeast-2", "Asia Pacific (Sydney)");

        /// <summary>
        /// The South America (Sao Paulo) endpoint.
        /// </summary>
        public static readonly RegionEndpoint SAEast1 = GetEndpoint("sa-east-1", "South America (Sao Paulo)");
		
		/// <summary>
        /// The US GovCloud East (Virginia) endpoint.
        /// </summary>
        public static readonly RegionEndpoint USGovCloudEast1 = GetEndpoint("us-gov-east-1", "US GovCloud East (Virginia)");

        /// <summary>
        /// The US GovCloud West (Oregon) endpoint.
        /// </summary>
        public static readonly RegionEndpoint USGovCloudWest1 = GetEndpoint("us-gov-west-1", "US GovCloud West (Oregon)");

        /// <summary>
        /// The China (Beijing) endpoint.
        /// </summary>
        public static readonly RegionEndpoint CNNorth1 = GetEndpoint("cn-north-1", "China (Beijing)");

        /// <summary>
        /// The China (Ningxia) endpoint.
        /// </summary>
        public static readonly RegionEndpoint CNNorthWest1 = GetEndpoint("cn-northwest-1", "China (Ningxia)");

        /// <summary>
        /// The Canada (Central) endpoint.
        /// </summary>
        public static readonly RegionEndpoint CACentral1 = GetEndpoint("ca-central-1", "Canada (Central)");

        /// <summary>
        /// The Middle East (Bahrain) endpoint.
        /// </summary>
        public static readonly RegionEndpoint MESouth1 = GetEndpoint("me-south-1", "Middle East (Bahrain)");

        /// <summary>
        /// The Africa (Cape Town) endpoint.
        /// </summary>
        public static readonly RegionEndpoint AFSouth1 = GetEndpoint("af-south-1", "Africa (Cape Town)");

        /// <summary>
        /// Represents the endpoint overridding rules in the endpoints.json
        /// Is used to map private region (ie us-east-1-regional) to public regions (us-east-1)
        /// For signing purposes.
        /// </summary>
        private static Dictionary<RegionEndpoint, RegionEndpoint> _hashRegionEndpointOverride  = new Dictionary<RegionEndpoint, RegionEndpoint>() {
                { USEast1Regional, USEast1 }
            };

        /// <summary>
        /// Enumerate through all the regions.
        /// </summary>
        public static IEnumerable<RegionEndpoint> EnumerableAllRegions
        {
            get
            {
                List<RegionEndpoint> list = new List<RegionEndpoint>();
                foreach (IRegionEndpoint endpoint in RegionEndpointProvider.AllRegionEndpoints)
                {                        
                    list.Add(GetEndpoint(endpoint.RegionName, endpoint.DisplayName));
                }
                return list;
            }
        }

        /// <summary>
        /// Gets the region based on its system name like "us-west-1"
        /// </summary>
        /// <param name="systemName">The system name of the service like "us-west-1"</param>
        /// <returns></returns>
        public static RegionEndpoint GetBySystemName(string systemName)
        {   
            return GetEndpoint(systemName, null);
        }

        /// <summary>
        /// Gets the region endpoint override if exists
        /// </summary>
        /// <param name="regionEndpoint">The region endpoint to find the possible override for</param>
        /// <returns></returns>
        public static RegionEndpoint GetRegionEndpointOverride(RegionEndpoint regionEndpoint)
        {
            return _hashRegionEndpointOverride.ContainsKey(regionEndpoint) ? _hashRegionEndpointOverride[regionEndpoint] : null;
        }

        private static RegionEndpoint GetEndpoint(string systemName, string displayName)
        {
            RegionEndpoint regionEndpoint = null;
            if (displayName == null)
            {
                lock (_hashBySystemName)
                {
                    if (_hashBySystemName.TryGetValue(systemName, out regionEndpoint))
                        return regionEndpoint;
                }

                // GetRegionEndpoint will always return a non-null value. If the the region(systemName) is unknown,
                // the providers will create a fallback instance that will generate an endpoint to the best
                // of its knowledge.
                displayName = RegionEndpointProvider.GetRegionEndpoint(systemName).DisplayName;
            }
            
            lock (_hashBySystemName)
            {
                if (_hashBySystemName.TryGetValue(systemName, out regionEndpoint))
                    return regionEndpoint;

                regionEndpoint = new RegionEndpoint(systemName, displayName);
                _hashBySystemName.Add(regionEndpoint.SystemName, regionEndpoint);
            }

            return regionEndpoint;
        }

        private static IRegionEndpointProvider _regionEndpointProvider;
        private static IRegionEndpointProvider RegionEndpointProvider
        {
            get
            {
                if (_regionEndpointProvider == null)
                {
                    // If the existing customer provided the endpoints.json file via
                    // <aws endpointDefinition=""/>, it's in v2 format.  We we will create
                    // a v2 provider which does a fall through during LoadEndpointDefinitions()
                    // and loads from the override file provided by the user.
                    //
                    // Else, we are loading from the assembly resource.  In which case we use the 
                    // latest provider.
                    //
                    // It's actually a bug that _regionEndpointProvider is a static member variable
                    // since the IEndpointProvider should respect the AWSConfigs.EndpointDefinition
                    // _at_ the time of the service client instantiation.  However, since this is
                    // the existing behavior with v2 endpoint file format, we will preserve this behavior as is.
                    if (!string.IsNullOrEmpty(AWSConfigs.EndpointDefinition))
                    {
                        _regionEndpointProvider = new RegionEndpointProviderV2();
                    }
                    else
                    {
                        _regionEndpointProvider = new RegionEndpointProviderV3();
                    }
                }
                return _regionEndpointProvider;
            }
        }
        #endregion

        private RegionEndpoint(string systemName, string displayName)
        {
            this.SystemName = systemName;
            this.DisplayName = displayName;
        }

        /// <summary>
        /// Gets the system name of a region.
        /// </summary>
        public string SystemName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the display name of a region.
        /// </summary>
        public string DisplayName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the partition name the region is in. For example for us-east-1 the partition name is aws. For cn-northwest-1 the partition name is aws-cn.
        /// </summary>
        public string PartitionName
        {
            get
            {
                var regionEndpointV3 = this.InternedRegionEndpoint as RegionEndpointV3;
                return regionEndpointV3?.PartitionName;
            }
        }

        /// <summary>
        /// Gets the dns suffix for the region endpoints in a partition. For example the aws partition's suffix is amazonaws.com. The aws-cn partition's suffix is amazonaws.com.cn.
        /// </summary>
        public string PartitionDnsSuffix
        {
            get
            {
                var regionEndpointV3 = this.InternedRegionEndpoint as RegionEndpointV3;
                return regionEndpointV3?.PartitionDnsSuffix;
            }
        }

        private IRegionEndpoint InternedRegionEndpoint
        {
            get
            {
                return RegionEndpointProvider.GetRegionEndpoint(SystemName);
            }
        }

        /// <summary>
        /// Gets the endpoint for a service in a region.
        /// </summary>
        /// <param name="serviceName">
        /// The services system name. Service system names can be obtained from the
        /// RegionEndpointServiceName member of the ClientConfig-derived class for the service.
        /// </param>
        /// <param>
        /// For forwards compatibility, if the service being requested for isn't known in the region, this method 
        /// will generate an endpoint using the AWS endpoint heuristics. In this case, it is not guaranteed the
        /// endpoint will point to a valid service endpoint.
        /// </param>
        /// <returns></returns>
        public Endpoint GetEndpointForService(string serviceName)
        {
            return GetEndpointForService(serviceName, false);
        }

        /// <summary>
        /// Gets the endpoint for a service in a region, optionally selecting a dualstack compatible endpoint.
        /// </summary>
        /// <param name="serviceName">
        /// The services system name. Service system names can be obtained from the
        /// RegionEndpointServiceName member of the ClientConfig-derived class for the service.
        /// </param>
        /// <param name="dualStack">
        /// If true a dualstack endpoint is returned. It is the user's responsibility to verify that the given service
        /// supports a dualstack endpoint for the region.
        /// </param>
        /// <param>
        /// For forwards compatibility, if the service being requested for isn't known in the region, this method 
        /// will generate an endpoint using the AWS endpoint heuristics. In this case, it is not guaranteed the
        /// endpoint will point to a valid service endpoint.
        /// </param>
        /// <returns></returns>
        public Endpoint GetEndpointForService(string serviceName, bool dualStack)
        {
            return InternedRegionEndpoint.GetEndpointForService(serviceName, dualStack);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", this.DisplayName, this.SystemName);
        }

        /// <summary>
        /// This class defines an endpoints hostname and which protocols it supports.
        /// </summary>
        public class Endpoint
        {
            internal Endpoint(string hostname, string authregion, string signatureVersionOverride)
            {
                this.Hostname = hostname;
                this.AuthRegion = authregion;
                this.SignatureVersionOverride = signatureVersionOverride;
            }

            /// <summary>
            /// Gets the hostname for the service.
            /// </summary>
            public string Hostname
            {
                get;
                private set;
            }

            /// <summary>
            /// The authentication region to be used in request signing.
            /// </summary>
            public string AuthRegion
            {
                get;
                private set;
            }

            public override string ToString()
            {
                return this.Hostname;
            }

            /// <summary>
            /// This property is only set for S3 endpoints.  For all other services this property returns null.
            /// For S3 endpoints, if the endpoint supports signature version 2 this property will be "2", otherwise it will be "4".
            /// </summary>
            public string SignatureVersionOverride
            {
                get;
                private set;
            }
        }
    }
}
