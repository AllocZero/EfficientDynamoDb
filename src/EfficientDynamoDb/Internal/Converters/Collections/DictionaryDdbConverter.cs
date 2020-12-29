using System;
using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class DictionaryDdbConverter<TKey, TValue> : DdbResumableConverter<Dictionary<TKey, TValue>> where TKey : struct
    {
        private static readonly Type ElementTypeValue = typeof(TValue);
        
        internal override DdbClassType ClassType => DdbClassType.Dictionary;

        public override Type? ElementType => ElementTypeValue;

        private readonly DdbConverter<TKey> _keyConverter;
        private readonly IDictionaryKeyConverter<TKey> _keyDictionaryConverter;
        private readonly DdbConverter<TValue> _valueConverter;

        public DictionaryDdbConverter(DynamoDbContextMetadata metadata)
        {
            _keyConverter = metadata.GetOrAddConverter<TKey>();
            _valueConverter = metadata.GetOrAddConverter<TValue>();
            _keyDictionaryConverter = _keyConverter as IDictionaryKeyConverter<TKey> ??
                                      throw new DdbException($"{_keyConverter.GetType().Name} must implement IDictionaryKeyConverter in order to store value as a dictionary key.");
        }

        public override Dictionary<TKey, TValue> Read(in AttributeValue attributeValue)
        {
            var document = attributeValue.AsDocument();
            
            var dictionary = new Dictionary<TKey, TValue>(document.Count);

            foreach (var pair in document)
            {
                dictionary.Add(_keyConverter.Read(new AttributeValue(new StringAttributeValue(pair.Key))), _valueConverter.Read(pair.Value));
            }

            return dictionary;
        }

        public override AttributeValue Write(ref Dictionary<TKey, TValue> value)
        {
            var document = new Document(value.Count);

            foreach (var pair in value)
            {
                var pairKey = pair.Key;
                var pairValue = pair.Value;
                document.Add(_keyConverter.Write(ref pairKey).GetString(), _valueConverter.Write(ref pairValue));
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
                var keyCopy = pair.Key;
                var valueCopy = pair.Value;
                
                _keyDictionaryConverter.WritePropertyName(writer, ref keyCopy);
                _valueConverter.Write(writer, ref valueCopy);
            }
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
        
        // TODO: Implement TryRead
    }

    internal sealed class DictionaryDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType || (!typeToConvert.IsClass && !typeToConvert.IsInterface))
                return false;

            var genericType = typeToConvert.GetGenericTypeDefinition();
            var isDictionary = genericType == typeof(Dictionary<,>);
            return isDictionary;
        }

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            var exactConverterType = typeof(DictionaryDdbConverter<,>).MakeGenericType(typeToConvert.GenericTypeArguments[0], typeToConvert.GenericTypeArguments[1]);

            return (DdbConverter) Activator.CreateInstance(exactConverterType, metadata);
        }
    }
}