// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EfficientDynamoDb.Internal.Core
{
    internal sealed class PooledByteBufferWriter : IBufferWriter<byte>, IDisposable
    {
        private const int DefaultBufferSize = 16 * 1024;

        private readonly Stream? _stream;
        private const float FlushThreshold = .9f;
        
        private byte[]? _rentedBuffer;
        private int _index;
        private int _flushThreshold;

        private const int MinimumBufferSize = 256;

        public PooledByteBufferWriter()
        {
            _rentedBuffer = ArrayPool<byte>.Shared.Rent(DefaultBufferSize);
            _index = 0;
            _flushThreshold = CalculateFlushThreshold();
        }

        public PooledByteBufferWriter(Stream steam) : this(steam, DefaultBufferSize)
        {
        }

        public PooledByteBufferWriter(Stream stream, int initialCapacity)
        {
            Debug.Assert(initialCapacity > 0);
            _stream = stream;

            _rentedBuffer = ArrayPool<byte>.Shared.Rent(initialCapacity);
            _index = 0;
            _flushThreshold = CalculateFlushThreshold();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ShouldFlush(Utf8JsonWriter writer)
        {
            // Take into account both pending bytes from the writer as well as current size of the buffer
            // In some cases buffer and writer can go out of sync - meaning that writer has pending bytes and at the same time _index is > 0 
            // It happens when we on purpose flush the writer without writing to the stream, mainly when we want to write raw bytes json directly into the buffer
            return _stream != null && (writer.BytesPending + _index) > _flushThreshold;
        }

        public ReadOnlyMemory<byte> WrittenMemory
        {
            get
            {
                Debug.Assert(_rentedBuffer != null);
                Debug.Assert(_index <= _rentedBuffer.Length);
                return _rentedBuffer.AsMemory(0, _index);
            }
        }

        public int WrittenCount
        {
            get
            {
                Debug.Assert(_rentedBuffer != null);
                return _index;
            }
        }

        public int Capacity
        {
            get
            {
                Debug.Assert(_rentedBuffer != null);
                return _rentedBuffer.Length;
            }
        }

        public int FreeCapacity
        {
            get
            {
                Debug.Assert(_rentedBuffer != null);
                return _rentedBuffer.Length - _index;
            }
        }

        public void Clear()
        {
            ClearHelper();
        }

        private void ClearHelper()
        {
            Debug.Assert(_rentedBuffer != null);
            Debug.Assert(_index <= _rentedBuffer.Length);

            _rentedBuffer.AsSpan(0, _index).Clear();
            _index = 0;
        }

        // Returns the rented buffer back to the pool
        public void Dispose()
        {
            if (_rentedBuffer == null)
            {
                return;
            }

            ClearHelper();
            ArrayPool<byte>.Shared.Return(_rentedBuffer);
            _rentedBuffer = null;
        }

        public void Advance(int count)
        {
            Debug.Assert(_rentedBuffer != null);
            // Debug.Assert(count >= 0);
            Debug.Assert(_index <= _rentedBuffer.Length - count);

            _index += count;
        }

        public Memory<byte> GetMemory(int sizeHint = 0)
        {
            CheckAndResizeBuffer(sizeHint);
            return _rentedBuffer.AsMemory(_index);
        }

        public Span<byte> GetSpan(int sizeHint = 0)
        {
            CheckAndResizeBuffer(sizeHint);
            return _rentedBuffer.AsSpan(_index);
        }

// #if BUILDING_INBOX_LIBRARY
        public async ValueTask FlushAsync(Utf8JsonWriter writer, CancellationToken cancellationToken = default)
        {
            if (_stream == null)
                return;
            
            // Call sync because we are working with in-memory buffer
            // ReSharper disable once MethodHasAsyncOverloadWithCancellation
            if(writer.BytesPending > 0)
                writer.Flush();

            var writtenMemory = WrittenMemory;
            if (writtenMemory.Length == 0)
                return;
            
            await _stream.WriteAsync(writtenMemory, cancellationToken).ConfigureAwait(false);
            
            ClearHelper();

            _flushThreshold = CalculateFlushThreshold();
        }
// #else
//         internal Task WriteToStreamAsync(Stream destination, CancellationToken cancellationToken)
//         {
//             return destination.WriteAsync(_rentedBuffer, 0, _index, cancellationToken);
//         }
// #endif

        private void CheckAndResizeBuffer(int sizeHint)
        {
            Debug.Assert(_rentedBuffer != null);
            Debug.Assert(sizeHint >= 0);

            if (sizeHint == 0)
            {
                sizeHint = MinimumBufferSize;
            }

            int availableSpace = _rentedBuffer.Length - _index;

            if (sizeHint > availableSpace)
            {
                int growBy = Math.Max(sizeHint, _rentedBuffer.Length);

                int newSize = checked(_rentedBuffer.Length + growBy);

                byte[] oldBuffer = _rentedBuffer;

                _rentedBuffer = ArrayPool<byte>.Shared.Rent(newSize);

                Debug.Assert(oldBuffer.Length >= _index);
                Debug.Assert(_rentedBuffer.Length >= _index);

                Span<byte> previousBuffer = oldBuffer.AsSpan(0, _index);
                previousBuffer.CopyTo(_rentedBuffer);
                previousBuffer.Clear();
                ArrayPool<byte>.Shared.Return(oldBuffer);
            }

            Debug.Assert(_rentedBuffer.Length - _index > 0);
            Debug.Assert(_rentedBuffer.Length - _index >= sizeHint);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CalculateFlushThreshold() => (int) (Capacity * FlushThreshold);
    }
}