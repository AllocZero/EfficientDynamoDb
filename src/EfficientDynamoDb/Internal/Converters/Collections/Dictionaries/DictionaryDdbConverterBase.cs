using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;

namespace EfficientDynamoDb.Internal.Converters.Collections.Dictionaries
{
    internal abstract class DictionaryDdbConverterBase<TKey, TValue> : DdbConverter<Dictionary<TKey, TValue>> where TKey : struct
    {
        private readonly DdbConverter<TValue> _valueConverter;

        protected abstract TKey ParseValue(string value);

        public DictionaryDdbConverterBase(DdbConverter<TValue> valueConverter)
        {
            _valueConverter = valueConverter;
        }

        public override Dictionary<TKey, TValue> Read(in AttributeValue attributeValue)
        {
            var document = attributeValue.AsDocument();
            
            var dictionary = new Dictionary<TKey, TValue>(document.Count);

            foreach (var pair in document)
            {
                var value = ParseValue(pair.Key);
                dictionary.Add(value, _valueConverter.Read(pair.Value));
            }

            return dictionary;
        }

        public override AttributeValue Write(ref Dictionary<TKey, TValue> value)
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