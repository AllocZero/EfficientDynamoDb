using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.NumberSet
{
    internal sealed class DoubleNumberSetDdbConverter : NumberSetDdbConverter<double>
    {
        protected override double ParseValue(string value) => double.Parse(value, CultureInfo.InvariantCulture);
        
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