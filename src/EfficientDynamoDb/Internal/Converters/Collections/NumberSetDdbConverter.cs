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

        public override HashSet<T>? Read(in AttributeValue attributeValue)
        {
            if (attributeValue.IsNull)
                return null;
            
            var values = attributeValue.AsNumberSetAttribute().Items;
            var set = new HashSet<T>(values.Count);

            foreach (var value in values)
                set.Add(ElementConverter.Read(new AttributeValue(new NumberAttributeValue(value))));

            return set;
        }

        public override bool TryWrite(ref HashSet<T>? value, out AttributeValue attributeValue)
        {
            attributeValue = WriteInlined(ref value!);
            return true;
        }

        public override AttributeValue Write(ref HashSet<T>? value)
        {
            return value == null ? AttributeValue.Null : WriteInlined(ref value!);
        }

        public override void Write(in DdbWriter writer, string attributeName, ref HashSet<T>? value)
        {
            writer.JsonWriter.WritePropertyName(attributeName);
            
            WriteInlined(in writer, ref value!);
        }

        public override void Write(in DdbWriter writer, ref HashSet<T>? value)
        {
            if (value == null)
            {
                writer.WriteDdbNull();
                return;
            }

            WriteInlined(in writer, ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private AttributeValue WriteInlined(ref HashSet<T> value)
        {
            var set = new HashSet<string>(value.Count);

            foreach (var item in value)
            {
                var copy = item;
                set.Add(ElementConverter.Write(ref copy).GetString());
            }

            return new NumberSetAttributeValue(set);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(in DdbWriter writer, ref HashSet<T> value)
        {
            writer.JsonWriter.WriteStartObject();
            writer.JsonWriter.WritePropertyName(DdbTypeNames.NumberSet);
            
            writer.JsonWriter.WriteStartArray();

            foreach (var item in value)
            {
                var itemCopy = item;
                ElementSetValueConverter.WriteStringValue(in writer, ref itemCopy);
            }
            
            writer.JsonWriter.WriteEndArray();
            writer.JsonWriter.WriteEndObject();
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