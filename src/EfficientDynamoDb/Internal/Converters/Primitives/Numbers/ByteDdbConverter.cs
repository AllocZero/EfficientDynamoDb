using System.Buffers.Text;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Reader;
using NotImplementedException = System.NotImplementedException;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
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

        public override byte Read(ref Utf8JsonReader reader, AttributeType attributeType)
        {
            if (!Utf8Parser.TryParse(reader.ValueSpan, out byte value, out _))
                throw new DdbException($"Couldn't parse byte ddb value from '{reader.GetString()}'.");

            return value;
        }
    }
}