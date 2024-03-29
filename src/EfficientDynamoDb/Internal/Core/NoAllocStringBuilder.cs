using System;
using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using EfficientDynamoDb.Internal.Constants;

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
        public void Append(StringBuilder value)
        {
            if (value.Length + _index > _buffer.Length)
                Resize(value.Length + _index);

            value.CopyTo(0, _buffer.Slice(_index), value.Length);
            _index += value.Length;
        }

        public void Append(int value)
        {
            if (!value.TryFormat(_buffer.Slice(_index), out var charsWritten))
                Resize(_buffer.Length + PrimitiveLengths.Int);

            if (!value.TryFormat(_buffer.Slice(_index), out charsWritten))
                Debug.Fail("Format should always be successful after resize");

            _index += charsWritten;
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
                var oldBuffer = _buffer;
                
                _buffer = _pooledBuffer = ArrayPool<char>.Shared.Rent(newSize);
                oldBuffer.CopyTo(_buffer);
                return;
            }
            
            var oldArray = _pooledBuffer;
            
            _buffer = _pooledBuffer = ArrayPool<char>.Shared.Rent(newSize);
            
            oldArray.CopyTo(_buffer);
            oldArray.AsSpan().Clear();
            ArrayPool<char>.Shared.Return(oldArray);
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