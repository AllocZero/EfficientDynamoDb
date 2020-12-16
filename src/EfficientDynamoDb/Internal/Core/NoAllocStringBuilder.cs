using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace EfficientDynamoDb.Internal.Core
{
    [StructLayout(LayoutKind.Auto)]
    internal ref struct NoAllocStringBuilder
    {
        public const int MaxStackAllocSize = 256;
        
        private Span<char> _buffer;
        private readonly bool _allowResize;
        private char[]? _pooledBuffer;
        private int _index;

        public NoAllocStringBuilder(in Span<char> initialBuffer, bool allowResize)
        {
            _buffer = initialBuffer;
            _allowResize = allowResize;
            _pooledBuffer = null;
            _index = 0;
        }

        public NoAllocStringBuilder(int initialSize) : this()
        {
            _buffer = _pooledBuffer = ArrayPool<char>.Shared.Rent(initialSize);
            _index = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpan<char> GetBuffer() => _buffer.Slice(0, _index);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(in ReadOnlySpan<char> value)
        {
            if (value.Length + _index > _buffer.Length)
                Resize(value.Length + _index);
            
            value.CopyTo(_buffer.Slice(_index));
            _index += value.Length;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(string value)
        {
            if (value.Length + _index > _buffer.Length)
                Resize(value.Length + _index);
            
            value.AsSpan().CopyTo(_buffer.Slice(_index));
            _index += value.Length;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(char value)
        {
            if (_index + 1 > _buffer.Length)
                Resize(_index + 1);

            _buffer[_index++] = value;
        }

        private void Resize(int minSize)
        {
            if(!_allowResize)
                throw new InvalidOperationException("Buffer reached max allowed size and can't be resized.");
            
            var newSize = _buffer.Length * 2;
            if (newSize < minSize)
                newSize = (int)(minSize * 1.5);

            if (_pooledBuffer == null)
            {
                _buffer = _pooledBuffer = ArrayPool<char>.Shared.Rent(newSize);
                return;
            }
            
            var oldBuffer = _pooledBuffer;
            
            _buffer = _pooledBuffer = ArrayPool<char>.Shared.Rent(newSize);
            
            oldBuffer.CopyTo(_buffer);
            oldBuffer.AsSpan().Clear();
            ArrayPool<char>.Shared.Return(oldBuffer);
        }

        public void Clear()
        {
            _index = 0;
        }

        public void Dispose()
        {
            if (_pooledBuffer == null)
                return;
            
            _pooledBuffer.AsSpan().Clear();
            ArrayPool<char>.Shared.Return(_pooledBuffer);
            _pooledBuffer = null;
        }

        public override string ToString() => new string(GetBuffer());
    }
}