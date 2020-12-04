using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Converters;

namespace EfficientDynamoDb.Internal.JsonConverters
{
    /// <summary>
    /// Converter to convert enums to and from strings.
    /// </summary>
    /// <remarks>
    /// Reading is case insensitive, writing can be customized via a <see cref="JsonNamingPolicy" />.
    /// </remarks>
    internal sealed class DdbEnumJsonConverterFactory : JsonConverterFactory
    {
        /// <inheritdoc />
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        /// <inheritdoc />
        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(DdbEnumJsonConverter<>).MakeGenericType(typeToConvert),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                new object?[] { },
                culture: null)!;

            return converter;
        }
    }
}