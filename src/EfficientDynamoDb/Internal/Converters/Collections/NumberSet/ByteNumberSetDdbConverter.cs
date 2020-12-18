using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Collections.NumberSet
{
    internal sealed class ByteNumberSetDdbConverter : NumberSetDdbConverter<byte>
    {
        protected override byte ParseValue(string value) => byte.Parse(value, CultureInfo.InvariantCulture);

        public override void Write(Utf8JsonWriter writer, string attributeName, ref HashSet<byte> value)
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