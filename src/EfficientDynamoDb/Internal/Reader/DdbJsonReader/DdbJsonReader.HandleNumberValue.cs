using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Reader
{
    internal static partial class DdbJsonReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleNumberValue(ref Utf8JsonReader reader, ref DdbReadStack state, IParsingOptions callbacks)
        {
            if (state.IsLastFrame && callbacks.HasNumberCallback)
                callbacks.OnNumber(ref reader, ref state);

            ref var current = ref state.GetCurrent();

            current.StringBuffer.Add(current.KeyName!);
            current.AttributesBuffer.Add(new AttributeValue(new NumberAttributeValue(Encoding.UTF8.GetString(reader.ValueSpan))));
        }
    }
}