using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class ArrayDdbConverter<T> : CollectionDdbConverter<T[]?, List<T>, T>
    {
        public ArrayDdbConverter(DdbConverter<T> elementConverter) : base(elementConverter)
        {
        }

        protected override void Add(List<T> collection, T item, int index) => collection.Add(item);

        protected override T[] ToResult(List<T> collection) => collection.ToArray();

        public override T[]? Read(in AttributeValue attributeValue)
        {
            if (attributeValue.IsNull)
                return null;
            
            var items = attributeValue.AsListAttribute().Items;
            var entities = new T[items.Length];

            for (var i = 0; i < items.Length; i++)
            {
                ref var item = ref items[i];
                entities[i] = ElementConverter.Read(in item);
            }

            return entities;
        }

        public override bool TryWrite(ref T[]? value, out AttributeValue attributeValue)
        {
            attributeValue = WriteInlined(ref value!);
            return true;
        }

        public override AttributeValue Write(ref T[]? value)
        {
            return value == null ? AttributeValue.Null : WriteInlined(ref value);
        }

        public override void Write(in DdbWriter writer, string attributeName, ref T[]? value)
        {
            writer.JsonWriter.WritePropertyName(attributeName);

            WriteInlined(in writer, ref value!);
        }

        public override void Write(in DdbWriter writer, ref T[]? value)
        {
            if (value == null)
            {
                writer.WriteDdbNull();
                return;
            }

            WriteInlined(in writer, ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private AttributeValue WriteInlined(ref T[] value)
        {
            var array = new AttributeValue[value.Length];

            for (var i = 0; i < value.Length; i++)
                array[i] = ElementConverter.Write(ref value[i]);

            return new AttributeValue(new ListAttributeValue(array));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(in DdbWriter ddbWriter, ref T[] value)
        {
            ddbWriter.JsonWriter.WriteStartObject();
            ddbWriter.JsonWriter.WritePropertyName("L");

            ddbWriter.JsonWriter.WriteStartArray();

            for (var i = 0; i < value!.Length; i++)
                ElementConverter.Write(in ddbWriter, ref value[i]);

            ddbWriter.JsonWriter.WriteEndArray();
            ddbWriter.JsonWriter.WriteEndObject();
        }
    }

    internal sealed class ArrayDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsArray;

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            var elementType =  typeToConvert.GetElementType()!;
            var converterType = typeof(ArrayDdbConverter<>).MakeGenericType(elementType);

            return (DdbConverter) Activator.CreateInstance(converterType, metadata.GetOrAddConverter(elementType, null));
        }
    }
}