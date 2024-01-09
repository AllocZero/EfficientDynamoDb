using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using EfficientDynamoDb.Internal.Core;
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
            var enumString = value.ToString();
            
            Span<char> buffer = stackalloc char[enumString.Length * 2]; // Allocate enough space to account for new underscores
            var sb = new NoAllocStringBuilder(in buffer, true);

            enumString.ToUpperSnakeCase(ref sb);
            
            writer.WriteStringValue(sb.GetBuffer());
        }
    }
}