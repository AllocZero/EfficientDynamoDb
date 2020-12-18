using System;
using System.Globalization;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;

namespace EfficientDynamoDb.Internal.Converters.Collections.Dictionaries
{
    internal sealed class UShortDictionaryDdbConverter<TValue> : DictionaryDdbConverterBase<ushort, TValue>
    {
        public UShortDictionaryDdbConverter(DdbConverter<TValue> valueConverter) : base(valueConverter)
        {
        }

        protected override ushort ParseValue(string value)
        {
            if (!ushort.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
                throw new DdbException($"Couldn't parse '{nameof(UInt16)}' enum '{value}' value.");

            return parsedValue;
        }
        
        protected override void WriteKeyName(Utf8JsonWriter writer, ushort value) => writer.WritePropertyName(value);
    }
}