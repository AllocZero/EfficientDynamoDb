using System;
using System.Globalization;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;

namespace EfficientDynamoDb.Internal.Converters.Collections.Dictionaries
{
    internal sealed class IntDictionaryDdbConverter<TValue> : DictionaryDdbConverterBase<int, TValue>
    {
        public IntDictionaryDdbConverter(DdbConverter<TValue> valueConverter) : base(valueConverter)
        {
        }

        protected override int ParseValue(string value)
        {
            if (!int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
                throw new DdbException($"Couldn't parse '{nameof(Int32)}' enum '{value}' value.");

            return parsedValue;
        }

        protected override void WriteKeyName(Utf8JsonWriter writer, int value) => writer.WritePropertyName(value);
    }
}