using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EfficientDynamoDb.Internal.Core
{
    [StructLayout(LayoutKind.Auto)]
    internal struct ReusableBuffer<TValue>
    {
        public TValue[]? RentedBuffer;
        public int Index;

        public ReusableBuffer(int defaultBufferSize) 
        {
            RentedBuffer = ArrayPool<TValue>.Shared.Rent(defaultBufferSize);
            Index = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(in TValue value)
        {
            if (Index == RentedBuffer!.Length)
                Resize();

            RentedBuffer[Index++] = value;
        }

        private void Resize()
        {
            var newSize = RentedBuffer!.Length * 2;

            var oldBuffer = RentedBuffer;

            RentedBuffer = ArrayPool<TValue>.Shared.Rent(newSize);

            var span = oldBuffer.AsSpan();
            span.CopyTo(RentedBuffer);
            span.Clear();
            ArrayPool<TValue>.Shared.Return(oldBuffer);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            RentedBuffer.AsSpan().Clear();
            ArrayPool<TValue>.Shared.Return(RentedBuffer!);
            RentedBuffer = null;
        }
    }
}