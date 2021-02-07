using System.Runtime.CompilerServices;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static class BitExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBitSet(this int value, int mask) => (value & mask) != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SetBit(this int value, int mask) => value | mask;
    }
}