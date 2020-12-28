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
        
        public override uint Read(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out uint value, out _))
                throw new DdbException($"Couldn't parse uint ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }
    }
}