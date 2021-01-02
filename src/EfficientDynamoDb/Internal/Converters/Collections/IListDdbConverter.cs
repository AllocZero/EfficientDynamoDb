using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
   internal sealed class IListDdbConverter<T> : CollectionDdbConverter<IList<T>, List<T>, T>
    {
        public IListDdbConverter(DdbConverter<T> elementConverter) : base(elementConverter)
        {
        }
        
        protected override void Add(List<T> collection, T item, int index) => collection.Add(item);

        protected override IList<T> ToResult(List<T> collection) => collection;

        public override IList<T> Read(in AttributeValue attributeValue)
        {
            var items = attributeValue.AsListAttribute().Items;
            var entities = new List<T>(items.Length);

            foreach (var item in items)
                entities.Add(ElementConverter.Read(in item));

            return entities;
        }

        public override AttributeValue Write(ref IList<T> value)
        {
            var array = new AttributeValue[value.Count];

            for (var i = 0; i < value.Count; i++)
            {
                var item = value[i];
                array[i] = ElementConverter.Write(ref item);
            }
            
            return new ListAttributeValue(array);
        }

        public override void Write(Utf8JsonWriter writer, string attributeName, ref IList<T> value)
        {
            writer.WritePropertyName(attributeName);

            WriteInlined(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref IList<T> value) => WriteInlined(writer, ref value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(Utf8JsonWriter writer, ref IList<T> value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("L");
            
            writer.WriteStartArray();

            foreach (var item in value)
            {
                var itemCopy = item;
                ElementConverter.Write(writer, ref itemCopy);
            }
            
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }

    internal sealed class IListDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType || !typeToConvert.IsInterface)
                return false;

            var genericType = typeToConvert.GetGenericTypeDefinition();
            return genericType == typeof(IList<>);
        }

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            var elementType = typeToConvert.GenericTypeArguments[0];
            var converterType = typeof(IListDdbConverter<>).MakeGenericType(elementType);

            return (DdbConverter) Activator.CreateInstance(converterType, metadata.GetOrAddConverter(elementType, null));
        }
    }
}