using System;
using System.Buffers.Text;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Extensions;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Enums
{
    internal sealed class ShortEnumDdbConverter<TEnum> : DdbConverter<TEnum>, IDictionaryKeyConverter<TEnum>, ISetValueConverter<TEnum> where TEnum : struct, Enum
    {
        public ShortEnumDdbConverter() : base(true)
        {
        }

        public override TEnum Read(in AttributeValue attributeValue)
        {
            var value = attributeValue.AsNumberAttribute().ToShort();

            return Unsafe.As<short, TEnum>(ref value);
        }
        
        public override TEnum Read(ref DdbReader reader)
        {
            if (!Utf8Parser.TryParse(reader.JsonReaderValue.ValueSpan, out short value, out _))
                throw new DdbException($"Couldn't parse short enum ddb value from '{reader.JsonReaderValue.GetString()}'.");

            return Unsafe.As<short, TEnum>(ref value);
        }

        public override AttributeValue Write(ref TEnum value) => new AttributeValue(new NumberAttributeValue(Unsafe.As<TEnum, short>(ref value).ToString()));

        public override void Write(in DdbWriter writer, ref TEnum value) => WriteInlined(writer.JsonWriter, ref value);

        public void WritePropertyName(in DdbWriter writer, ref TEnum value) => writer.JsonWriter.WritePropertyName(Unsafe.As<TEnum, short>(ref value));

        public string WriteStringValue(ref TEnum value) => Unsafe.As<TEnum, short>(ref value).ToString();

        public void WriteStringValue(in DdbWriter writer, ref TEnum value) => writer.JsonWriter.WriteStringValue(Unsafe.As<TEnum, short>(ref value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlined(Utf8JsonWriter writer, ref TEnum value)
        {
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.Number, Unsafe.As<TEnum, short>(ref value));
            writer.WriteEndObject();
        }
    }
}