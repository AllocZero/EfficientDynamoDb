using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class UIntDdbConverter : NumberDdbConverter<uint>
    {
        public override uint Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToUInt();
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref uint value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }
    }
}