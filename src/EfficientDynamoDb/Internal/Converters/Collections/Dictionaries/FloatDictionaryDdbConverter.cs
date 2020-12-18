using System;
using System.Globalization;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;

namespace EfficientDynamoDb.Internal.Converters.Collections.Dictionaries
{
    internal sealed class FloatDictionaryDdbConverter<TValue> : DictionaryDdbConverterBase<float, TValue>
    {
        public FloatDictionaryDdbConverter(DdbConverter<TValue> valueConverter) : base(valueConverter)
        {
        }

        protected override float ParseValue(string value)
        {
            if (!float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue))
                throw new DdbException($"Couldn't parse '{nameof(Single)}' enum '{value}' value.");

            return parsedValue;
        }
        
        protected override void WriteKeyName(Utf8JsonWriter writer, float value) => writer.WritePropertyName(value);
    }
}