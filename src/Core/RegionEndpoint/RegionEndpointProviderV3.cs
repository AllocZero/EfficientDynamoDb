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
using Amazon.Runtime;
using System.Globalization;
using Amazon.Runtime.Internal.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using ThirdParty.Json.LitJson;

namespace Amazon.Internal
{
    public class RegionEndpointV3 : IRegionEndpoint
    {
        private ServiceMap _serviceMap = new ServiceMap();
        public string RegionName { get; private set; }
        public string DisplayName { get; private set; }
        public string PartitionName
        {
            get
            {
                return (string)_partitionJsonData["partition"];
            }
        }

        public string PartitionDnsSuffix
        {
            get
            {
                return (string)_partitionJsonData["dnsSuffix"];
            }
        }

        private JsonData _partitionJsonData;
        private JsonData _servicesJsonData;
        private bool _servicesLoaded = false;

        public RegionEndpointV3(string regionName, string displayName, JsonData partition, JsonData services)
        {
            RegionName = regionName;
            DisplayName = displayName;

            _partitionJsonData = partition;
            _servicesJsonData = services;
        }

        public RegionEndpoint.Endpoint GetEndpointForService(string serviceName, bool dualStack)
        {
            RegionEndpoint.Endpoint endpointObject = null;

            lock (_serviceMap)
            {
                if (!_servicesLoaded)
                {
                    ParseAllServices();
                    _servicesLoaded = true;
                }

                if (!_serviceMap.TryGetEndpoint(serviceName, dualStack, out endpointObject))
                {
                    // Do a fallback of creating an unknown endpoint based on the
                    // current region's hostname template.
                    endpointObject = CreateUnknownEndpoint(serviceName, dualStack);
                }
            }
            return endpointObject;
        }

        private RegionEndpoint.Endpoint CreateUnknownEndpoint(string serviceName, bool dualStack)
        {
            string template = (string)_partitionJsonData["defaults"]["hostname"];

            if (dualStack)
            {
                template = template.Replace("{region}", "dualstack.{region}");
            }

            string hostname = template.Replace("{service}", serviceName)
                                 .Replace("{region}", RegionName)
                                 .Replace("{dnsSuffix}", (string)_partitionJsonData["dnsSuffix"]);

            return new RegionEndpoint.Endpoint(hostname, null, null);
        }

        private void ParseAllServices()
        {
            foreach (string serviceName in _servicesJsonData.PropertyNames)
            {
                if(_servicesJsonData[serviceName]!=null&&_servicesJsonData[serviceName].Count>0)
                    AddServiceToMap(_servicesJsonData[serviceName], serviceName);
            }
        }

        private void AddServiceToMap(JsonData service, string serviceName)
        {
            string partitionEndpoint = service["partitionEndpoint"] != null ? (string)service["partitionEndpoint"] : "";
            bool isRegionalized = service["isRegionalized"] != null ? (bool)service["isRegionalized"] : true;

            string serviceEndpoint = RegionName;

            if (!isRegionalized && !string.IsNullOrEmpty(partitionEndpoint))
            {
                serviceEndpoint = partitionEndpoint;
            }

            JsonData regionEndpoint = service["endpoints"][serviceEndpoint];
            if (regionEndpoint == null)
            {
                return;
            }

            JsonData result = new JsonData();
            MergeJsonData(result, regionEndpoint);
            MergeJsonData(result, service["defaults"]);
            MergeJsonData(result, _partitionJsonData["defaults"]);

            CreateEndpointAndAddToServiceMap(result, RegionName, serviceName);
        }

        private static void MergeJsonData(JsonData target, JsonData source)
        {
            if (source == null || target == null)
            {
                return;
            }
            foreach (var propertyName in source.PropertyNames)
            {
                if (target[propertyName] == null)
                {
                    target[propertyName] = source[propertyName];
                }
            }
        }

        private void CreateEndpointAndAddToServiceMap(JsonData result, string regionName, string serviceName)
        {
            CreateEndpointAndAddToServiceMap(result, regionName, serviceName, false);
            CreateEndpointAndAddToServiceMap(result, regionName, serviceName, true);
        }

