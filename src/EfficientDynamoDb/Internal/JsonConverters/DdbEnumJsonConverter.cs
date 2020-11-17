using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EfficientDynamoDb.Internal.JsonConverters
{
    public class DdbEnumJsonConverter<T> : JsonConverter<T> where T: struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var enumString = reader.GetString();

            // Try parsing case sensitive first
            if (!Enum.TryParse(enumString, out T value) && !Enum.TryParse(enumString, true, out value))
            {
                return default;
            }

            return value;      
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}