using System;
using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Converters.Collections;

namespace EfficientDynamoDb.Converters.Collections
{
    public sealed class SdkEnumHashSetConverter<TEnum> : CollectionDdbConverter<HashSet<TEnum>?,HashSet<TEnum>, TEnum> where TEnum : Enum
    {
        public SdkEnumHashSetConverter(DynamoDbContextMetadata metadata) : base(metadata.GetOrAddConverter<TEnum>())
        {
        }

        public override HashSet<TEnum>? Read(in AttributeValue attributeValue)
        {
            if (attributeValue.IsNull)
                return null;
            
            var items = attributeValue.AsListAttribute().Items;
            var entities = new HashSet<TEnum>(items.Count);

            foreach (var item in items)
                entities.Add(ElementConverter.Read(in item));

            return entities;
        }

        public override AttributeValue Write(ref HashSet<TEnum>? value)
        {
            if (value == null)
                return AttributeValue.Null;
            
            var list = new List<AttributeValue>(value.Count);

            foreach (var item in value)
            {
                var localItem = item;
                list.Add(ElementConverter.Write(ref localItem));
            }

            return new AttributeValue(new ListAttributeValue(list));
        }

        protected override void Add(HashSet<TEnum> collection, TEnum item, int index) => collection.Add(item);

        protected override HashSet<TEnum> ToResult(HashSet<TEnum> collection) => collection;
    }

    public sealed class SdkEnumHashSetConverterFactory : DdbConverterFactory
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
            return elementType.IsEnum;
        }

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            return (DdbConverter) Activator.CreateInstance(typeof(SdkEnumHashSetConverter<>).MakeGenericType(typeToConvert.GenericTypeArguments[0]), metadata)!;
        }
    }
}