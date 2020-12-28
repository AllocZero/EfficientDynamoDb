using System.Buffers.Text;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
{
    internal sealed class ShortDdbConverter : NumberDdbConverter<short>
    {
        public override short Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToShort();
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref short value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }
        
        public override short Read(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out short value, out _))
                throw new DdbException($"Couldn't parse short ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }
    }
}