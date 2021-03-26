using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Reader
{
    internal static partial class DdbJsonReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleEndObject(ref DdbReadStack state)
        {
            if (state.IsLastFrame)
                return;

            var document = state.GetCurrent().CreateDocumentFromBuffer();

            state.PopObject();

            if (document == null)
                return;

            ref var current = ref state.GetCurrent();

            if (current.AttributeType == AttributeType.Map)
            {
                ref var prevState = ref state.GetPrevious();
                prevState.StringBuffer.Add(prevState.KeyName!);
                prevState.AttributesBuffer.Add(new AttributeValue(new MapAttributeValue(document)));
            }
            else
            {
                current.StringBuffer.Add(current.KeyName!);
                current.AttributesBuffer.Add(new AttributeValue(new MapAttributeValue(document)));
            }
        }
    }
}