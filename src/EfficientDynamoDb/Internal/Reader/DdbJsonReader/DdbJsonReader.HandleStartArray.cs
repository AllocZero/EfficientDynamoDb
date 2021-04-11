using System.Runtime.CompilerServices;

namespace EfficientDynamoDb.Internal.Reader
{
    internal static partial class DdbJsonReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleStartArray(ref DdbReadStack state)
        {
            state.PushArray();

            ref var current = ref state.GetCurrent();
            var metadata = current.Metadata;
            if (metadata is null || !metadata.IsArray)
                return;

            current.NextMetadata = current.Metadata;
        }
    }
}