        private void CreateEndpointAndAddToServiceMap(JsonData result, string regionName, string serviceName, bool dualStack)
        {
            string template = (string)result["hostname"];
            string hostname = template.Replace("{service}", serviceName)
                                 .Replace("{region}", regionName)
                                 .Replace("{dnsSuffix}", (string)_partitionJsonData["dnsSuffix"]);

            if (dualStack)
            {
                // We need special handling for S3's s3.amazonaws.com endpoint, which doesn't
                // support dualstack (need to transform to s3.dualstack.us-east-1.amazonaws.com).
                // Other endpoints that begin s3-* need to transform to s3.* for dualstack support.
                // S3's 'external' endpoints do not support dualstack and should not be transformed.
                if (serviceName.Equals("s3", StringComparison.OrdinalIgnoreCase))
                {
                    if (hostname.Equals("s3.amazonaws.com", StringComparison.OrdinalIgnoreCase))
                    {
                        hostname = "s3.dualstack.us-east-1.amazonaws.com";
                    }
                    else
                    {
                        var isExternalEndpoint = hostname.StartsWith("s3-external-", StringComparison.OrdinalIgnoreCase);
                        if (!isExternalEndpoint)
                        {
                            // transform fixed s3-<region> to s3.<region> and then onto s3.dualstack.<region>,
                            // bypassing endpoints that do not start with the expected tags.
                            if (hostname.StartsWith("s3-", StringComparison.OrdinalIgnoreCase))
                                hostname = "s3." + hostname.Substring(3);

                            if (hostname.StartsWith("s3.", StringComparison.OrdinalIgnoreCase))
                                hostname = hostname.Replace("s3.", "s3.dualstack.");
                        }
                    }
                }
                else if (serviceName.Equals("s3-control", StringComparison.OrdinalIgnoreCase))
                {
                    // transform s3-control.<region>.amazonaws.com or s3-control-fips.<region>.amazonaws.com into
                    // s3-control.dualstack.<region>.amazonaws.com and s3-control-fips.dualstack.<region>.amazonaws.com
                    if (hostname.StartsWith("s3-control", StringComparison.OrdinalIgnoreCase))
                    {
                        int firstDot = hostname.IndexOf('.');
                        if (firstDot >= 0)
                        {
                            hostname = hostname.Substring(0, firstDot) + ".dualstack." + hostname.Substring(firstDot + 1);
                        }
                    }
                }
                else
                {
                    // For certain region and endpoint combination, we actually get an explicit endpoint as "hostname" property
                    // (e.g. sts.ap-northeast-2.amazon.com). We can't assume that the template variable will be {service}.{region}.{dnsSuffix},
                    // so just construct a brand new endpoint.
                    hostname = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}", serviceName,
                                                            "dualstack." + regionName,
                                                            (string)_partitionJsonData["dnsSuffix"]);
                }
            }

            string authRegion = null;
            string customService = null;
            JsonData credentialScope = result["credentialScope"];
            if (credentialScope != null)
            {
                authRegion = DetermineAuthRegion(credentialScope);
                // This is a workaround until we overhaul how the SDK consumes the v3 endpoint structure
                // and customize the signing based on the metadata in the endpoint structure.
                // There are points in SDK where we retrieve endpoints via service name such as "execute-api".
                // Since we are building a cache, just add the custom service entry.
                if (credentialScope["service"] != null && string.Compare((string)credentialScope["service"], serviceName, StringComparison.OrdinalIgnoreCase) != 0)
                {
                    customService = (string)credentialScope["service"];
                }
            }

            string signatureOverride = DetermineSignatureOverride(result, serviceName);

            RegionEndpoint.Endpoint endpoint = new RegionEndpoint.Endpoint(hostname, authRegion, signatureOverride);

