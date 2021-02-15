using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
   internal sealed class IListDdbConverter<T> : CollectionDdbConverter<IList<T>?, List<T>, T>
    {
        public IListDdbConverter(DdbConverter<T> elementConverter) : base(elementConverter)
        {
        }
        
        protected override void Add(List<T> collection, T item, int index) => collection.Add(item);

        protected override IList<T> ToResult(List<T> collection) => collection;

        public override IList<T>? Read(in AttributeValue attributeValue)
        {
            if (attributeValue.IsNull)
                return null;
            
            var items = attributeValue.AsListAttribute().Items;
            var entities = new List<T>(items.Count);

            foreach (var item in items)
                entities.Add(ElementConverter.Read(in item));

            return entities;
        }

        public override AttributeValue Write(ref IList<T>? value)
        {
            return value == null ? AttributeValue.Null : WriteInlined(ref value);
        }

        public override void Write(in DdbWriter writer, ref IList<T>? value)
        {            
            if (value == null)
            {
                writer.WriteDdbNull();
                return;
            }

            WriteInlined(in writer, ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private AttributeValue WriteInlined(ref IList<T> value)
        {
            var list = new List<AttributeValue>(value.Count);

            foreach (var item in value)
            {
                var localItem = item;
                list.Add(ElementConverter.Write(ref localItem));
            }

            return new AttributeValue(new ListAttributeValue(list));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(in DdbWriter writer, ref IList<T> value)
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