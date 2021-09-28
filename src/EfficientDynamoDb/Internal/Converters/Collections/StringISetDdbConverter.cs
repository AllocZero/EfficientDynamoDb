using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class StringISetDdbConverter<T> : SetDdbConverter<ISet<T>, T>
    {
        private readonly bool _isString;
        
        public StringISetDdbConverter(DynamoDbContextMetadata metadata) : base(metadata)
        {
            _isString = typeof(T) == typeof(string);
        }

        protected override ISet<T> CreateSet() => new HashSet<T>();

        public override ISet<T>? Read(in AttributeValue attributeValue)
        {
            if (attributeValue.IsNull)
                return null;
            
            if (ElementConverter.IsInternal && _isString)
                return attributeValue.AsStringSetAttribute().Items as ISet<T>;
            
            var values = attributeValue.AsStringSetAttribute().Items;
            var set = new HashSet<T>(values.Count);

            foreach (var value in values)
                set.Add(ElementConverter.Read(new AttributeValue(new StringAttributeValue(value))));

            return set;
        }

        public override AttributeValue Write(ref ISet<T>? value)
        {
            if (value == null)
                return AttributeValue.Null;
            
            if (ElementConverter.IsInternal && value is HashSet<string> hashSetValue)
                return new StringSetAttributeValue(hashSetValue);

            var set = new HashSet<string>(value.Count);

            foreach (var item in value)
            {
                var copy = item;
                set.Add(ElementSetValueConverter.WriteStringValue(ref copy));
            }

            return new StringSetAttributeValue(set);
        }

        public override void Write(in DdbWriter writer, ref ISet<T>? value)
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

    internal sealed class StringISetDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType || !typeToConvert.IsInterface)
                return false;
            
            var genericType = typeToConvert.GetGenericTypeDefinition();
            return genericType == typeof(ISet<>);
        }

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            return (DdbConverter) Activator.CreateInstance(typeof(StringISetDdbConverter<>).MakeGenericType(typeToConvert.GenericTypeArguments[0]), metadata)!;
        }
    }
}