using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class ByteDdbConverter : NumberDdbConverter<byte>
    {
        public override byte Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToByte();

        public override void Write(Utf8JsonWriter writer, string attributeName, ref byte value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }
    }
}