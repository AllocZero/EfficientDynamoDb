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

        // public DdbReadStackFrame Current;
        
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
            Current.DocumentBuffer = new ReusableBuffer<KeyValuePair<string, AttributeValue>>(32);
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
            if (_index == (DefaultStackLength - 1))
                Resize();
            
            // Use a previously allocated slot.
            _index++;

            ref var current = ref Current;
            // Current buffer should always be equal to next free frame to make sure that buffers are reused
            current.Reset();

            _objectLevel++;
            if (current.DocumentBuffer.RentedBuffer == null)
            {
                current.DocumentBuffer = new ReusableBuffer<KeyValuePair<string, AttributeValue>>(_previous[_index - 1].BufferLengthHint);

                // Copy buffer to next free frame otherwise last buffer will be recreated every single time
                //_previous[_index].DocumentBuffer.RentedBuffer = Current.DocumentBuffer.RentedBuffer;
                
                if (_index > _usedFrames)
                    _usedFrames = _index;
            }

            // if (Current.Items == null)
            // {
            //     Current.Items = new KeyValuePair<string, AttributeValue>[_previous[_index - 1].BufferLengthHint];
            //     _previous[_index].Items = Current.Items;
            // }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushArray()
        {
            if (_index == (DefaultStackLength - 1))
                Resize();
            
            // Use a previously allocated slot.
            _index++;

            ref var current = ref Current;
            // Current buffer should always be equal to next free frame to make sure that buffers are reused
            current.Reset();

            _objectLevel += (_objectLevel>>31) - (-_objectLevel>>31);

            if (current.DocumentBuffer.RentedBuffer == null)
            {
                current.DocumentBuffer = new ReusableBuffer<KeyValuePair<string, AttributeValue>>(_previous[_index - 1].BufferLengthHint);

                if (_index > _usedFrames)
                    _usedFrames = _index;
            }
            
            // if (Current.Items == null)
            // {
            //     Current.Items = new KeyValuePair<string, AttributeValue>[_previous[_index - 1].BufferLengthHint];
            //     _previous[_index].Items = Current.Items;
            // }
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
                if(_previous[i].DocumentBuffer.RentedBuffer != null)
                    _previous[i].DocumentBuffer.Dispose();
            }

            ArrayPool<DdbReadStackFrame>.Shared.Return(_previous);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Resize()
        {
            var oldBuffer = _previous!;
            _previous = ArrayPool<DdbReadStackFrame>.Shared.Rent(oldBuffer.Length * 2);
            Buffer.BlockCopy(oldBuffer, 0, _previous, 0, oldBuffer.Length);
            ArrayPool<DdbReadStackFrame>.Shared.Return(oldBuffer);
        }
    }
}