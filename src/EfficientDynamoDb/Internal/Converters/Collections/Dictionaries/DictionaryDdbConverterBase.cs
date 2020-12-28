using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Collections.Dictionaries
{
    internal abstract class DictionaryDdbConverterBase<TKey, TValue> : DdbConverter<Dictionary<TKey, TValue>> where TKey : struct
    {
        private static readonly Type ElementTypeValue = typeof(TValue);
        
        internal override DdbClassType ClassType => DdbClassType.Dictionary;

        public override Type? ElementType => ElementTypeValue;
        
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

        public override Dictionary<TKey, TValue> Read(ref DdbReader reader)
        {
            throw new NotSupportedException("Should never be called.");
        }
    }

    internal sealed class DictionaryDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType || (!typeToConvert.IsClass && !typeToConvert.IsInterface))
                return false;

            var genericType = typeToConvert.GetGenericTypeDefinition();
            var isDictionary = genericType == typeof(Dictionary<,>) || genericType == typeof(IDictionary<,>) || genericType == typeof(IReadOnlyDictionary<,>);
            return isDictionary;
        }

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            var dictionaryConverterType = GetDictionaryConverterType(typeToConvert.GenericTypeArguments[0]);
            
            var exactConverterType = dictionaryConverterType.MakeGenericType(typeToConvert.GenericTypeArguments[0], typeToConvert.GenericTypeArguments[1]);

            return (DdbConverter) Activator.CreateInstance(exactConverterType, metadata.GetOrAddConverter(typeToConvert.GenericTypeArguments[1], null));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Type GetDictionaryConverterType(Type keyType) => keyType switch
        {
            _ when keyType == typeof(string) => typeof(StringDictionaryDdbConverter<>),
            _ when keyType == typeof(int) => typeof(IntDictionaryDdbConverter<>),
            _ when keyType == typeof(uint) => typeof(UIntDictionaryDdbConverter<>),
            _ when keyType == typeof(long) => typeof(LongDictionaryDdbConverter<>),
            _ when keyType == typeof(ulong) => typeof(ULongDictionaryDdbConverter<>),
            _ when keyType == typeof(short) => typeof(ShortDictionaryDdbConverter<>),
            _ when keyType == typeof(ushort) => typeof(UShortDictionaryDdbConverter<>),
            _ when keyType == typeof(byte) => typeof(ByteDictionaryDdbConverter<>),
            _ when keyType == typeof(float) => typeof(FloatDictionaryDdbConverter<>),
            _ when keyType == typeof(double) => typeof(DoubleDictionaryDdbConverter<>),
            _ when keyType == typeof(decimal) => typeof(DecimalDictionaryDdbConverter<>),
            _ when keyType == typeof(Guid) => typeof(GuidDictionaryDdbConverter<>),
            _ when keyType.IsEnum => typeof(StringEnumDictionaryDdbConverter<,>),
            _ => throw new DdbException($"Type '{keyType.Name}' requires an explicit ddb converter.")
        };

    }
}