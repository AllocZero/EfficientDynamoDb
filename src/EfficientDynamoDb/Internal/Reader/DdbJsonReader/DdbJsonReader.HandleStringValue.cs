using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Reader
{
    internal partial class DdbJsonReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleStringValue(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
            ref var current = ref state.GetCurrent();

            switch (current.AttributeType)
            {
                case AttributeType.String:
                {
                    ref var prevState = ref state.GetPrevious();
                    prevState.StringBuffer.Add(prevState.KeyName!);
                    prevState.AttributesBuffer.Add(new AttributeValue(new StringAttributeValue(reader.GetString()!)));
                    break;
                }
                case AttributeType.Number:
                {
                    ref var prevState = ref state.GetPrevious();
                    prevState.StringBuffer.Add(prevState.KeyName!);
                    prevState.AttributesBuffer.Add(new AttributeValue(new NumberAttributeValue(reader.GetString()!)));
                    break;
                }
                case AttributeType.Binary:
                {
                    ref var prevState = ref state.GetPrevious();
                    prevState.StringBuffer.Add(prevState.KeyName!);
                    prevState.AttributesBuffer.Add(new AttributeValue(new BinaryAttributeValue(reader.GetBytesFromBase64())));
                    break;
                }
                default:
                {
                    if (current.KeyName == null)
                    {
                        current.StringBuffer.Add(reader.GetString()!);
                    }
                    else
                    {
                        var value = reader.GetString();
                        current.StringBuffer.Add(current.KeyName);
                        current.AttributesBuffer.Add(value != null ? new AttributeValue(new StringAttributeValue(value)) : new AttributeValue(new NullAttributeValue(true)));
                    }
                    break;
                }
            }
        }
    }
}