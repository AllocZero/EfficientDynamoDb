// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace EfficientDynamoDb.Internal.Reader
{
    public class JsonReaderDictionary<TValue> where TValue : class
    {
        private struct Entry {
            public ulong HashCode;    // Lower 31 bits of hash code, -1 if unused
            public int Next;        // Index of next entry, -1 if last
            public byte[] Key;
            public TValue Value;    // Value of entry
        }
        
        private int[]? _buckets;
        private Entry[]? _entries;
        private int _count;
        private ulong _bucketsLength;

        public JsonReaderDictionary()
        {
            var size = KeysCache.GetPrime(0);
            
            _buckets = new int[size];
            _buckets.AsSpan().Fill(-1);
            _bucketsLength = (ulong) size;
            _entries = new Entry[size];
            _count = 0;
        }

        public void Add(string key, TValue value)
        {
            var bytesLength = Encoding.UTF8.GetByteCount(key);
            Span<byte> buffer = stackalloc byte[bytesLength];
            Encoding.UTF8.GetBytes(key, buffer);

            var hashCode = KeysCache.GetKey(buffer);
            var targetBucket = hashCode % _bucketsLength;

            if (_count == _entries!.Length)
            {
                var newSize = KeysCache.GetPrime(_count * 2);
                Resize(newSize);
                targetBucket = hashCode % _bucketsLength;
            }

            var index = _count++;

            _entries[index].HashCode = hashCode;
            _entries[index].Next = _buckets![targetBucket];
            _entries[index].Key = Encoding.UTF8.GetBytes(key);
            _entries[index].Value = value;
            _buckets[targetBucket] = index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(ref Utf8JsonReader reader, [NotNullWhen(true)] out TValue? value)
        {
            var hashCode = KeysCache.GetKey(reader.ValueSpan);
            var targetBucket = hashCode % _bucketsLength;
            
            for (var i = _buckets![targetBucket]; i >= 0; i = _entries[i].Next)
            {
                if (!(_entries![i].HashCode == hashCode && (reader.ValueSpan.Length <= 7 || reader.ValueTextEquals(_entries[i].Key))))
                    continue;

                value = _entries[i].Value;
                return true;
            }

            value = null;
            return false;
        }

        private void Resize(int newSize)
        {
            var oldBuckets = _buckets;
            var oldEntries = _entries;
            
            _buckets = new int[newSize];
            _buckets.AsSpan().Fill(-1);
            _entries = new Entry[newSize];
            _bucketsLength = (ulong) newSize;

            oldEntries.AsSpan(0, _count).CopyTo(_entries);
           
            for (var i = 0; i < _count; i++)
            {
                if (_entries[i].HashCode == 0) 
                    continue;
                
                var bucket = _entries[i].HashCode % _bucketsLength;
                _entries[i].Next = _buckets[bucket];
                _buckets[bucket] = i;
            }
        }
    }
}