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
    internal sealed class LongDdbConverter : NumberDdbConverter<long>
    {
        public override long Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToLong();
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref long value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }
        
        public override long Read(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out long value, out _))
                throw new DdbException($"Couldn't parse long ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }
    }
}