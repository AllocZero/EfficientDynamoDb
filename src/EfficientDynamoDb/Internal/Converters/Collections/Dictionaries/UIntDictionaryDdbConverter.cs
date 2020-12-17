using System;
using System.Globalization;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.Internal.Converters.Collections.Dictionaries
{
    internal sealed class UIntDictionaryDdbConverter<TValue> : DictionaryDdbConverterBase<uint, TValue>
    {
        public UIntDictionaryDdbConverter(DdbConverter<TValue> valueConverter) : base(valueConverter)
        {
        }

        protected override uint ParseValue(string value)
        {
            if (!uint.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedValue))
                throw new DdbException($"Couldn't parse '{nameof(UInt32)}' enum '{value}' value.");

            return parsedValue;
        }
    }
}