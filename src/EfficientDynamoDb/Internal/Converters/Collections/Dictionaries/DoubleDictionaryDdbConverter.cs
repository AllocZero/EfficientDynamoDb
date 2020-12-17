using System;
using System.Globalization;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.Internal.Converters.Collections.Dictionaries
{
    internal sealed class DoubleDictionaryDdbConverter<TValue> : DictionaryDdbConverterBase<double, TValue>
    {
        public DoubleDictionaryDdbConverter(DdbConverter<TValue> valueConverter) : base(valueConverter)
        {
        }

        protected override double ParseValue(string value)
        {
            if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedValue))
                throw new DdbException($"Couldn't parse '{nameof(Double)}' enum '{value}' value.");

            return parsedValue;
        }
    }
}