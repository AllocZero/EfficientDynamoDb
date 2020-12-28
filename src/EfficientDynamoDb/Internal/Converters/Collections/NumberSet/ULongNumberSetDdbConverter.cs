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
    internal sealed class ULongNumberSetDdbConverter : NumberSetDdbConverter<ulong>
    {
        protected override ulong ParseValue(string value) => ulong.Parse(value, CultureInfo.InvariantCulture);

        protected override ulong ReadValue(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out ulong value, out _))
                throw new DdbException($"Couldn't parse ulong ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return value;
        }

        public override void Write(Utf8JsonWriter writer, string attributeName, ref HashSet<ulong> value)
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