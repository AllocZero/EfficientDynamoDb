// ==++==
// 
//   Copyright (c) Microsoft Corporation.  All rights reserved.
// 
// ==--==

using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using EfficientDynamoDb.Internal.System;

namespace EfficientDynamoDb.Internal.Reader
{
    internal struct KeysCache : IDisposable
    {
        private readonly int _maxCacheLength;
        
        private struct Entry {
            public ulong HashCode;    // Lower 31 bits of hash code, -1 if unused
            public int Next;        // Index of next entry, -1 if last
            public string Value;    // Value of entry
        }
        
        private int[]? _buckets;
        private Entry[]? _entries;
        private int _count;
        private ulong _bucketsLength;

        public readonly bool IsInitialized;

        public KeysCache(int capacity, int maxCacheLength)
        {
            _maxCacheLength = maxCacheLength;
            _buckets = ArrayPool<int>.Shared.Rent(capacity);
            _buckets.AsSpan().Fill(-1);
            _bucketsLength = (ulong) capacity;
            _entries = ArrayPool<Entry>.Shared.Rent(capacity);
            _count = 0;
            IsInitialized = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetOrAdd(ref Utf8JsonReader reader, out string? value)
        {
            var hashCode = GetKey(reader.ValueSpan);
            var targetBucket = hashCode % _bucketsLength;

            for (var i = _buckets![targetBucket]; i >= 0; i = _entries[i].Next)
            {
                if (!(_entries![i].HashCode == hashCode && (reader.ValueSpan.Length <= 7 || reader.ValueTextEquals(_entries[i].Value))))
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
                targetBucket = hashCode % _bucketsLength;
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
        
        /// <summary>
        /// Get a key from the property name.
        /// The key consists of the first 7 bytes of the property name and then the length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong GetKey(ReadOnlySpan<byte> name)
        {
            ulong key;

            ref byte reference = ref MemoryMarshal.GetReference(name);
            int length = name.Length;

            if (length > 7)
            {
                key = Unsafe.ReadUnaligned<ulong>(ref reference) & 0x00ffffffffffffffL;
                key |= (ulong)Math.Min(length, 0xff) << 56;
            }
            else
            {
                key =
                    length > 5 ? Unsafe.ReadUnaligned<uint>(ref reference) | (ulong)Unsafe.ReadUnaligned<ushort>(ref Unsafe.Add(ref reference, 4)) << 32 :
                    length > 3 ? Unsafe.ReadUnaligned<uint>(ref reference) :
                    length > 1 ? Unsafe.ReadUnaligned<ushort>(ref reference) : 0UL;
                key |= (ulong)length << 56;

                if ((length & 1) != 0)
                {
                    var offset = length - 1;
                    key |= (ulong)Unsafe.Add(ref reference, offset) << (offset * 8);
                }
            }

            // Verify key contains the embedded bytes as expected.
            const int BitsInByte = 8;
            Debug.Assert(
                // Verify embedded property name.
                (name.Length < 1 || name[0] == ((key & ((ulong)0xFF << BitsInByte * 0)) >> BitsInByte * 0)) &&
                (name.Length < 2 || name[1] == ((key & ((ulong)0xFF << BitsInByte * 1)) >> BitsInByte * 1)) &&
                (name.Length < 3 || name[2] == ((key & ((ulong)0xFF << BitsInByte * 2)) >> BitsInByte * 2)) &&
                (name.Length < 4 || name[3] == ((key & ((ulong)0xFF << BitsInByte * 3)) >> BitsInByte * 3)) &&
                (name.Length < 5 || name[4] == ((key & ((ulong)0xFF << BitsInByte * 4)) >> BitsInByte * 4)) &&
                (name.Length < 6 || name[5] == ((key & ((ulong)0xFF << BitsInByte * 5)) >> BitsInByte * 5)) &&
                (name.Length < 7 || name[6] == ((key & ((ulong)0xFF << BitsInByte * 6)) >> BitsInByte * 6)) &&
                // Verify embedded length.
                (name.Length >= 0xFF || (key & ((ulong)0xFF << BitsInByte * 7)) >> BitsInByte * 7 == (ulong)name.Length) &&
                (name.Length < 0xFF || (key & ((ulong)0xFF << BitsInByte * 7)) >> BitsInByte * 7 == 0xFF));

            return key;
        }
    }
}