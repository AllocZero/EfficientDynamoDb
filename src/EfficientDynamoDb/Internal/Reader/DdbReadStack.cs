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
        
        public void Push(int objectLevelChange)
        {
            _previous ??= ArrayPool<DdbReadStackFrame>.Shared.Rent(DefaultStackLength);

            if (_index == DefaultStackLength)
                throw new InvalidOperationException("Ddb stack frame limit reached.");
            
            // Use a previously allocated slot.
            _previous[_index++] = Current;

            Current.Reset();
            
            _objectLevel+=objectLevelChange;
        }

        public void Pop(int objectLevelChange)
        {
            _objectLevel-=objectLevelChange;
            
            Current = _previous![--_index];
        }

        public void Dispose()
        {
            if(_previous != null)
                ArrayPool<DdbReadStackFrame>.Shared.Return(_previous);
        }
    }
}