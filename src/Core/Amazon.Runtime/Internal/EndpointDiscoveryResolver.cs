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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Util;

namespace Amazon.Runtime.Internal
{    
    /// <summary>
    /// Class used to resolve endpoints using Endpoint Discovery.
    /// <para>
    /// This class is only intended for internal use inside the AWS client libraries.
    /// Callers shouldn't ever interact directly with objects of this class.
    /// </para>
    /// </summary>
    public abstract class EndpointDiscoveryResolverBase
    {
        private IClientConfig _config;
        private Logger _logger;
        private LruCache<string, IList<DiscoveryEndpointBase>> _cache;
        private object objectCacheLock = new object();

        protected EndpointDiscoveryResolverBase(IClientConfig config, Logger logger)
        {
            _config = config;
            _logger = logger;
            _cache = new LruCache<string, IList<DiscoveryEndpointBase>>(config.EndpointDiscoveryCacheLimit);
        }

        /// <summary>
        /// Method that performs endpoint discovery for the current operation
        /// </summary>
        /// <param name="context">Context information used in calculations for endpoint discovery</param>
        /// <param name="InvokeEndpointOperation">The operation to fetch endpoints from the server</param>
        /// <returns></returns>
        public virtual IEnumerable<DiscoveryEndpointBase> ResolveEndpoints(EndpointOperationContextBase context, Func<IList<DiscoveryEndpointBase>> InvokeEndpointOperation)
        {
            //Build the cacheKey
            var cacheKey = BuildEndpointDiscoveryCacheKey(context);

            //Check / cleanup the cache            
            var refreshCache = false;
            IEnumerable<DiscoveryEndpointBase> endpoints = ProcessEndpointCache(cacheKey, context.EvictCacheKey, context.EvictUri, out refreshCache);
            if (endpoints != null)
            {
                if (refreshCache)
                {
                    //Async fetch new endpoints because one or more of the endpoints in the cache have expired.
#if AWS_ASYNC_API
                    // Task only exists in framework 4.5 and up, and Standard.
                    System.Threading.Tasks.Task.Run(() =>
                    {
                        ProcessInvokeEndpointOperation(cacheKey, InvokeEndpointOperation, false);
                    });
#else
                    // ThreadPool only exists in 3.5 and below. These implementations do not have the Task library.
                    System.Threading.ThreadPool.QueueUserWorkItem((state) =>
                    {
                        ProcessInvokeEndpointOperation(cacheKey, InvokeEndpointOperation, false);
                    });
#endif
                }

                return endpoints;
            }

            if (context.EvictCacheKey)
            {
                return null;
            }

            //Determine if we are required to get an endpoint or if we can defer it to an async call
            if (context.EndpointDiscoveryData.Required)
            {
                //Must find an endpoint or error for this operation
                endpoints = ProcessInvokeEndpointOperation(cacheKey, InvokeEndpointOperation, true);
            }
            else if (_config.EndpointDiscoveryEnabled)
            {
                //Optionally find and endpoint for this supported operation async
#if AWS_ASYNC_API
                // Task only exists in framework 4.5 and up, and Standard.
                System.Threading.Tasks.Task.Run(() =>
                {
                    ProcessInvokeEndpointOperation(cacheKey, InvokeEndpointOperation, false);
                });
#else
                // ThreadPool only exists in 3.5 and below. These implementations do not have the Task library.
                System.Threading.ThreadPool.QueueUserWorkItem((state) =>
                {
                    ProcessInvokeEndpointOperation(cacheKey, InvokeEndpointOperation, false);
                });
#endif
                return null;
            }
            //else not required or endpoint discovery has been disabled so fall through to normal regional endpoint

            return endpoints;
        }

