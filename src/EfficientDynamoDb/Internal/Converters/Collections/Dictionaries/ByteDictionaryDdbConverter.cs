using System;
using System.Globalization;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.Internal.Converters.Collections.Dictionaries
{
    internal sealed class ByteDictionaryDdbConverter<TValue> : DictionaryDdbConverterBase<byte, TValue>
    {
        public ByteDictionaryDdbConverter(DdbConverter<TValue> valueConverter) : base(valueConverter)
        {
        }

        protected override byte ParseValue(string value)
        {
            if (!byte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
                throw new DdbException($"Couldn't parse '{nameof(Byte)}' enum '{value}' value.");

            return parsedValue;
        }
    }
}