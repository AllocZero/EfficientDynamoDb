using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.JsonConverters
{
    public class UnixDateTimeJsonConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) => reader.GetDouble().FromUnixSeconds();

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value.ToUnixSeconds());
        }
    }
}