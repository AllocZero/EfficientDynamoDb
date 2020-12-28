using System.Buffers.Text;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
{
    internal sealed class IntDdbConverter : NumberDdbConverter<int>
    {
        public override int Read(in AttributeValue attributeValue) => attributeValue.ToInt();
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref int value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }

        public override int Read(ref Utf8JsonReader reader, AttributeType attributeType)
        {
            if (!Utf8Parser.TryParse(reader.ValueSpan, out int value, out _))
                throw new DdbException($"Couldn't parse int ddb value from '{reader.GetString()}'.");

            return value;
        }
    }
}