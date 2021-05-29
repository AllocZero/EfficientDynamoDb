using System.Runtime.CompilerServices;

namespace EfficientDynamoDb.Internal.Core.Utilities
{
    internal static class FastMath
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int TwoPowX(int power) => 1 << power;
    }
}