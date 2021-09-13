using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Converters.Collections
{
    public class IDictionaryDdbConverter<TKey, TValue> : DictionaryDdbConverterBase<IDictionary<TKey, TValue>?, TKey, TValue> where TKey : notnull
    {
        public IDictionaryDdbConverter(DynamoDbContextMetadata metadata) : base(metadata)
        {
        }

        protected IDictionaryDdbConverter(DdbConverter<TKey> keyConverter, DdbConverter<TValue> valueConverter) : base(keyConverter, valueConverter)
        {
        }

        protected sealed override IDictionary<TKey, TValue> ToResult(Dictionary<TKey, TValue> dictionary) => dictionary;

        public sealed override IDictionary<TKey, TValue> Read(in AttributeValue attributeValue)
        {
            var document = attributeValue.AsDocument();

            var dictionary = new Dictionary<TKey, TValue>(document.Count);

            foreach (var pair in document)
            {
                dictionary.Add(KeyConverter.Read(new AttributeValue(new StringAttributeValue(pair.Key))), ValueConverter.Read(pair.Value));
            }

            return dictionary;
        }

        public sealed override AttributeValue Write(ref IDictionary<TKey, TValue>? value)
        {
            return value == null ? AttributeValue.Null : WriteInlined(ref value);
        }

        public sealed override void Write(in DdbWriter writer, ref IDictionary<TKey, TValue>? value)
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
                var pairValue = pair.Value;
                if (pairValue is null || !ValueConverter.ShouldWrite(ref pairValue))
                    continue;

                var pairKey = pair.Key;
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
                var valueCopy = pair.Value;
                if (valueCopy is null || !ValueConverter.ShouldWrite(ref valueCopy))
                    continue;
                
                var keyCopy = pair.Key;

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