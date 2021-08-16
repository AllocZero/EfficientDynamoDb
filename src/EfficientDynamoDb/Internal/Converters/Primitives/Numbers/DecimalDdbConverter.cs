using System.Buffers.Text;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Extensions;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
{
    internal sealed class DecimalDdbConverter : NumberDdbConverter<decimal>, IDictionaryKeyConverter<decimal>, ISetValueConverter<decimal>
    {
        public override decimal Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToDecimal();

        public override void Write(in DdbWriter writer, ref decimal value)
        {
            writer.JsonWriter.WriteStartObject();
            writer.JsonWriter.WriteString(DdbTypeNames.Number, value);
            writer.JsonWriter.WriteEndObject();
        }

        public void WritePropertyName(in DdbWriter writer, ref decimal value) => writer.JsonWriter.WritePropertyName(value);
        
        public void WriteStringValue(in DdbWriter writer, ref decimal value) => writer.JsonWriter.WriteStringValue(value);

        public override decimal Read(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out decimal value, out _))
                throw new DdbException($"Couldn't parse decimal ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }
    }
}