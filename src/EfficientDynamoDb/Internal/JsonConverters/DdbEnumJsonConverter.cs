using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.TypeParsers;

namespace EfficientDynamoDb.Internal.JsonConverters
{
    internal class DdbEnumJsonConverter<T> : JsonConverter<T> where T: struct, Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var enumString = reader.GetString();

            return EnumParser.TryParseUpperSnakeCase(enumString, out T value)
                ? value
                : default;
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var enumString = Enum.GetName(value) ?? value.ToString();

            // Allocate enough space to account for new underscores.
            Span<char> buffer = stackalloc char[enumString.Length * 2];
            var charsWritten = enumString.ToUpperSnakeCaseAscii(buffer);

            writer.WriteStringValue(buffer[..charsWritten]);
        }
    }
}