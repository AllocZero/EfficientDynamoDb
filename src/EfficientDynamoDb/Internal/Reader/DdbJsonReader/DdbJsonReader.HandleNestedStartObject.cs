using System.Runtime.CompilerServices;

namespace EfficientDynamoDb.Internal.Reader
{
    internal static partial class DdbJsonReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleNestedStartObject(ref DdbReadStack state)
        {
            state.PushObject();
        }
    }
}