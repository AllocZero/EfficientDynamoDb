using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
{
    internal sealed class FloatDdbConverter : NumberDdbConverter<float>
    {
        public override float Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToFloat();
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref float value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }
    }
}