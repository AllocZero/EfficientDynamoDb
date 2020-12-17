using System;
using System.Globalization;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.Internal.Converters.Collections.Dictionaries
{
    internal sealed class ShortDictionaryDdbConverter<TValue> : DictionaryDdbConverterBase<short, TValue>
    {
        public ShortDictionaryDdbConverter(DdbConverter<TValue> valueConverter) : base(valueConverter)
        {
        }

        protected override short ParseValue(string value)
        {
            if (!short.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
                throw new DdbException($"Couldn't parse '{nameof(Int16)}' enum '{value}' value.");

            return parsedValue;
        }
    }
}