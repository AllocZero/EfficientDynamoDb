using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader.Metadata;

namespace EfficientDynamoDb.Internal.Reader
{
    [StructLayout(LayoutKind.Auto)]
    internal struct DdbEntityReadStack : IDisposable
    {
        public const int DefaultStackLength = 16;

        private DdbEntityReadStackFrame[] _previous;

        private int _index;

        private int _ddbObjectLevel;

        private int _usedFrames;
        
        public long BytesConsumed;

        public int IsDdbSyntax;

        public bool UseFastPath;
        
        public bool IsLastFrame => _index == 0;

        public readonly DynamoDbContextMetadata Metadata;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref DdbEntityReadStackFrame GetCurrent() => ref _previous[_index];
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref DdbEntityReadStackFrame GetPrevious() => ref _previous[_index - 1];

        public DdbEntityReadStack(int defaultStackLength, DynamoDbContextMetadata metadata) : this()
        {
            _previous = ArrayPool<DdbEntityReadStackFrame>.Shared.Rent(defaultStackLength);
            ref var current = ref GetCurrent();
            current.Reset();
            Metadata = metadata;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ContainsDdbAttributeType() => _ddbObjectLevel != 0 && (_ddbObjectLevel & 1) == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PushObject()
        {
            if (_index == _previous.Length)
                Resize();
            
            ref var prev = ref GetCurrent();

            DdbClassInfo nextClassInfo;
            if (prev.ClassInfo!.ClassType == DdbClassType.Enumerable)
            {
                nextClassInfo = prev.ClassInfo.ElementClassInfo!;
            }
            else
            {
                nextClassInfo = prev.PropertyInfo!.RuntimeClassInfo;
            }

            _index++;

            ref var current = ref GetCurrent();
            current.Reset();

            current.ClassInfo = nextClassInfo;

            _ddbObjectLevel += IsDdbSyntax;

            if (_usedFrames < _index)
                _usedFrames = _index;
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
            
            if (_usedFrames < _index)
                _usedFrames = _index;
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
            _previous.AsSpan(0, _usedFrames + 1).Clear();
            ArrayPool<DdbEntityReadStackFrame>.Shared.Return(_previous);
        }

        private void Resize()
        {
            var oldBuffer = _previous!;
            _previous = ArrayPool<DdbEntityReadStackFrame>.Shared.Rent(oldBuffer.Length * 2);
            var span = oldBuffer.AsSpan();
            span.CopyTo(_previous);
            span.Clear();
            ArrayPool<DdbEntityReadStackFrame>.Shared.Return(oldBuffer);
        }
    }
}