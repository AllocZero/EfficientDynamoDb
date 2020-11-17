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

        private int _objectLevel;

        private int _usedFrames;

        public long BytesConsumed;
        
        public bool IsLastFrame => _index == 0;

        public ref DdbReadStackFrame Current => ref _previous[_index];

        public DdbReadStack(int defaultStackLength) : this()
        {
            _previous = ArrayPool<DdbReadStackFrame>.Shared.Rent(defaultStackLength);
            Current.Reset();
            Current.KeysBuffer = new ReusableBuffer<string>(DdbReadStackFrame.DefaultAttributeBufferSize);
            Current.AttributesBuffer = new ReusableBuffer<AttributeValue>(DdbReadStackFrame.DefaultAttributeBufferSize);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref DdbReadStackFrame GetPrevious()
        {
            return ref _previous![_index - 1];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsDdbAttributeType() => _objectLevel != 0 && (_objectLevel & 1) == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushObject()
        {
            if (_index == DefaultStackLength)
                Resize();
            
            _index++;

            ref var current = ref Current;
            current.Reset();

            _objectLevel++;

            EnsureBufferExists(ref current);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushArray()
        {
            if (_index == DefaultStackLength)
                Resize();
            
            _index++;

            ref var current = ref Current;
            current.Reset();

            _objectLevel += (_objectLevel>>31) - (-_objectLevel>>31);

            EnsureBufferExists(ref current);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PopObject()
        {
            _objectLevel--;
            
            --_index;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PopArray()
        {
            _objectLevel -= (_objectLevel>>31) - (-_objectLevel>>31);
            
            --_index;
        }

        public void Dispose()
        {
            // Every even frame except zero contains a pooled buffer
            for (var i = 0; i < _usedFrames; i++)
            {
                if (_previous[i].KeysBuffer.RentedBuffer != null)
                {
                    _previous[i].KeysBuffer.Dispose();
                    _previous[i].AttributesBuffer.Dispose();
                }
            }

            ArrayPool<DdbReadStackFrame>.Shared.Return(_previous);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Resize()
        {
            var oldBuffer = _previous!;
            _previous = ArrayPool<DdbReadStackFrame>.Shared.Rent(oldBuffer.Length * 2);
            Array.Copy(oldBuffer, _previous, oldBuffer.Length);
            ArrayPool<DdbReadStackFrame>.Shared.Return(oldBuffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureBufferExists(ref DdbReadStackFrame current)
        {
            if (current.KeysBuffer.RentedBuffer != null) 
                return;
            
            current.KeysBuffer = new ReusableBuffer<string>(_previous[_index - 1].BufferLengthHint);
            current.AttributesBuffer = new ReusableBuffer<AttributeValue>(_previous[_index - 1].BufferLengthHint);

            if (_index > _usedFrames)
                _usedFrames = _index;
        }
    }
}