using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Enums
{
    internal sealed class IntEnumDdbConverter<TEnum> : DdbConverter<TEnum> where TEnum : struct, Enum
    {
        public override TEnum Read(in AttributeValue attributeValue)
        {
            var value = attributeValue.AsNumberAttribute().ToInt();

            return Unsafe.As<int, TEnum>(ref value);
        }
        
        public override AttributeValue Write(ref TEnum value) => new NumberAttributeValue(Unsafe.As<TEnum, int>(ref value).ToString());
        
        public override void Write(Utf8JsonWriter writer, string attributeName, ref TEnum value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, Unsafe.As<TEnum, int>(ref value));
            writer.WriteEndObject();
        }
    }
}