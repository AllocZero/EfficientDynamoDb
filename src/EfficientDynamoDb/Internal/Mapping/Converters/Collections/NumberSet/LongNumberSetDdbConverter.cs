using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.NumberSet
{
    internal sealed class LongNumberSetDdbConverter : NumberSetDdbConverter<long>
    {
        protected override long ParseValue(string value) => long.Parse(value, CultureInfo.InvariantCulture);
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref HashSet<long> value)
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