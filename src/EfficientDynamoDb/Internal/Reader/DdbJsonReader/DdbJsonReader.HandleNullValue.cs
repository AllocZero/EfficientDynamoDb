using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Reader
{
    internal static partial class DdbJsonReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleNullValue(ref DdbReadStack state)
        {
            ref var current = ref state.GetCurrent();

            current.StringBuffer.Add(current.KeyName!);
            current.AttributesBuffer.Add(new AttributeValue(new NullAttributeValue(true)));
        }
    }
}