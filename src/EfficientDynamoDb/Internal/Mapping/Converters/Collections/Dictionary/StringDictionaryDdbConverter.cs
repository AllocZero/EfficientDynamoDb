using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Collections.Dictionary
{
    internal sealed class StringDictionaryDdbConverter<TValue> : DdbConverter<Dictionary<string, TValue>>
    {
        private readonly DdbConverter<TValue> _valueConverter;

        public StringDictionaryDdbConverter(DdbConverter<TValue> valueConverter)
        {
            _valueConverter = valueConverter;
        }

        public override Dictionary<string, TValue> Read(in AttributeValue attributeValue)
        {
            var document = attributeValue.AsDocument();
            
            var dictionary = new Dictionary<string, TValue>(document.Count);
            
            foreach (var pair in document)
                dictionary.Add(pair.Key, _valueConverter.Read(pair.Value));

            return dictionary;
        }

        public override AttributeValue Write(ref Dictionary<string, TValue> value)
        {
            var document = new Document(value.Count);

            foreach (var pair in value)
            {
                var pairValue = pair.Value;
                document.Add(pair.Key, _valueConverter.Write(ref pairValue));
            }

            return document;
        }
    }
}