using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EfficientDynamoDb.Internal.Reader
{
    [StructLayout(LayoutKind.Auto)]
    public struct DdbReadStack : IDisposable
    {
        public const int DefaultStackLength = 16;

        private DdbReadStackFrame[]? _previous;

        public DdbReadStackFrame Current;
        
        private int _index;

        private int _objectLevel;

        public long BytesConsumed;
        
        public bool IsLastFrame => _index == 0;

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
            _previous ??= ArrayPool<DdbReadStackFrame>.Shared.Rent(DefaultStackLength);

            if (_index == DefaultStackLength)
                Resize();
            
            // Use a previously allocated slot.
            _previous[_index++] = Current;

            Current.Reset();

            _objectLevel++;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushArray()
        {
            _previous ??= ArrayPool<DdbReadStackFrame>.Shared.Rent(DefaultStackLength);

            if (_index == DefaultStackLength)
                Resize();
            
            // Use a previously allocated slot.
            _previous[_index++] = Current;

            Current.Reset();

            _objectLevel += (_objectLevel>>31) - (-_objectLevel>>31);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PopObject()
        {
            _objectLevel--;
            
            Current = _previous![--_index];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PopArray()
        {
            _objectLevel -= (_objectLevel>>31) - (-_objectLevel>>31);
            
            Current = _previous![--_index];
        }

        public void Dispose()
        {
            if(_previous != null)
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