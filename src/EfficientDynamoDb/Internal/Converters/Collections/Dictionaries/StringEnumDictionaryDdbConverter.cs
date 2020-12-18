using System;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.TypeParsers;

namespace EfficientDynamoDb.Internal.Converters.Collections.Dictionaries
{
    internal sealed class StringEnumDictionaryDdbConverter<TEnum, TValue> : DictionaryDdbConverterBase<TEnum, TValue> where TEnum : struct, Enum
    {
        public StringEnumDictionaryDdbConverter(DdbConverter<TValue> valueConverter) : base(valueConverter)
        {
        }

        protected override TEnum ParseValue(string value)
        {
            if (!EnumParser.TryParseCaseInsensitive(value, out TEnum parsedValue))
                throw new DdbException($"Couldn't parse '{typeof(TEnum).Name}' enum '{value}' value.");

            return parsedValue;
        }
        
        protected override void WriteKeyName(Utf8JsonWriter writer, TEnum value) => writer.WritePropertyName(value.ToString());
    }
}