using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace EfficientDynamoDb.Internal.Core
{
    [StructLayout(LayoutKind.Auto)]
    public struct ReusableBuffer<TValue> : IDisposable
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Resize()
        {
            var newSize = RentedBuffer!.Length * 2;

            TValue[] oldBuffer = RentedBuffer;

            RentedBuffer = ArrayPool<TValue>.Shared.Rent(newSize);

            Buffer.BlockCopy(oldBuffer, 0, RentedBuffer, 0, oldBuffer.Length);
            ArrayPool<TValue>.Shared.Return(oldBuffer);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            var rented = RentedBuffer;
            RentedBuffer = null;
            ArrayPool<TValue>.Shared.Return(rented);
        }
    }
}