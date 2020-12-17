using System;
using System.Globalization;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;

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
    }
}