using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class NumberSetDdbConverter<T> : SetDdbConverter<HashSet<T>, T> where T : struct
    {
        public NumberSetDdbConverter(DynamoDbContextMetadata metadata) : base(metadata)
        {
        }
        
        protected override HashSet<T> CreateSet() => new HashSet<T>();

        public override HashSet<T> Read(in AttributeValue attributeValue)
        {
            var values = attributeValue.AsNumberSetAttribute().Items;
            var set = new HashSet<T>(values.Length);

            foreach (var value in values)
                set.Add(ElementConverter.Read(new AttributeValue(new NumberAttributeValue(value))));

            return set;
        }

        public override AttributeValue Write(ref HashSet<T> value)
        {
            var array = new string[value.Count];

            var i = 0;
            foreach (var item in value)
            {
                var copy = item;
                array[i++] = ElementConverter.Write(ref copy).GetString();
            }
            
            return new NumberSetAttributeValue(array);
        }
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref HashSet<T> value)
        {
            writer.WritePropertyName(attributeName);
            
            WriteInlined(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref HashSet<T> value) => WriteInlined(writer, ref value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(Utf8JsonWriter writer, ref HashSet<T> value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(DdbTypeNames.NumberSet);
            
            writer.WriteStartArray();

            foreach (var item in value)
            {
                var itemCopy = item;
                ElementSetValueConverter.WriteStringValue(writer, ref itemCopy);
            }
            
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }

    internal sealed class NumberSetDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType || !typeToConvert.IsClass)
                return false;
            
            var genericType = typeToConvert.GetGenericTypeDefinition();
            var isSet = genericType == typeof(HashSet<>);
            if (!isSet)
                return false;

            var elementType = typeToConvert.GenericTypeArguments[0];
            switch (elementType)
            {
                case var _ when elementType == typeof(int):
                case var _ when elementType == typeof(uint):
                case var _ when elementType == typeof(long):
                case var _ when elementType == typeof(ulong):
                case var _ when elementType == typeof(short):
                case var _ when elementType == typeof(ushort):
                case var _ when elementType == typeof(byte):
                case var _ when elementType == typeof(float):
                case var _ when elementType == typeof(double):
                case var _ when elementType == typeof(decimal):
                    return true;
                default:
                    return false;
            }
        }

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            return (DdbConverter) Activator.CreateInstance(typeof(NumberSetDdbConverter<>).MakeGenericType(typeToConvert.GenericTypeArguments[0]), metadata);
        }
    }
}