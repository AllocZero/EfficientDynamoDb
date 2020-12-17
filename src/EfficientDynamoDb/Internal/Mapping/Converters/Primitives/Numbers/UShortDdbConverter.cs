using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class UShortDdbConverter : NumberDdbConverter<ushort>
    {
        public override ushort Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToUShort();
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref ushort value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }
    }
}