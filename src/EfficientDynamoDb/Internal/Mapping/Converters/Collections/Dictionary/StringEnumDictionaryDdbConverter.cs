using System;
using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.TypeParsers;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.Dictionary
{
    internal sealed class StringEnumDictionaryDdbConverter<TEnum, TValue> : DdbConverter<Dictionary<TEnum, TValue>> where TEnum : struct, Enum
    {
        private readonly DdbConverter<TValue> _valueConverter;

        public StringEnumDictionaryDdbConverter(DdbConverter<TValue> valueConverter)
        {
            _valueConverter = valueConverter;
        }

        public override Dictionary<TEnum, TValue> Read(in AttributeValue attributeValue)
        {
            var document = attributeValue.AsDocument();
            
            var dictionary = new Dictionary<TEnum, TValue>(document.Count);

            foreach (var pair in document)
            {
                if (!EnumParser.TryParseCaseInsensitive(pair.Key, out TEnum enumValue))
                    throw new DdbException($"Couldn't parse '{typeof(TEnum).Name}' enum '{pair.Key}' value.");
                
                dictionary.Add(enumValue, _valueConverter.Read(pair.Value));
            }

            return dictionary;
        }

        public override AttributeValue Write(ref Dictionary<TEnum, TValue> value)
        {
            var document = new Document(value.Count);

            foreach (var pair in value)
            {
                var pairValue = pair.Value;
                document.Add(pair.Key.ToString(), _valueConverter.Write(ref pairValue));
            }

            return document;
        }
    }
}