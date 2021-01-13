using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Constants;


namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal sealed class StringSetDdbConverter : SetDdbConverter<HashSet<string>, string>
    {
        public StringSetDdbConverter(DynamoDbContextMetadata metadata) : base(metadata)
        {
        }

        protected override HashSet<string> CreateSet() => new HashSet<string>();

        public override HashSet<string>? Read(in AttributeValue attributeValue)
        {
            if (attributeValue.IsNull)
                return null;
            
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

        public override bool TryWrite(ref HashSet<string>? value, out AttributeValue attributeValue)
        {
            attributeValue = WriteInlined(ref value!);
            return true;
        }

        public override AttributeValue Write(ref HashSet<string>? value)
        {
            return value == null ? AttributeValue.Null : WriteInlined(ref value);
        }
        
        public override void Write(in DdbWriter writer, string attributeName, ref HashSet<string>? value)
        {
            writer.JsonWriter.WritePropertyName(attributeName);
            
            WriteInlined(in writer, ref value!);
        }

        public override void Write(in DdbWriter writer, ref HashSet<string>? value)
        {
            if (value == null)
            {
                writer.WriteDdbNull();
                return;
            }

            WriteInlined(in writer, ref value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private AttributeValue WriteInlined(ref HashSet<string> value)
        {
            if (ElementConverter.IsInternal)
                return new StringSetAttributeValue(value);

            var set = new HashSet<string>(value.Count);

            foreach (var item in value)
            {
                var copy = item;
                set.Add(ElementConverter.Write(ref copy).GetString());
            }

            return new StringSetAttributeValue(set);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteInlined(in DdbWriter writer, ref HashSet<string> value)
        {
            writer.JsonWriter.WriteStartObject();
            writer.JsonWriter.WritePropertyName(DdbTypeNames.StringSet);
            
            writer.JsonWriter.WriteStartArray();

            if (ElementConverter.IsInternal)
            {
                foreach (var item in value)
                    writer.JsonWriter.WriteStringValue(item);
            }
            else
            {
                foreach (var item in value)
                {
                    var itemCopy = item;
                    ElementSetValueConverter.WriteStringValue(in writer, ref itemCopy);
                }
            }
            
            writer.JsonWriter.WriteEndArray();
            writer.JsonWriter.WriteEndObject();
        }
    }

    internal sealed class StringSetDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(HashSet<string>);

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata) => new StringSetDdbConverter(metadata);
    }
}