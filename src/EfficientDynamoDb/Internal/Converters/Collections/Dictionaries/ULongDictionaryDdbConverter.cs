using System;
using System.Globalization;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;

namespace EfficientDynamoDb.Internal.Converters.Collections.Dictionaries
{
    internal sealed class ULongDictionaryDdbConverter<TValue> : DictionaryDdbConverterBase<ulong, TValue>
    {
        public ULongDictionaryDdbConverter(DdbConverter<TValue> valueConverter) : base(valueConverter)
        {
        }

        protected override ulong ParseValue(string value)
        {
            if (!ulong.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
                throw new DdbException($"Couldn't parse '{nameof(UInt64)}' enum '{value}' value.");

            return parsedValue;
        }
        
        protected override void WriteKeyName(Utf8JsonWriter writer, ulong value) => writer.WritePropertyName(value);
    }
}