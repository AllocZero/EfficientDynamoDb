using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Collections.Dictionaries
{
    internal abstract class DictionaryDdbConverterBase<TKey, TValue> : DdbConverter<Dictionary<TKey, TValue>> where TKey : struct
    {
        protected DdbConverter<TValue> ValueConverter { get; }

        protected abstract TKey ParseValue(string value);

        protected abstract void WriteKeyName(Utf8JsonWriter writer, TKey value);

        public DictionaryDdbConverterBase(DdbConverter<TValue> valueConverter)
        {
            ValueConverter = valueConverter;
        }

        public override Dictionary<TKey, TValue> Read(in AttributeValue attributeValue)
        {
            var document = attributeValue.AsDocument();
            
            var dictionary = new Dictionary<TKey, TValue>(document.Count);

            foreach (var pair in document)
            {
                var value = ParseValue(pair.Key);
                dictionary.Add(value, ValueConverter.Read(pair.Value));
            }

            return dictionary;
        }

        public override AttributeValue Write(ref Dictionary<TKey, TValue> value)
        {
            var document = new Document(value.Count);

            foreach (var pair in value)
            {
                var pairValue = pair.Value;
                document.Add(pair.Key.ToString(), ValueConverter.Write(ref pairValue));
            }

            return document;
        }

        public override void Write(Utf8JsonWriter writer, string attributeName, ref Dictionary<TKey, TValue> value)
        {
            writer.WritePropertyName(attributeName);
            writer.WriteStartObject();
            writer.WritePropertyName(DdbTypeNames.Map);
            writer.WriteStartObject();
            foreach (var pair in value)
            {
                var valueCopy = pair.Value;
                WriteKeyName(writer, pair.Key);
                ValueConverter.Write(writer, ref valueCopy);
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }
}