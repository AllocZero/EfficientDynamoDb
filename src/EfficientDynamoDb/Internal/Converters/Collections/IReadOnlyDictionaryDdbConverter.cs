using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class IReadOnlyDictionaryDdbConverter<TKey, TValue> : DictionaryDdbConverterBase<IReadOnlyDictionary<TKey, TValue>, TKey, TValue>
    {
        public IReadOnlyDictionaryDdbConverter(DynamoDbContextMetadata metadata) : base(metadata)
        {
        }

        protected override IReadOnlyDictionary<TKey, TValue> ToResult(Dictionary<TKey, TValue> dictionary) => dictionary;

        public override IReadOnlyDictionary<TKey, TValue> Read(in AttributeValue attributeValue)
        {
            var document = attributeValue.AsDocument();

            var dictionary = new Dictionary<TKey, TValue>(document.Count);

            foreach (var pair in document)
            {
                dictionary.Add(KeyConverter.Read(new AttributeValue(new StringAttributeValue(pair.Key))), ValueConverter.Read(pair.Value));
            }

            return dictionary;
        }

        public override AttributeValue Write(ref IReadOnlyDictionary<TKey, TValue> value)
        {
            var document = new Document(value.Count);

            foreach (var pair in value)
            {
                var pairKey = pair.Key;
                var pairValue = pair.Value;
                document.Add(KeyConverter.Write(ref pairKey).GetString(), ValueConverter.Write(ref pairValue));
            }

            return document;
        }

        public override void Write(Utf8JsonWriter writer, string attributeName, ref IReadOnlyDictionary<TKey, TValue> value)
        {
            writer.WritePropertyName(attributeName);

            WriteInlined(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref IReadOnlyDictionary<TKey, TValue> value) => WriteInlined(writer, ref value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(Utf8JsonWriter writer, ref IReadOnlyDictionary<TKey, TValue> value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(DdbTypeNames.Map);
            writer.WriteStartObject();
            foreach (var pair in value)
            {
                var keyCopy = pair.Key;
                var valueCopy = pair.Value;

                KeyDictionaryConverter.WritePropertyName(writer, ref keyCopy);
                ValueConverter.Write(writer, ref valueCopy);
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
        }
    }

    internal sealed class IReadOnlyDictionaryDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType || !typeToConvert.IsInterface)
                return false;

            var genericType = typeToConvert.GetGenericTypeDefinition();
            var isDictionary = genericType == typeof(IReadOnlyDictionary<,>);
            return isDictionary;
        }

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            var exactConverterType = typeof(IReadOnlyDictionaryDdbConverter<,>).MakeGenericType(typeToConvert.GenericTypeArguments[0], typeToConvert.GenericTypeArguments[1]);

            return (DdbConverter) Activator.CreateInstance(exactConverterType, metadata);
        }
    }
}