using System;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;

namespace EfficientDynamoDb.Internal.Converters.Collections.Dictionaries
{
    internal sealed class GuidDictionaryDdbConverter<TValue> : DictionaryDdbConverterBase<Guid, TValue>
    {
        public GuidDictionaryDdbConverter(DdbConverter<TValue> valueConverter) : base(valueConverter)
        {
        }

        protected override Guid ParseValue(string value)
        {
            if (!Guid.TryParse(value, out var parsedValue))
                throw new DdbException($"Couldn't parse '{nameof(Guid)}' enum '{value}' value.");

            return parsedValue;
        }

        protected override void WriteKeyName(Utf8JsonWriter writer, Guid value) => writer.WritePropertyName(value);
    }
}