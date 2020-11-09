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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Amazon.Internal;
using Amazon.Runtime.Internal.Util;

namespace Amazon.Util.Internal
{
    /// <summary>
    /// Finds region string in the endpoint string using predefined rules
    /// If predefined rules fail to match the region, regular expression strings provided in
    /// endpoints.json are used to find the region.
    /// If regular expressions also fail, then a default region is returned.
    /// </summary>
    public class RegionFinder
    {
        internal class EndpointSegment
        {
            public string Value { get; set; }
            public IRegionEndpoint RegionEndpoint { get; set; }
            public bool UseThisValue { get; set; }
            public List<EndpointSegment> Children { get; set; }
        }

        #region Constants

        private const string DefaultRegion = "us-east-1";
        private const string DefaultGovRegion = "us-gov-west-1";

        #endregion

        #region Members

        private readonly EndpointSegment _root;
        private readonly Logger _logger;
        private readonly Dictionary<string, IRegionEndpoint> _regionEndpoints;
        private readonly RegionEndpointProviderV3 _regionEndpointProviderV3;

        #endregion

        #region Constructors

        internal RegionFinder()
        {
            _regionEndpointProviderV3 = new RegionEndpointProviderV3();
            _regionEndpoints = BuildRegionEndpoints();
            _root = BuildRoot();
            _logger = Logger.GetLogger(typeof(RegionFinder));
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Finds the region in the provided endpoint parsing from right to left
        /// Try to find exact match of the region in endpoints.json
        /// If there doesn't exist an exact match, find a fuzzy match
        /// Else return default region
        /// </summary>
        /// <param name="endpoint">Endpoint string</param>
        /// <returns>First successfully parsed region from right to left in the given endpoint or default region</returns>
        public IRegionEndpoint FindRegion(string endpoint)
        {
            if (string.IsNullOrEmpty(endpoint))
            {
                return _root.RegionEndpoint;
            }

            endpoint = GetAuthority(endpoint.ToLower());

            var exactRegion = FindExactRegion(endpoint);
            if (exactRegion != null && exactRegion.UseThisValue)
            {
                return exactRegion.RegionEndpoint;
            }

            _logger.InfoFormat($"Unable to find exact matched region in endpoint {endpoint}");

            var fuzzyRegion = FindFuzzyRegion(endpoint);
            if (fuzzyRegion != null)
            {
                _logger.InfoFormat($"{fuzzyRegion.RegionName} fuzzy region found in endpoint {endpoint}");
                return fuzzyRegion;
            }

            _logger.InfoFormat($"Unable to find fuzzy matched region in endpoint {endpoint}");

            // Return the default region
            return _root.RegionEndpoint;
        }

        /// <summary>
        /// Returns the Domain Name System host name
        /// </summary>
        /// <param name="url">URL string</param>
        /// <returns>A String containing the authority component of the URL</returns>
        public static string GetAuthority(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            var schemeEndIndex = url.IndexOf("://", StringComparison.Ordinal);
            if (schemeEndIndex != -1)
            {
                url = url.Substring(schemeEndIndex + 3);
            }

            var hostEndIndex = url.IndexOf("/", StringComparison.Ordinal);
            if (hostEndIndex != -1)
            {
                url = url.Substring(0, hostEndIndex);
            }

            return url;
        }

        /// <summary>
        /// Find region in the endpoint using endpoints.json region regexs
        /// If there doesn't exist a match, return null
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns>First matched region from right to left in the given endpoint or null</returns>
        public IRegionEndpoint FindFuzzyRegion(string endpoint)
        {
            foreach (var regionRegex in _regionEndpointProviderV3.AllRegionRegex)
            {
                // A typical region regex looks like "^(us|eu|ap|sa|ca|me|af)\\-\\w+\\-\\d+$"
                // Remove the start (^) and end ($) keyword to allow regex matching without defined start and end pattern
                var trimmedRegionRegex = regionRegex.Trim('^', '$');

                var match = Regex.Match(endpoint, trimmedRegionRegex, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.RightToLeft);
                if (match.Success)
                {
                    return new RegionEndpointProviderV2.RegionEndpoint(match.Value, "Unknown");
                }
            }

            return null;
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Find endpoint segment in the endpoint parsing right to left
        /// If there exists an exception such as us-gov, return exception value
        /// Else return null
        /// </summary>
        /// <param name="endpoint">Endpoint string</param>
        /// <returns>First parsed region from right to left in the given endpoint or null</returns>
        internal EndpointSegment FindExactRegion(string endpoint)
        {
            var segments = endpoint.Split('.');
            return FindExactRegion(segments, segments.Length - 1, _root);
        }

        #endregion

        #region Private methods

        private Dictionary<string, IRegionEndpoint> BuildRegionEndpoints()
        {
            var allRegionEndpoints = new Dictionary<string, IRegionEndpoint>();
            foreach (var regionEndpoint in _regionEndpointProviderV3.AllRegionEndpoints)
            {
                allRegionEndpoints[regionEndpoint.RegionName] = regionEndpoint;
            }

            return allRegionEndpoints;
        }

        /// <summary>
        /// Builds an exception tree root that is used to handle the exception cases for an endpoint to determine the region.
        /// New exceptions must be added as a child to the root.
        /// If there exists a sub-exception that depends on the parent exception, it must be added as a child to the parent node
        /// For example, us-gov followed by s-accelerate from right to left, then us-gov must have s3-accelerate as a Child
        /// </summary>
        /// <returns>Root of exception tree</returns>
        private EndpointSegment BuildRoot()
        {
            return new EndpointSegment()
            {
                Children = new List<EndpointSegment>()
                {
                    new EndpointSegment()
                    {
                        Value = "s3-accelerate",
                        RegionEndpoint = null,
                        UseThisValue = true,
                    },
                    new EndpointSegment()
                    {
                        Value = "us-gov",
                        RegionEndpoint = _regionEndpoints[DefaultGovRegion],
                        UseThisValue = true
                    }
                },
                RegionEndpoint = _regionEndpoints[DefaultRegion]
            };
        }

        private EndpointSegment FindExactRegion(IList<string> segments, int segmentIndex, EndpointSegment currentEndpointSegment)
        {
            // Return null if there doesn't exist a matching region
            if (segmentIndex < 0)
            {
                return null;
            }

            var segment = segments[segmentIndex];

            // Move down in the tree, if there exists a child node
            var nextEndpointSegment = currentEndpointSegment.Children.FirstOrDefault(endpointSegment => endpointSegment.Value.Equals(segment));
            if (nextEndpointSegment != null)
            {
                currentEndpointSegment = nextEndpointSegment;
            }

            // Return the value of node if exception is configured with return value
            if (currentEndpointSegment.UseThisValue)
            {
                return currentEndpointSegment;
            }

            // Check for the region
            var valueToCheck = string.Empty;
            var dashedSegments = segment.Split('-');
            for (var dashedSegmentIndex = dashedSegments.Length - 1; dashedSegmentIndex >= 0; dashedSegmentIndex--)
            {
                valueToCheck = string.IsNullOrEmpty(valueToCheck) ? dashedSegments[dashedSegmentIndex] : $"{dashedSegments[dashedSegmentIndex]}-{valueToCheck}";
                if (_regionEndpoints.ContainsKey(valueToCheck))
                {
                    return new EndpointSegment()
                    {
                        RegionEndpoint = _regionEndpoints[valueToCheck],
                        UseThisValue = true
                    };
                }
            }

            return FindExactRegion(segments, segmentIndex - 1, currentEndpointSegment);
        }

        #endregion

        private static readonly RegionFinder _instance = new RegionFinder();

        /// <summary>
        /// Gets the singleton.
        /// </summary>
        public static RegionFinder Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}