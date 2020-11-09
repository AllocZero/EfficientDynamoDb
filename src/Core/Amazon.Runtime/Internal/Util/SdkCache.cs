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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Amazon.Util;
using Amazon.Runtime.Internal.Util;
using ThirdParty.Json.LitJson;
using System.Threading;

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// Interface for a non-generic cache.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Clears the entire cache.
        /// </summary>
        void Clear();

        /// <summary>
        /// Maximum time to keep an item around after its last use.
        /// </summary>
        TimeSpan MaximumItemLifespan { get; set; }

        /// <summary>
        /// How often should the cache be cleared of old items.
        /// </summary>
        TimeSpan CacheClearPeriod { get; set; }

        /// <summary>
        /// The number of items in the cache.
        /// </summary>
        int ItemCount { get; }
    }

    /// <summary>
    /// Interface for a generic cache.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public interface ICache<TKey, TValue> : ICache
    {
        /// <summary>
        /// Retrieves a value out of the cache or from the source.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        TValue GetValue(TKey key, Func<TKey, TValue> creator);

        /// <summary>
        /// Retrieves a value out of the cache or from the source.
        /// If the item was in the cache, isStaleItem is set to true;
        /// otherwise, if the item comes from the source, isStaleItem is false.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="creator"></param>
        /// <param name="isStaleItem"></param>
        /// <returns></returns>
        TValue GetValue(TKey key, Func<TKey, TValue> creator, out bool isStaleItem);

        /// <summary>
        /// Clears a specific value from the cache if it's there.
        /// </summary>
        /// <param name="key"></param>
        void Clear(TKey key);

        /// <summary>
        /// Returns the keys for all items in the cache.
        /// </summary>
        /// <returns></returns>
        List<TKey> Keys { get; }

        /// <summary>
        /// Executes specified operation, catches exception, clears the cache for
        /// the given key, retries the operation.
        /// </summary>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="key"></param>
        /// <param name="operation"></param>
        /// <param name="onError"></param>
        /// <param name="shouldRetryForException"></param>
        /// <returns></returns>
        TOut UseCache<TOut>(TKey key, Func<TOut> operation, Action onError, Predicate<Exception> shouldRetryForException);
    }
    
    /// <summary>
    /// SDK-wide cache.
    /// Provides access to caches specific to a particular set of credentials
    /// and target region.
    /// </summary>
    public static class SdkCache
    {
        private static object cacheLock = new object();
        private static Cache<CacheKey, ICache> cache = new Cache<CacheKey, ICache>();

        /// <summary>
        /// Clear all caches
        /// </summary>
        public static void Clear()
        {
            cache.Clear();
        }

        /// <summary>
        /// Clear all caches of a particular type
        /// </summary>
        /// <param name="cacheType"></param>
        public static void Clear(object cacheType)
        {
            lock (cacheLock)
            {
                var keys = cache.Keys;
                foreach (CacheKey key in keys)
                {
                    if (AWSSDKUtils.AreEqual(key.CacheType, cacheType))
                    {
                        var value = cache.GetValue(key, null);
                        value.Clear();
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve a cache of a specific type for a client object.
        /// The client object can be null in cases where a cache does
        /// not correspond to a specific AWS account or target region.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="client"></param>
        /// <param name="cacheIdentifier"></param>
        /// <param name="keyComparer"></param>
        /// <returns></returns>
        public static ICache<TKey, TValue> GetCache<TKey, TValue>(
            object client,
            object cacheIdentifier,
            IEqualityComparer<TKey> keyComparer)
        {
            return GetCache<TKey, TValue>(client as AmazonServiceClient, cacheIdentifier, keyComparer);
        }

        /// <summary>
        /// Retrieve a cache of a specific type for a client object.
        /// The client object can be null in cases where a cache does
        /// not correspond to a specific AWS account or target region.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="client"></param>
        /// <param name="cacheIdentifier"></param>
        /// <param name="keyComparer"></param>
        /// <returns></returns>
        public static ICache<TKey, TValue> GetCache<TKey, TValue>(
            AmazonServiceClient client,
            object cacheIdentifier,
            IEqualityComparer<TKey> keyComparer)
        {
            // If client is null, create an empty key to use.
            // This supports mock frameworks (where the service is mocked up,
            // but there is no client to get credentials/region from) and
            // caches that do not depend on a client (such as a cache
            // for client-side reflection data).
            CacheKey key;
            if (client == null)
                key = CacheKey.Create(cacheIdentifier);
            else
                key = CacheKey.Create(client, cacheIdentifier);

            ICache value = null;
            lock (cacheLock)
            {
                value = cache.GetValue(key, k => new Cache<TKey, TValue>(keyComparer));
            }

            var typedValue = value as Cache<TKey, TValue>;
            if (value != null && typedValue == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "Unable to cast cache of type {0} as cache of type {1}",
                    value.GetType().FullName,
                    typeof(Cache<TKey, TValue>).FullName));
            }

            return typedValue;
        }

        // Composite cache key consisting of credentials, region, service url, cache type
        internal class CacheKey
        {
            public ImmutableCredentials ImmutableCredentials { get; private set; }
            public RegionEndpoint RegionEndpoint { get; private set; }
            public string ServiceUrl { get; private set; }
            public object CacheType { get; private set; }

            private CacheKey()
            {
                ImmutableCredentials = null;
                RegionEndpoint = null;
                ServiceUrl = null;
                CacheType = null;
            }

            public static CacheKey Create(AmazonServiceClient client, object cacheType)
            {
                if (client == null) throw new ArgumentNullException("client");

                var key = new CacheKey();

                var credentials = client.Credentials;
                key.ImmutableCredentials = credentials == null ?
                    null : credentials.GetCredentials();
                key.RegionEndpoint = client.Config.RegionEndpoint;
                key.ServiceUrl = client.Config.ServiceURL;
                key.CacheType = cacheType;

                return key;
            }
            public static CacheKey Create(object cacheType)
            {
                var key = new CacheKey();
                key.CacheType = cacheType;
                return key;
            }

            #region Public overrides

            public override int GetHashCode()
            {
                return Hashing.Hash(
                    ImmutableCredentials,
                    RegionEndpoint,
                    ServiceUrl,
                    CacheType);
            }
            public override bool Equals(object obj)
            {
                if (object.ReferenceEquals(this, obj))
                    return true;

                CacheKey ck = obj as CacheKey;
                if (ck == null)
                    return false;

                var allEqual = AWSSDKUtils.AreEqual(
                    new object[] { this.ImmutableCredentials, this.RegionEndpoint, this.ServiceUrl, this.CacheType },
                    new object[] { ck.ImmutableCredentials, ck.RegionEndpoint, ck.ServiceUrl, ck.CacheType });

                return allEqual;
            }

            #endregion
        }
    }

    // Implementation of generic ICache<TKey,TValue> interface
    internal class Cache<TKey, TValue> : ICache<TKey, TValue>
    {
        #region Private members

        private Dictionary<TKey, CacheItem<TValue>> Contents;
        private readonly object CacheLock = new object();

        #endregion

        #region Constructor

        public Cache(IEqualityComparer<TKey> keyComparer = null)
        {
            Contents = new Dictionary<TKey, CacheItem<TValue>>(keyComparer);

            MaximumItemLifespan = DefaultMaximumItemLifespan;
            CacheClearPeriod = DefaultCacheClearPeriod;
        }

        #endregion

        #region Public members

        public static TimeSpan DefaultMaximumItemLifespan = TimeSpan.FromHours(6);
        public static TimeSpan DefaultCacheClearPeriod = TimeSpan.FromHours(1);
        public DateTime LastCacheClean { get; private set; }

        #endregion

        #region ICache implementation

        public TValue GetValue(TKey key, Func<TKey, TValue> creator)
        {
            bool isStaleItem;
            return GetValueHelper(key, out isStaleItem, creator);
        }
        public TValue GetValue(TKey key, Func<TKey, TValue> creator, out bool isStaleItem)
        {
            return GetValueHelper(key, out isStaleItem, creator);
        }
        public void Clear(TKey key)
        {
            lock (CacheLock)
            {
                Contents.Remove(key);
            }
        }
        public void Clear()
        {
            lock (CacheLock)
            {
                Contents.Clear();
                LastCacheClean = GetCorrectedLocalTime();
            }
        }
        public List<TKey> Keys
        {
            get
            {
                lock (CacheLock)
                {
                    return Contents.Keys.ToList();
                }
            }
        }

        private TimeSpan maximumItemLifespan;
        public TimeSpan MaximumItemLifespan
        {
            get { return maximumItemLifespan; }
            set
            {
                if (value < TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException("value");
                maximumItemLifespan = value;
            }
        }

        private TimeSpan cacheClearPeriod;
        public TimeSpan CacheClearPeriod
        {
            get { return cacheClearPeriod; }
            set
            {
                if (value < TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException("value");
                cacheClearPeriod = value;
            }
        }

        public int ItemCount
        {
            get
            {
                lock (CacheLock)
                {
                    return Contents.Count;
                }
            }
        }

        public TOut UseCache<TOut>(TKey key, Func<TOut> operation,
            Action onError, Predicate<Exception> shouldRetryForException)
        {
            TOut output = default(TOut);

            try
            {
                output = operation();
            }
            catch(Exception e)
            {
                // if predicate is specified, check whether to retry on exception
                // otherwise, retry
                var shouldRetry = shouldRetryForException == null || shouldRetryForException(e);
                if (shouldRetry)
                {
                    // clear existing value
                    Clear(key);

                    // allow calling code to cleanup
                    if (onError != null)
                        onError();

                    // retry operation
                    output = operation();
                }
                else
                    throw;
            }

            return output;
        }

        #endregion

        #region Private methods and classes

        private TValue GetValueHelper(TKey key, out bool isStaleItem, Func<TKey, TValue> creator = null)
        {
            isStaleItem = true;
            CacheItem<TValue> item = null;
            if (AWSConfigs.UseSdkCache)
            {
                lock (CacheLock)
                {
                    if (!Contents.TryGetValue(key, out item) || !IsValidItem(item))
                    {
                        if (creator == null)
                            throw new InvalidOperationException("Unable to calculate value for key " + key);

                        var value = creator(key);
                        isStaleItem = false;
                        item = new CacheItem<TValue>(value);
                        Contents[key] = item;

                        RemoveOldItems_Locked();
                    }
                }
            }
            else
            {
                if (creator == null)
                    throw new InvalidOperationException("Unable to calculate value for key " + key);

                var value = creator(key);
                item = new CacheItem<TValue>(value);
                isStaleItem = false;
            }

            if (item == null)
                throw new InvalidOperationException("Unable to find value for key " + key);

            return item.Value;
        }

        private bool IsValidItem(CacheItem<TValue> item)
        {
            if (item == null)
                return false;
            var cutoff = GetCorrectedLocalTime() - this.MaximumItemLifespan;
            if (item.LastUseTime < cutoff)
                return false;

            return true;
        }
        private void RemoveOldItems_Locked()
        {
            if (LastCacheClean + CacheClearPeriod > AWSConfigs.utcNowSource().ToLocalTime())
                return;

            // Remove all items that were not accessed since the cutoff.
            // Using a cutoff is more optimal than item.Age, as we only need
            // to do DateTime calculation once, not for each item.
            var cutoff = GetCorrectedLocalTime() - MaximumItemLifespan;

            var keysToRemove = new List<TKey>();
            foreach (var kvp in Contents)
            {
                var key = kvp.Key;
                var item = kvp.Value;

                if (item == null || item.LastUseTime < cutoff)
                    keysToRemove.Add(key);
            }

            foreach (var key in keysToRemove)
                Contents.Remove(key);

            LastCacheClean = GetCorrectedLocalTime();
        }

        private class CacheItem<T>
        {
            private T _value;

            public T Value
            {
                get
                {
                    LastUseTime = GetCorrectedLocalTime();
                    return _value;
                }
                private set
                {
                    _value = value;
                }
            }
            public DateTime LastUseTime { get; private set; }

            public CacheItem(T value)
            {
                Value = value;
                LastUseTime = GetCorrectedLocalTime();
            }
        }

        private static DateTime GetCorrectedLocalTime()
        {
#pragma warning disable CS0612 // Type or member is obsolete
            return AWSSDKUtils.CorrectedUtcNow.ToLocalTime();
#pragma warning restore CS0612 // Type or member is obsolete
        }

        #endregion
    }
}
