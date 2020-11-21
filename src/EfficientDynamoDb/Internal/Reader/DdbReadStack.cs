using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Internal.Reader
{
    [StructLayout(LayoutKind.Auto)]
    public struct DdbReadStack : IDisposable
    {
        public const int DefaultStackLength = 16;

        private DdbReadStackFrame[] _previous;
        
        private int _index;

        private int _ddbObjectLevel;

        private int _usedFrames;

        public long BytesConsumed;

        public int IsDdbSyntax;
        
        public bool IsLastFrame => _index == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref DdbReadStackFrame GetCurrent() => ref _previous[_index];
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref DdbReadStackFrame GetPrevious() => ref _previous[_index - 1];

        public DdbReadStack(int defaultStackLength, JsonObjectMetadata? metadata) : this()
        {
            _previous = ArrayPool<DdbReadStackFrame>.Shared.Rent(defaultStackLength);
            ref var current = ref GetCurrent();
            current.Reset();
            current.StringBuffer = new ReusableBuffer<string>(DdbReadStackFrame.DefaultAttributeBufferSize);
            current.AttributesBuffer = new ReusableBuffer<AttributeValue>(DdbReadStackFrame.DefaultAttributeBufferSize);
            current.Metadata = metadata;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsDdbAttributeType() => _ddbObjectLevel != 0 && (_ddbObjectLevel & 1) == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushObject()
        {
            if (_index == _previous.Length)
                Resize();

            _index++;

            ref var current = ref GetCurrent();
            current.Reset();
            current.Metadata = GetPrevious().NextMetadata;

            _ddbObjectLevel += IsDdbSyntax;

            EnsureBufferExists(ref current);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushArray()
        {
            if (_index == _previous.Length)
                Resize();
            
            _index++;

            ref var current = ref GetCurrent();
            current.Reset();
            
            _ddbObjectLevel += (_ddbObjectLevel>>31) - (-_ddbObjectLevel>>31);

            EnsureBufferExists(ref current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PopObject()
        {
            _ddbObjectLevel -= IsDdbSyntax;

            --_index;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PopArray()
        {
            _ddbObjectLevel -= (_ddbObjectLevel>>31) - (-_ddbObjectLevel>>31);
            
            --_index;
        }

        public void Dispose()
        {
            // Every even frame except zero contains a pooled buffer
            for (var i = 0; i <= _usedFrames; i++)
            {
                _previous[i].StringBuffer.Dispose();
                _previous[i].AttributesBuffer.Dispose();
            }

            _previous.AsSpan(0, _usedFrames + 1).Clear();
            ArrayPool<DdbReadStackFrame>.Shared.Return(_previous);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Resize()
        {
            var oldBuffer = _previous!;
            _previous = ArrayPool<DdbReadStackFrame>.Shared.Rent(oldBuffer.Length * 2);
            var span = oldBuffer.AsSpan();
            span.CopyTo(_previous);
            span.Clear();
            ArrayPool<DdbReadStackFrame>.Shared.Return(oldBuffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureBufferExists(ref DdbReadStackFrame current)
        {
            if (current.StringBuffer.RentedBuffer != null) 
                return;

            var size = _previous[_index - 1].BufferLengthHint;
            current.StringBuffer = new ReusableBuffer<string>(size);
            current.AttributesBuffer = new ReusableBuffer<AttributeValue>(size);

            if (_index > _usedFrames)
                _usedFrames = _index;
        }
    }
}