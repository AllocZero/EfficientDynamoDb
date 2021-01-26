using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;


namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class IReadOnlyListDdbConverter<T> : CollectionDdbConverter<IReadOnlyList<T>?, List<T>, T>
    {
        public IReadOnlyListDdbConverter(DdbConverter<T> elementConverter) : base(elementConverter)
        {
        }

        protected override void Add(List<T> collection, T item, int index) => collection.Add(item);

        protected override IReadOnlyList<T> ToResult(List<T> collection) => collection.ToArray();

        public override IReadOnlyList<T>? Read(in AttributeValue attributeValue)
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

        public override bool TryWrite(ref IReadOnlyList<T>? value, out AttributeValue attributeValue)
        {
            attributeValue = WriteInlined(ref value!);
            return true;
        }

        public override AttributeValue Write(ref IReadOnlyList<T>? value)
        {
            return value == null ? AttributeValue.Null : WriteInlined(ref value);
        }

        public override void Write(in DdbWriter writer, string attributeName, ref IReadOnlyList<T>? value)
        {
            writer.JsonWriter.WritePropertyName(attributeName);

            WriteInlined(in writer, ref value!);
        }

        public override void Write(in DdbWriter writer, ref IReadOnlyList<T>? value)
        {
            if (value == null)
            {
                writer.WriteDdbNull();
                return;
            }

            WriteInlined(in writer, ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private AttributeValue WriteInlined(ref IReadOnlyList<T> value)
        {
            var array = new AttributeValue[value.Count];

            for (var i = 0; i < value.Count; i++)
            {
                var valueCopy = value[i];
                array[i] = ElementConverter.Write(ref valueCopy);
            }

            return new AttributeValue(new ListAttributeValue(array));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(in DdbWriter writer, ref IReadOnlyList<T> value)
        {
            writer.JsonWriter.WriteStartObject();
            writer.JsonWriter.WritePropertyName("L");

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

    internal sealed class IReadOnlyListDdbConverterFactory : DdbConverterFactory
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
            var converterType = typeof(IReadOnlyListDdbConverter<>).MakeGenericType(elementType);

            return (DdbConverter) Activator.CreateInstance(converterType, metadata.GetOrAddConverter(elementType, null));
        }
    }
}