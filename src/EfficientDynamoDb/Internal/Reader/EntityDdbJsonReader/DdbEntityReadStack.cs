using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Core;
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

        /// <summary>
        ///  Used to restore the stack state without calling Reset in case when we need to pause and fill the input buffer
        /// For example if _continuationCount = 3, next 3 Push calls just increment the index without modifying existing state
        /// In the simplest case of a single input buffer, continuation counter is never used and is always equal to 0
        /// </summary>
        private int _continuationCount;

        private int _usedFrames;
        private bool _usedPools;
        
        public long BytesConsumed;

        /// <summary>
        /// In case when entire response fits a single input buffer - use optimized reading logic that does not need to maintain the parsing state
        /// </summary>
        public bool UseFastPath;

        public bool ReadAhead;

        public byte[]? Buffer;
        public int BufferStart;
        public int BufferLength;

        public readonly DynamoDbContextMetadata Metadata;
        
        public KeysCache KeysCache;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref DdbEntityReadStackFrame GetCurrent() => ref _previous[_index];
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref DdbEntityReadStackFrame GetPrevious() => ref _previous[_index - 1];

        public DdbEntityReadStack(int defaultStackLength, DynamoDbContextMetadata metadata) : this()
        {
            _previous = ArrayPool<DdbEntityReadStackFrame>.Shared.Rent(defaultStackLength);
            Metadata = metadata;
        }

        public void Push()
        {
            if (_index == _previous.Length)
                Resize();

            if (_continuationCount == 0)
            {
                ref var prev = ref GetCurrent();

                var nextClassInfo = prev.NextClassInfo;
                nextClassInfo ??= ((DdbClassType.Enumerable | DdbClassType.Dictionary) & prev.ClassInfo!.ClassType) != 0
                    ? prev.ClassInfo.ElementClassInfo!
                    : prev.PropertyInfo!.RuntimeClassInfo;

                _index++;

                ref var current = ref GetCurrent();
                current.Reset();

                current.ClassInfo = nextClassInfo;

                if (_usedFrames < _index)
                    _usedFrames = _index;
            }
            else
            {
                _continuationCount--;
                _index++;
            }
        }
        
        public void PushDocument()
        {
            if (_index == _previous.Length)
                Resize();

            if (_continuationCount == 0)
            {
                _index++;

                ref var current = ref GetCurrent();
                current.Reset();

                if (_usedFrames < _index)
                    _usedFrames = _index;
                
                if (current.StringBuffer.RentedBuffer == null)
                {
                    _usedPools = true;
                    current.StringBuffer = new ReusableBuffer<string>(DdbEntityReadStackFrame.DefaultAttributeBufferSize);
                    current.AttributesBuffer = new ReusableBuffer<AttributeValue>(DdbEntityReadStackFrame.DefaultAttributeBufferSize);
                }
            }
            else
            {
                _continuationCount--;
                _index++;
            }
        }

        public void Pop(bool success)
        {
            if (!success && _continuationCount == 0)
                _continuationCount = _index;
            
            --_index;
        }
        
        public void Dispose()
        {
            if (_usedPools)
            {
                for (var i = 0; i <= _usedFrames; i++)
                {
                    ref var state = ref _previous[i];

                    if (state.StringBuffer.RentedBuffer == null)
                        continue;
                    
                    state.StringBuffer.Dispose();
                    state.AttributesBuffer.Dispose();
                } 
            }

            _previous.AsSpan(0, _usedFrames + 1).Clear();
            ArrayPool<DdbEntityReadStackFrame>.Shared.Return(_previous);

            if (KeysCache.IsInitialized)
                KeysCache.Dispose();
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