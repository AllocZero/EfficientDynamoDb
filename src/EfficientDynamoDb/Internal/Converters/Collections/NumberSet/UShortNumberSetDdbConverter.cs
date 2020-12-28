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
    internal sealed class UShortNumberSetDdbConverter : NumberSetDdbConverter<ushort>
    {
        protected override ushort ParseValue(string value) => ushort.Parse(value, CultureInfo.InvariantCulture);

        protected override ushort ReadValue(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out ushort value, out _))
                throw new DdbException($"Couldn't parse ushort ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }

        public override void Write(Utf8JsonWriter writer, string attributeName, ref HashSet<ushort> value)
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