using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.System;

namespace EfficientDynamoDb.Internal.Reader
{
    internal struct KeysCache : IDisposable
    {
        private readonly int _maxCacheLength;
        
        private struct Entry {
            public int HashCode;    // Lower 31 bits of hash code, -1 if unused
            public int Next;        // Index of next entry, -1 if last
            public string Value;    // Value of entry
        }
        
        private int[]? _buckets;
        private Entry[]? _entries;
        private int _count;

        public readonly bool IsInitialized;

        public KeysCache(int capacity, int maxCacheLength)
        {
            _maxCacheLength = maxCacheLength;
            _buckets = ArrayPool<int>.Shared.Rent(capacity);
            _buckets.AsSpan().Fill(-1);
            _entries = ArrayPool<Entry>.Shared.Rent(capacity);
            _count = 0;
            IsInitialized = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetOrAdd(ref Utf8JsonReader reader, out string? value)
        {
            var hashCode = reader.ValueSpan.ComputeHash32() & 0x7FFFFFFF;
            var targetBucket = hashCode % _buckets!.Length;

            for (var i = _buckets[targetBucket]; i >= 0; i = _entries[i].Next)
            {
                if (_entries![i].HashCode != hashCode || !reader.ValueTextEquals(_entries[i].Value))
                    continue;

                value = _entries[i].Value;
                return true;
            }

            if (_count == _entries!.Length)
            {
                var newSize = _count * 2;
                if (newSize > _maxCacheLength)
                {
                    value = null;
                    return false;
                }
                
                Resize(newSize);
                targetBucket = hashCode % _buckets.Length;
            }

            var index = _count++;

            _entries[index].HashCode = hashCode;
            _entries[index].Next = _buckets[targetBucket];
            _entries[index].Value = value = reader.GetString()!;
            _buckets[targetBucket] = index;

            return true;
        }

        private void Resize(int newSize)
        {
            var oldBuckets = _buckets;
            var oldEntries = _entries;
            
            _buckets = ArrayPool<int>.Shared.Rent(newSize);
            _buckets.AsSpan().Fill(-1);
            _entries = ArrayPool<Entry>.Shared.Rent(newSize);

            oldEntries.AsSpan(0, _count).CopyTo(_entries);
           
            for (var i = 0; i < _count; i++)
            {
                if (_entries[i].HashCode < 0) 
                    continue;
                
                var bucket = _entries[i].HashCode % newSize;
                _entries[i].Next = _buckets[bucket];
                _buckets[bucket] = i;
            }
            
            oldBuckets.AsSpan().Clear();
            oldEntries.AsSpan().Clear();
            ArrayPool<int>.Shared.Return(oldBuckets);
            ArrayPool<Entry>.Shared.Return(oldEntries);
        }

        public void Dispose()
        {
            _buckets.AsSpan().Clear();
            _entries.AsSpan().Clear();
            ArrayPool<int>.Shared.Return(_buckets);
            ArrayPool<Entry>.Shared.Return(_entries);

            _buckets = null;
            _entries = null;
        }
    }
}