using System.Buffers.Text;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
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
        
        public override ushort Read(ref Utf8JsonReader reader, AttributeType attributeType)
        {
            if (!Utf8Parser.TryParse(reader.ValueSpan, out ushort value, out _))
                throw new DdbException($"Couldn't parse ushort ddb value from '{reader.GetString()}'.");

            return value;
        }
    }
}