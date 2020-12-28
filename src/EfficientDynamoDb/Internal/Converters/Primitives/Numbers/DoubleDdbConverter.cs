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
    internal sealed class DoubleDdbConverter : NumberDdbConverter<double>
    {
        public override double Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToDouble();
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref double value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, value);
            writer.WriteEndObject();
        }
        
        public override double Read(ref Utf8JsonReader reader, AttributeType attributeType)
        {
            if (!Utf8Parser.TryParse(reader.ValueSpan, out double value, out _))
                throw new DdbException($"Couldn't parse double ddb value from '{reader.GetString()}'.");

            return value;
        }
    }
}