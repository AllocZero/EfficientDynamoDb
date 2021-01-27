using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class IReadOnlyCollectionDdbConverter<T> : CollectionDdbConverter<IReadOnlyCollection<T>?, List<T>, T>
    {
        public IReadOnlyCollectionDdbConverter(DdbConverter<T> elementConverter) : base(elementConverter)
        {
        }

        protected override void Add(List<T> collection, T item, int index) => collection.Add(item);

        protected override IReadOnlyCollection<T> ToResult(List<T> collection) => collection.ToArray();

        public override IReadOnlyCollection<T>? Read(in AttributeValue attributeValue)
        {
            if (attributeValue.IsNull)
                return null;
            
            var items = attributeValue.AsListAttribute().Items;
            var entities = new T[items.Count];

            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                entities[i] = ElementConverter.Read(in item);
            }

            return entities;
        }

        public override bool TryWrite(ref IReadOnlyCollection<T>? value, out AttributeValue attributeValue)
        {
            attributeValue = WriteInlined(ref value!);
            return true;
        }

        public override AttributeValue Write(ref IReadOnlyCollection<T>? value)
        {
            return value == null ? AttributeValue.Null : WriteInlined(ref value);
        }

        public override void Write(in DdbWriter writer, string attributeName, ref IReadOnlyCollection<T>? value)
        {
            writer.JsonWriter.WritePropertyName(attributeName);

            WriteInlined(in writer, ref value!);
        }

        public override void Write(in DdbWriter writer, ref IReadOnlyCollection<T>? value)
        {
            if (value == null)
            {
                writer.WriteDdbNull();
                return;
            }

            WriteInlined(in writer, ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private AttributeValue WriteInlined(ref IReadOnlyCollection<T> value)
        {
            var list = new List<AttributeValue>(value.Count);

            var i = 0;
            foreach (var item in value)
            {
                var itemCopy = item;
                list[i++] = ElementConverter.Write(ref itemCopy);
            }

            return new ListAttributeValue(list);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(in DdbWriter writer, ref IReadOnlyCollection<T> value)
        {
            writer.JsonWriter.WriteStartObject();
            writer.JsonWriter.WritePropertyName(DdbTypeNames.List);

            writer.JsonWriter.WriteStartArray();

            foreach (var item in value)
            {
                var itemCopy = item;
                ElementConverter.Write(in writer, ref itemCopy);
            }

            writer.JsonWriter.WriteEndArray();
            writer.JsonWriter.WriteEndObject();
        }
    }

    internal sealed class IReadOnlyCollectionDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType || !typeToConvert.IsInterface)
                return false;

            var genericType = typeToConvert.GetGenericTypeDefinition();
            return genericType == typeof(IReadOnlyCollection<>);
        }

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            var elementType = typeToConvert.GetElementType()!;
            var converterType = typeof(IReadOnlyCollectionDdbConverter<>).MakeGenericType(elementType);

            return (DdbConverter) Activator.CreateInstance(converterType, metadata.GetOrAddConverter(elementType, null));
        }
    }
}