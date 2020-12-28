using System.Buffers.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Collections.NumberSet
{
    internal sealed class DoubleNumberSetDdbConverter : NumberSetDdbConverter<double>
    {
        protected override double ParseValue(string value) => double.Parse(value, CultureInfo.InvariantCulture);

        protected override double ReadValue(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out double value, out _))
                throw new DdbException($"Couldn't parse double ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }

        public override void Write(Utf8JsonWriter writer, string attributeName, ref HashSet<double> value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WritePropertyName(DdbTypeNames.NumberSet);
            
            writer.WriteStartArray();

            foreach (var item in value)
                writer.WriteStringValue(item);
            
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}