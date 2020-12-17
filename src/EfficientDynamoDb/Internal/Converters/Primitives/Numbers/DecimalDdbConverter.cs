using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
{
    internal sealed class DecimalDdbConverter : NumberDdbConverter<decimal>
    {
        public override decimal Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToDecimal();
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref decimal value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }
    }
}