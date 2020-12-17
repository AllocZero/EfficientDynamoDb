using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
{
    internal sealed class ULongDdbConverter : NumberDdbConverter<ulong>
    {
        public override ulong Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToULong();
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref ulong value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }
    }
}