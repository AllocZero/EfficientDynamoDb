using System;
using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;
using NotImplementedException = System.NotImplementedException;

namespace EfficientDynamoDb.Internal.Converters.Collections.Dictionaries
{
    internal sealed class StringDictionaryDdbConverter<TValue> : DdbConverter<Dictionary<string, TValue>>
    {
        private static readonly Type ElementTypeValue = typeof(string);
        
        internal override DdbClassType ClassType => DdbClassType.Dictionary;

        private readonly DdbConverter<TValue> _valueConverter;
        
        public override Type? ElementType => ElementTypeValue;

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
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref Dictionary<string, TValue> value)
        {
            writer.WritePropertyName(attributeName);
            writer.WriteStartObject();
            writer.WritePropertyName(DdbTypeNames.Map);
            writer.WriteStartObject();
            foreach (var pair in value)
            {
                var valueCopy = pair.Value;
                _valueConverter.Write(writer, pair.Key, ref valueCopy);
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        public override Dictionary<string, TValue> Read(ref DdbReader reader)
        {
            throw new NotSupportedException("Should never be called.");
        }
    }
}