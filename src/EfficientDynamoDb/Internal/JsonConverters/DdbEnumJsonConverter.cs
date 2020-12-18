using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using EfficientDynamoDb.Internal.TypeParsers;

namespace EfficientDynamoDb.Internal.JsonConverters
{
    internal class DdbEnumJsonConverter<T> : JsonConverter<T> where T: struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var enumString = reader.GetString();

            if (!EnumParser.TryParseCaseInsensitive(enumString, out T value))
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