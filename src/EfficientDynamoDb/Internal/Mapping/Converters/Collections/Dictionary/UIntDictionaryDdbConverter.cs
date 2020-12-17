using System;
using System.Collections.Generic;
using System.Globalization;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.Dictionary
{
    internal sealed class UIntDictionaryDdbConverter<TValue> : DdbConverter<Dictionary<uint, TValue>>
    {
        private readonly DdbConverter<TValue> _valueConverter;

        public UIntDictionaryDdbConverter(DdbConverter<TValue> valueConverter)
        {
            _valueConverter = valueConverter;
        }

        public override Dictionary<uint, TValue> Read(in AttributeValue attributeValue)
        {
            var document = attributeValue.AsDocument();
            
            var dictionary = new Dictionary<uint, TValue>(document.Count);

            foreach (var pair in document)
            {
                if (!uint.TryParse(pair.Key, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
                    throw new DdbException($"Couldn't parse '{nameof(UInt32)}' enum '{pair.Key}' value.");
                dictionary.Add(value, _valueConverter.Read(pair.Value));
            }

            return dictionary;
        }

        public override AttributeValue Write(ref Dictionary<uint, TValue> value)
        {
            var document = new Document(value.Count);

            foreach (var pair in value)
            {
                var pairValue = pair.Value;
                document.Add(pair.Key.ToString(CultureInfo.InvariantCulture), _valueConverter.Write(ref pairValue));
            }

            return document;
        }
    }
}