        private IEnumerable<DiscoveryEndpointBase> ProcessInvokeEndpointOperation(string cacheKey, Func<IList<DiscoveryEndpointBase>> InvokeEndpointOperation, bool endpointRequired)
        {
            IList<DiscoveryEndpointBase> endpoints = null;

            try
            {
                endpoints = InvokeEndpointOperation();
                if (endpoints != null && endpoints.Count > 0)
                {
                    _cache.AddOrUpdate(cacheKey, endpoints);
                }
                else
                {
                    endpoints = null;
                    if (!endpointRequired)
                    {
                        //Since it is not required that endpoint discovery is used for this operation, cache
                        //a null endpoint with an expiration time of 60 seconds so that requests are not
                        //excessively sent.
                        var cacheEndpoints = new List<DiscoveryEndpointBase>();
                        cacheEndpoints.Add(new DiscoveryEndpoint(null, 1));                        
                        _cache.AddOrUpdate(cacheKey, cacheEndpoints);
                    }
                    _logger.InfoFormat("The request to discover endpoints did not return any endpoints.");
                }
            }
            catch (Exception exception)
            {
                _logger.Error(exception, "An unhandled exception occurred while trying to discover endpoints.");
            }

            if (endpoints == null && endpointRequired)
            {
                throw new AmazonClientException("Failed to discover the endpoint for the request. Requests will not succeed until an endpoint can be retrieved or an endpoint is manually specified.");
            }

            return endpoints;
        }

        public virtual IList<DiscoveryEndpointBase> GetDiscoveryEndpointsFromCache(string cacheKey)
        {
            IList<DiscoveryEndpointBase> endpoints = null;
            if (!_cache.TryGetValue(cacheKey, out endpoints))
            {
                return null;
            }

            return endpoints;
        }

        /// <summary>
        /// Gets the number of cache keys in the cache
        /// </summary>
        public virtual int CacheCount
        {
            get
            {
                return _cache.Count;
            }
        }

        private IEnumerable<DiscoveryEndpointBase> ProcessEndpointCache(string cacheKey, bool evictCacheKey, Uri evictUri, out bool refreshCache)
        {
            refreshCache = false;
            IList<DiscoveryEndpointBase> endpoints = GetDiscoveryEndpointsFromCache(cacheKey);

            if (evictCacheKey && endpoints != null)
            {
                //Force eviction of the evictUri endpoint but keep the rest to use. This only happens if an 
                //Invalid Endpoint Exception was processed against the cached endpoint data.
                var testAddress = evictUri.ToString();
                lock (objectCacheLock)
                {
                    for (var i = 0; i < endpoints.Count; i++)
                    {
                        var endpoint = endpoints[i];
                        if (endpoint.Address != null && endpoint.Address.Equals(testAddress, StringComparison.OrdinalIgnoreCase))
                        {
                            endpoints.RemoveAt(i);
                            break;
                        }
                    }
                }

                if (endpoints.Count == 0)
                {
                    _cache.Evict(cacheKey);
                    return null;
                }                
            }

            //Check to see if we have cached endpoints            
            if (endpoints != null)
            {
                //If any endpoints have expired we need to async get new endpoints while continuing to use
                //the existing endpoints in the cache. They must not be evicted if they expired until new
                //endpoints are obtained.                    
                foreach (var endpoint in endpoints)
                {
                    if (endpoint.HasExpired())
                    {
                        refreshCache = true;
                        //Add 1 minute to the expiration so we don't retry again until 1 minute from now if 
                        //the refresh fails. If the refresh succeeds all the extended endpoints will be replaced
                        //before the extended 1 minute expiration.
                        endpoint.ExtendExpiration(1);
                    }
                }

                return endpoints;
            }

            return null;
        }

        private static string BuildEndpointDiscoveryCacheKey(EndpointOperationContextBase context)
        {
            var cacheKeyBuilder = new StringBuilder();
            cacheKeyBuilder.Append(context.CustomerCredentials);
            var identifiers = context.EndpointDiscoveryData.Identifiers;
            if (identifiers != null && identifiers.Count > 0)
            {
                cacheKeyBuilder.Append(string.Format(CultureInfo.InvariantCulture, ".{0}", context.OperationName));
                foreach (var identifier in identifiers)
                {
                    cacheKeyBuilder.Append(string.Format(CultureInfo.InvariantCulture, ".{0}", identifier.Value));
                }
            }

            return cacheKeyBuilder.ToString();
        }
    }

    /// <summary>
    /// Class used to resolve endpoints using Endpoint Discovery.
    /// <para>
    /// This class is only intended for internal use inside the AWS client libraries.
    /// Callers shouldn't ever interact directly with objects of this class.
    /// </para>
    /// </summary>
    public class EndpointDiscoveryResolver : EndpointDiscoveryResolverBase
    {
        public EndpointDiscoveryResolver(IClientConfig config, Logger logger) : base(config, logger)
        {
        }
    }
}
