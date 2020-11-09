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

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// a size-limited cache of key value pairs
    ///
    /// Once the maximum size has been reached, the least recently
    /// used pairs will be evicted to make room for any new items.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class LruCache<TKey, TValue>
        where TKey : class
        where TValue : class
    {
        private readonly object cacheLock = new object();
        private Dictionary<TKey, LruListItem<TKey, TValue>> cache;
        private LruList<TKey, TValue> lruList;

        /// <summary>
        /// the maximum number of entries this LruCache will hold
        /// before items begin getting evicted
        /// </summary>
        public int MaxEntries { get; private set; }

        /// <summary>
        /// the number of items in the cache
        /// </summary>
        public int Count
        {
            get
            {
                lock (cacheLock)
                {
                    return cache.Count;
                }
            }
        }

        /// <summary>
        /// Construct a new LruCache.
        /// </summary>
        /// <param name="maxEntries">maximum number of entries before items are evicted</param>
        public LruCache(int maxEntries)
        {
            if (maxEntries < 1)
            {
                throw new ArgumentException("maxEntries must be greater than zero.");
            }

            MaxEntries = maxEntries;
            cache = new Dictionary<TKey, LruListItem<TKey, TValue>>();
            lruList = new LruList<TKey, TValue>();
        }

        /// <summary>
        /// Add the key/value pair to the cache, or update
        /// the value if the key already exists.
        ///
        /// If the cache is full, evicts the least recently used item.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddOrUpdate(TKey key, TValue value)
        {
            lock (cacheLock)
            {
                LruListItem<TKey, TValue> existingLruListItem;
                if (cache.TryGetValue(key, out existingLruListItem))
                {
                    // update
                    existingLruListItem.Value = value;
                    lruList.Touch(existingLruListItem);
                }
                else
                {
                    // add
                    var newLruListItem = new LruListItem<TKey, TValue>(key, value);
                    while (cache.Count >= MaxEntries)
                    {
                        cache.Remove(lruList.EvictOldest());
                    }
                    lruList.Add(newLruListItem);
                    cache.Add(key, newLruListItem);
                }
            }
        }

        /// <summary>
        /// Evicts a specific key out of the cache if it exists
        /// </summary>
        /// <param name="key">the key to evict from the cache</param>        
        public void Evict(TKey key)
        {
            lock (cacheLock)
            {
                LruListItem<TKey, TValue> existingLruListItem;
                if (cache.TryGetValue(key, out existingLruListItem))
                {
                    lruList.Remove(existingLruListItem);
                    cache.Remove(key);
                }
            }
        }

        /// <summary>
        /// Try to get the value associated with the key.
        /// </summary>
        /// <param name="key">the key to look up</param>
        /// <param name="value">the value, if the key exists</param>
        /// <returns>true if there is a value associated with the key, or false if no value is associated with the key</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            LruListItem<TKey, TValue> existingListItem;
            lock (cacheLock)
            {
                if (cache.TryGetValue(key, out existingListItem))
                {
                    lruList.Touch(existingListItem);
                    value = existingListItem.Value;
                    return true;
                }
                else
                {
                    value = null;
                    return false;
                }
            }
        }

        /// <summary>
        /// Try to get the value associated with the key, if it doesn't exist, use the provided factory method to 
        /// create a new value and tries adding it to the cache.
        /// </summary>
        /// <param name="key">the key to look up</param>
        /// <param name="factory">the factory method used in case the key is not present in the cache</param>
        /// <returns>the looked up value or the value created by the factory</returns>
        public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory)
        {
            TValue value;
            if (TryGetValue(key, out value))
            {
                return value;
            }
            value = factory(key);
            AddOrUpdate(key, value);
            return value;
        }

        /// <summary>
        /// Clear the LruCache of all entries.
        /// </summary>
        public void Clear()
        {
            lock (cacheLock)
            {
                cache.Clear();
                lruList.Clear();
            }
        }
    }

    /// <summary>
    /// Helper class to support LruCache.
    /// Does not implement the error checking and synchronization that
    /// would be necessary for it to stand alone.
    /// </summary>
    public class LruList<TKey, TValue>
    {
        public LruListItem<TKey, TValue> Head { get; private set; }
        public LruListItem<TKey, TValue> Tail { get; private set; }

        public void Add(LruListItem<TKey, TValue> item)
        {
            if (Head == null)
            {
                Head = item;
                Tail = item;
                item.Previous = null;
                item.Next = null;
            }
            else
            {
                Head.Previous = item;
                item.Next = Head;
                item.Previous = null;
                Head = item;
            }
        }

        public void Remove(LruListItem<TKey, TValue> item)
        {
            if (Head == item || Tail == item)
            {
                if (Head == item)
                {
                    Head = item.Next;
                    if (Head != null)
                    {
                        Head.Previous = null;
                    }
                }

                if (Tail == item)
                {
                    Tail = item.Previous;
                    if (Tail != null)
                    {
                        Tail.Next = null;
                    }
                }
            }
            else
            {
                item.Previous.Next = item.Next;
                item.Next.Previous = item.Previous;
            }

            item.Previous = null;
            item.Next = null;
        }

        public void Touch(LruListItem<TKey, TValue> item)
        {
            Remove(item);
            Add(item);
        }

        public TKey EvictOldest()
        {
            TKey key = default(TKey);
            if (Tail != null)
            {
                key = Tail.Key;
                Remove(Tail);
            }
            return key;
        }

        internal void Clear()
        {
            Head = null;
            Tail = null;
        }
    }

    /// <summary>
    /// Item to be stored in the LruList
    /// linked list structure.
    /// </summary>
    public class LruListItem<TKey, TValue>
    {
        public TValue Value { get; set; }
        public TKey Key { get; private set; }
        public LruListItem<TKey, TValue> Next { get; set; }
        public LruListItem<TKey, TValue> Previous { get; set; }

        public LruListItem(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
