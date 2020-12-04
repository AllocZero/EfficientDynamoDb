using System.Runtime.CompilerServices;

namespace EfficientDynamoDb.Internal.Reader
{
    internal static partial class DdbJsonReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleStartArray(ref DdbReadStack state)
        {
            state.PushArray();
        }
    }
}