using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections
{
    internal sealed class DictionaryDdbConverter<TValue> : DdbConverter<Dictionary<string, TValue>>
    {
        private readonly DdbConverter<TValue> _valueConverter;

        public DictionaryDdbConverter(DdbConverter<TValue> valueConverter)
        {
            _valueConverter = valueConverter;
        }

        public override Dictionary<string, TValue> Read(AttributeValue attributeValue)
        {
            var document = attributeValue.AsDocument();
            
            var dictionary = new Dictionary<string, TValue>(document.Count);
            
            foreach (var pair in document)
                dictionary.Add(pair.Key, _valueConverter.Read(pair.Value));

            return dictionary;
        }
    }
}