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
    internal sealed class StringISetDdbConverter : SetDdbConverter<ISet<string>, string>
    {
        public StringISetDdbConverter(DynamoDbContextMetadata metadata) : base(metadata)
        {
        }

        protected override ISet<string> CreateSet() => new HashSet<string>();

        public override ISet<string> Read(in AttributeValue attributeValue)
        {
            if(ElementConverter.IsInternal)
                return attributeValue.AsStringSetAttribute().Items;
            
            var values = attributeValue.AsStringSetAttribute().Items;
            var set = new HashSet<string>(values.Count);

            if (ElementConverter.IsInternal)
            {
                foreach (var value in values)
                    set.Add(value);
            }
            else
            {
                foreach (var value in values)
                    set.Add(ElementConverter.Read(new AttributeValue(new StringAttributeValue(value))));
            }

            return set;
        }

        public override AttributeValue Write(ref ISet<string> value)
        {
            if(ElementConverter.IsInternal && value is HashSet<string> hashSetValue)
                return new StringSetAttributeValue(hashSetValue);
            
            var set = new HashSet<string>(value.Count);

            foreach (var item in value)
            {
                var copy = item;
                set.Add(ElementConverter.Write(ref copy).GetString());
            }
            
            return new StringSetAttributeValue(set);
        }

        public override void Write(Utf8JsonWriter writer, string attributeName, ref ISet<string> value)
        {
            writer.WritePropertyName(attributeName);
            
            WriteInlined(writer, ref value);
        }

        public override void Write(Utf8JsonWriter writer, ref ISet<string> value) => WriteInlined(writer, ref value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(Utf8JsonWriter writer, ref ISet<string> value)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(DdbTypeNames.StringSet);
            
            writer.WriteStartArray();

            if (ElementConverter.IsInternal)
            {
                foreach (var item in value)
                    writer.WriteStringValue(item);
            }
            else
            {
                foreach (var item in value)
                {
                    var itemCopy = item;
                    ElementSetValueConverter.WriteStringValue(writer, ref itemCopy);
                }
            }
            
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }

    internal sealed class StringISetDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(ISet<string>);

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata) => new StringSetDdbConverter(metadata);
    }
}