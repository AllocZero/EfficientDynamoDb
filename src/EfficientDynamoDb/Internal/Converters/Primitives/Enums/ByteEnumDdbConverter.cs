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
    internal sealed class ByteEnumDdbConverter<TEnum> : DdbConverter<TEnum> where TEnum : struct, Enum
    {
        public override TEnum Read(in AttributeValue attributeValue)
        {
            var value = attributeValue.AsNumberAttribute().ToByte();

            return Unsafe.As<byte, TEnum>(ref value);
        }

        public override TEnum Read(ref Utf8JsonReader reader, AttributeType attributeType)
        {
            if (!Utf8Parser.TryParse(reader.ValueSpan, out byte value, out _))
                throw new DdbException($"Couldn't parse byte enum ddb value from '{reader.GetString()}'.");

            return Unsafe.As<byte, TEnum>(ref value);
        }

        public override AttributeValue Write(ref TEnum value) => new NumberAttributeValue(Unsafe.As<TEnum, byte>(ref value).ToString());

        public override void Write(Utf8JsonWriter writer, string attributeName, ref TEnum value)
        {
            writer.WritePropertyName(attributeName);
            
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, Unsafe.As<TEnum, byte>(ref value));
            writer.WriteEndObject();
        }
    }
}