            _serviceMap.Add(serviceName, dualStack, endpoint);
            if (!string.IsNullOrEmpty(customService) && !_serviceMap.ContainsKey(customService))
            {
                _serviceMap.Add(customService, dualStack, endpoint);
            }
        }

        private static string DetermineSignatureOverride(JsonData defaults, string serviceName)
        {
            if (string.Equals(serviceName, "s3", StringComparison.OrdinalIgnoreCase))
            {
                bool supportsSigV2 = false;
                foreach (JsonData element in defaults["signatureVersions"])
                {
                    string sig = (string)element;
                    if (string.Equals(sig, "s3", StringComparison.OrdinalIgnoreCase))
                    {
                        supportsSigV2 = true;
                        break;
                    }
                }
                return (supportsSigV2 ? "2" : "4");
            }

            return null;
        }

        private static string DetermineAuthRegion(JsonData credentialScope)
        {
            string authRegion = null;
            if (credentialScope["region"] != null)
            {
                authRegion = (string) credentialScope["region"];
            }
            return authRegion;
        }

        class ServiceMap
        {
            private Dictionary<string, Amazon.RegionEndpoint.Endpoint> _serviceMap = new Dictionary<string, RegionEndpoint.Endpoint>();
            private Dictionary<string, Amazon.RegionEndpoint.Endpoint> _dualServiceMap = new Dictionary<string, RegionEndpoint.Endpoint>();

            private Dictionary<string, Amazon.RegionEndpoint.Endpoint> GetMap(bool dualStack)
            {
                return dualStack ? _dualServiceMap : _serviceMap;
            }

            public bool ContainsKey(string servicName)
            {
                return _serviceMap.ContainsKey(servicName);
            }

            public void Add(string serviceName, bool dualStack, RegionEndpoint.Endpoint endpoint)
            {
                var map = dualStack ? _dualServiceMap : _serviceMap;
                GetMap(dualStack).Add(serviceName, endpoint);
            }

            public bool TryGetEndpoint(string serviceName, bool dualStack, out RegionEndpoint.Endpoint endpoint)
            {
                return this.GetMap(dualStack).TryGetValue(serviceName, out endpoint);
            }
        }
    }

    public class RegionEndpointProviderV3 : IRegionEndpointProvider
    {
#if NETSTANDARD
        private const string ENDPOINT_JSON_RESOURCE = "Core.endpoints.json";
#else
        private const string ENDPOINT_JSON_RESOURCE = "Amazon.endpoints.json";
#endif
        private const string ENDPOINT_JSON = "endpoints.json";

        private JsonData _root;
        private Dictionary<string, IRegionEndpoint> _regionEndpointMap = new Dictionary<string, IRegionEndpoint>();
        private object _regionEndpointMapLock = new object();

        public RegionEndpointProviderV3()
        {
            using (var stream = GetEndpointJsonSourceStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                _root = JsonMapper.ToObject(reader);
            }
        }

        public RegionEndpointProviderV3(JsonData root)
        {
            _root = root;
        }

        private static Stream GetEndpointJsonSourceStream()
        {
#if BCL || (NETSTANDARD && !NETSTANDARD13)
            //
            // If the endpoints.json file has been provided next to the assembly:
            //
            string assemblyLocation = typeof(RegionEndpointProviderV3).Assembly.Location;
            if (!string.IsNullOrEmpty(assemblyLocation))
            {
                string endpointsPath = Path.Combine(Path.GetDirectoryName(assemblyLocation), ENDPOINT_JSON);
                if (File.Exists(endpointsPath))
                {
                    return File.Open(endpointsPath, FileMode.Open, FileAccess.Read);
                }
            }
#endif
            //
            // Default to endpoints.json file provided in the resource manifest:
            //
            return Amazon.Util.Internal.TypeFactory.GetTypeInfo(typeof(RegionEndpointProviderV3)).Assembly.GetManifestResourceStream(ENDPOINT_JSON_RESOURCE);
        }

        private object _allRegionEndpointsLock = new object();
        private IEnumerable<IRegionEndpoint> _allRegionEndpoints;
        public IEnumerable<IRegionEndpoint> AllRegionEndpoints
        {
            get
            {
                lock (_allRegionEndpointsLock)
                lock (_regionEndpointMapLock)
                {
                    if (_allRegionEndpoints == null)
                    {
                        JsonData partitions = _root["partitions"];
                        List<IRegionEndpoint> endpoints = new List<IRegionEndpoint>();
                        foreach (JsonData partition in partitions)
                        {
                            JsonData regions = partition["regions"];
                            foreach (string regionName in regions.PropertyNames)
                            {
                                IRegionEndpoint endpoint;
                                if (!_regionEndpointMap.TryGetValue(regionName, out endpoint))
                                {
                                    endpoint = new RegionEndpointV3(regionName, (string)regions[regionName]["description"], partition, partition["services"]);
                                    _regionEndpointMap.Add(regionName, endpoint);
                                }
                                endpoints.Add(endpoint);
                            }
                        }

                        _allRegionEndpoints = endpoints;
                    }
                }
                return _allRegionEndpoints;
            }
        }

        private object _allRegionRegexLock = new object();
        private IEnumerable<string> _allRegionRegex;
        public IEnumerable<string> AllRegionRegex
        {
            get
            {
                lock (_allRegionRegexLock)
                lock (_regionEndpointMapLock)
                {
                    if (_allRegionRegex == null)
                    {
                        JsonData partitions = _root["partitions"];
                        var allRegionRegex = new List<string>();
                        foreach (JsonData partition in partitions)
                        {
                            var regionRegex = (string)partition["regionRegex"];
                            allRegionRegex.Add(regionRegex);
                        }

                        _allRegionRegex = allRegionRegex;
                    }
                }
                return _allRegionRegex;
            }
        }

        private static string GetUnknownRegionDescription(string regionName)
        {
            if (regionName.StartsWith("cn-", StringComparison.OrdinalIgnoreCase) ||
                regionName.EndsWith("cn-global", StringComparison.OrdinalIgnoreCase))
            {
                return "China (Unknown)";
            }
            else
            {
                return "Unknown";
            }
        }

        private static bool IsRegionInPartition(string regionName, JsonData partition, out string description)
        {
            JsonData regionsData = partition["regions"];
            string regionPattern = (string)partition["regionRegex"];

            // see if the region name is a real region 
            if (regionsData[regionName] != null)
            {
                description = (string) regionsData[regionName]["description"];
                return true;
            }
            
            // see if the region is global region by concatenating the partition and "-global" to construct the global name 
            // for the partition
            else if (regionName.Equals(string.Concat((string)partition["partition"], "-global"), StringComparison.OrdinalIgnoreCase))
            {
                description = "Global";
                return true;
            }

            // no region key in the entry, but it matches the pattern in this partition.
            // we can try to construct an endpoint based on the heuristics described in endpoints.json
            else if (new Regex(regionPattern).Match(regionName).Success)
            {
                description = GetUnknownRegionDescription(regionName);
                return true;
            }

            else
            {
                description = GetUnknownRegionDescription(regionName);
                return false;
            }
        }

        public IRegionEndpoint GetRegionEndpoint(string regionName)
        {
            try
            {
                lock (_regionEndpointMapLock)
                {
                    IRegionEndpoint endpoint;
                    if (_regionEndpointMap.TryGetValue(regionName, out endpoint))
                    {
                        return endpoint;
                    }
                    else
                    {
                        JsonData partitions = _root["partitions"];
                        foreach (JsonData partition in partitions)
                        {
                            string description;
                            if (IsRegionInPartition(regionName, partition, out description))
                            {
                                endpoint = new RegionEndpointV3(regionName, description, partition, partition["services"]);
                                _regionEndpointMap.Add(regionName, endpoint);
                                return endpoint;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new AmazonClientException("Invalid endpoint.json format.");
            }

            return GetNonstandardRegionEndpoint(regionName);
        }

        /// <summary>
        /// This region name is non-standard.  Search the whole endpoints.json file to
        /// determine the partition this region is in.
        /// </summary>
        private IRegionEndpoint GetNonstandardRegionEndpoint(string regionName)
        {
            // default to "aws" partition
            JsonData partitionData = _root["partitions"][0];
            string regionDescription = GetUnknownRegionDescription(regionName);
            JsonData servicesData = _emptyDictionaryJsonData;
            
            foreach (JsonData partition in _root["partitions"])
            {
                JsonData partitionServices = partition["services"];
                foreach (string service in partitionServices.PropertyNames)
                {
                    if(partitionServices[service]!=null&& partitionServices[service].Count>0)
                    {
                        JsonData serviceData = partitionServices[service];
                        if (serviceData != null && serviceData["endpoints"][regionName] != null)
                        {
                            partitionData = partition;
                            servicesData = partitionServices;
                            break;
                        }
                    }
                }
            }

            return new RegionEndpointV3(regionName, regionDescription, partitionData, servicesData);
        }
        private static JsonData _emptyDictionaryJsonData = JsonMapper.ToObject("{}");
    }
}
