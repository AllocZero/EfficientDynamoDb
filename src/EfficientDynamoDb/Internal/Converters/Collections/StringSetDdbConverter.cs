using System;
using System.Collections.Generic;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class StringSetDdbConverter<T> : SetDdbConverter<HashSet<T>, T>
    {
        private readonly bool _isString;
        
        public StringSetDdbConverter(DynamoDbContextMetadata metadata) : base(metadata)
        {
            _isString = typeof(T) == typeof(string);
        }

        protected override HashSet<T> CreateSet() => new HashSet<T>();

        public override HashSet<T>? Read(in AttributeValue attributeValue)
        {
            if (attributeValue.IsNull)
                return null;

            if (ElementConverter.IsInternal && _isString)
                return attributeValue.AsStringSetAttribute().Items as HashSet<T>;

            var values = attributeValue.AsStringSetAttribute().Items;
            var set = new HashSet<T>(values.Count);

            foreach (var value in values)
                set.Add(ElementConverter.Read(new AttributeValue(new StringAttributeValue(value))));

            return set;
        }

        public override AttributeValue Write(ref HashSet<T>? value)
        {
            if (value == null)
                return AttributeValue.Null;

            if (ElementConverter.IsInternal && _isString)
                return new StringSetAttributeValue((value as HashSet<string>)!);

            var set = new HashSet<string>(value.Count);

            foreach (var item in value)
            {
                var copy = item;
                set.Add(ElementSetValueConverter.WriteStringValue(ref copy));
            }

            return new StringSetAttributeValue(set);
        }

        public override void Write(in DdbWriter writer, ref HashSet<T>? value)
        {
            if (value == null)
            {
                writer.WriteDdbNull();
                return;
            }

            writer.JsonWriter.WriteStartObject();
            writer.JsonWriter.WritePropertyName(DdbTypeNames.StringSet);
            
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

    internal sealed class StringSetDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType || !typeToConvert.IsClass)
                return false;
            
            var genericType = typeToConvert.GetGenericTypeDefinition();
            return genericType == typeof(HashSet<>);
        }

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            return (DdbConverter) Activator.CreateInstance(typeof(StringSetDdbConverter<>).MakeGenericType(typeToConvert.GenericTypeArguments[0]), metadata)!;
        }
    }
}