using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Enums
{
    internal sealed class LongEnumDdbConverter<TEnum> : DdbConverter<TEnum> where TEnum : struct, Enum
    {
        public override TEnum Read(in AttributeValue attributeValue)
        {
            var value = attributeValue.AsNumberAttribute().ToLong();

            return Unsafe.As<long, TEnum>(ref value);
        }
        
        public override AttributeValue Write(ref TEnum value) => new NumberAttributeValue(Unsafe.As<TEnum, long>(ref value).ToString());
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref TEnum value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, Unsafe.As<TEnum, long>(ref value));
            writer.WriteEndObject();
        }
    }
}