using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class IDictionaryDdbConverter<TKey, TValue> : DictionaryDdbConverterBase<IDictionary<TKey, TValue>?, TKey, TValue> where TKey : notnull
    {
        public IDictionaryDdbConverter(DynamoDbContextMetadata metadata) : base(metadata)
        {
        }

        protected override IDictionary<TKey, TValue> ToResult(Dictionary<TKey, TValue> dictionary) => dictionary;

        public override IDictionary<TKey, TValue> Read(in AttributeValue attributeValue)
        {
            var document = attributeValue.AsDocument();

            var dictionary = new Dictionary<TKey, TValue>(document.Count);

            foreach (var pair in document)
            {
                dictionary.Add(KeyConverter.Read(new AttributeValue(new StringAttributeValue(pair.Key))), ValueConverter.Read(pair.Value));
            }

            return dictionary;
        }

        public override AttributeValue Write(ref IDictionary<TKey, TValue>? value)
        {
            return value == null ? AttributeValue.Null : WriteInlined(ref value);
        }

        public override void Write(in DdbWriter writer, ref IDictionary<TKey, TValue>? value)
        {
            if (value == null)
            {
                writer.WriteDdbNull();
                return;
            }

            WriteInlined(in writer, ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private AttributeValue WriteInlined(ref IDictionary<TKey, TValue> value)
        {
            var document = new Document(value.Count);

            foreach (var pair in value)
            {
                var pairKey = pair.Key;
                var pairValue = pair.Value;
                document.Add(KeyDictionaryConverter.WriteStringValue(ref pairKey), ValueConverter.Write(ref pairValue));
            }

            return document;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(in DdbWriter writer, ref IDictionary<TKey, TValue> value)
        {
            writer.JsonWriter.WriteStartObject();
            writer.JsonWriter.WritePropertyName(DdbTypeNames.Map);
            writer.JsonWriter.WriteStartObject();
            foreach (var pair in value)
            {
                var keyCopy = pair.Key;
                var valueCopy = pair.Value;

                KeyDictionaryConverter.WritePropertyName(in writer, ref keyCopy);
                ValueConverter.Write(in writer, ref valueCopy);
            }

            writer.JsonWriter.WriteEndObject();
            writer.JsonWriter.WriteEndObject();
        }
    }

    internal sealed class IDictionaryDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType || !typeToConvert.IsInterface)
                return false;

            var genericType = typeToConvert.GetGenericTypeDefinition();
            var isDictionary = genericType == typeof(IDictionary<,>);
            return isDictionary;
        }

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            var exactConverterType = typeof(IDictionaryDdbConverter<,>).MakeGenericType(typeToConvert.GenericTypeArguments[0], typeToConvert.GenericTypeArguments[1]);

            return (DdbConverter) Activator.CreateInstance(exactConverterType, metadata)!;
        }
    }
}