using System;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.Extensions;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Enums
{
    internal sealed class UIntEnumDdbConverter<TEnum> : DdbConverter<TEnum> where TEnum : struct, Enum
    {
        public override TEnum Read(in AttributeValue attributeValue)
        {
            var value = attributeValue.AsNumberAttribute().ToUInt();

            return Unsafe.As<uint, TEnum>(ref value);
        }
        
        public override TEnum Read(ref Utf8JsonReader reader, AttributeType attributeType)
        {
            if (!Utf8Parser.TryParse(reader.ValueSpan, out uint value, out _))
                throw new DdbException($"Couldn't parse uint enum ddb value from '{reader.GetString()}'.");

            return Unsafe.As<uint, TEnum>(ref value);
        }
        
        public override AttributeValue Write(ref TEnum value) => new NumberAttributeValue(Unsafe.As<TEnum, uint>(ref value).ToString());
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref TEnum value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, Unsafe.As<TEnum, uint>(ref value));
            writer.WriteEndObject();
        }
    }
}