using System;
using System.Collections.Generic;
using System.Globalization;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.Dictionary
{
    internal sealed class FloatDictionaryDdbConverter<TValue> : DdbConverter<Dictionary<float, TValue>>
    {
        private readonly DdbConverter<TValue> _valueConverter;

        public FloatDictionaryDdbConverter(DdbConverter<TValue> valueConverter)
        {
            _valueConverter = valueConverter;
        }

        public override Dictionary<float, TValue> Read(in AttributeValue attributeValue)
        {
            var document = attributeValue.AsDocument();
            
            var dictionary = new Dictionary<float, TValue>(document.Count);

            foreach (var pair in document)
            {
                if (!float.TryParse(pair.Key, NumberStyles.Float, CultureInfo.InvariantCulture, out var value))
                    throw new DdbException($"Couldn't parse '{nameof(Single)}' enum '{pair.Key}' value.");
                dictionary.Add(value, _valueConverter.Read(pair.Value));
            }

            return dictionary;
        }

        public override AttributeValue Write(ref Dictionary<float, TValue> value)
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