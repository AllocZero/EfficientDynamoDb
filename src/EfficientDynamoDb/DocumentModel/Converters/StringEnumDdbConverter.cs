using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Constants;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.TypeParsers;

namespace EfficientDynamoDb.DocumentModel.Converters
{
    public sealed class StringEnumDdbConverter<TEnum> : DdbConverter<TEnum>, IDictionaryKeyConverter<TEnum>, ISetValueConverter<TEnum> where TEnum : struct, Enum
    {
        public StringEnumDdbConverter() : base(true)
        {
        }
        
        public override TEnum Read(in AttributeValue attributeValue)
        {
            var enumString = attributeValue.AsString();
            
            if (!EnumParser.TryParseCaseInsensitive(enumString, out TEnum value))
                throw new DdbException($"Couldn't parse '{typeof(TEnum).Name}' enum '{enumString}' value.");

            return value;
        }

        public override TEnum Read(ref DdbReader reader)
        {
            throw new NotImplementedException();
        }

        public override AttributeValue Write(ref TEnum value) => new StringAttributeValue(value.ToString());

        public override void Write(in DdbWriter writer, ref TEnum value) => WriteInlined(writer.JsonWriter, ref value);

        public void WritePropertyName(in DdbWriter writer, ref TEnum value)=> writer.JsonWriter.WritePropertyName(value.ToString());

        public string WriteStringValue(ref TEnum value) => value.ToString();
        
        public void WriteStringValue(in DdbWriter writer, ref TEnum value) => writer.JsonWriter.WriteStringValue(value.ToString());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteInlined(Utf8JsonWriter writer, ref TEnum value)
        {
            writer.WriteStartObject();
            writer.WriteString(DdbTypeNames.String, value.ToString());
            writer.WriteEndObject();
        }
    }

    public sealed class StringEnumDdbConverterFactory : DdbConverterFactory
    {
        public override bool CanConvert(Type typeToConvert) => typeToConvert.IsEnum;

        public override DdbConverter CreateConverter(Type typeToConvert, DynamoDbContextMetadata metadata)
        {
            return (DdbConverter) Activator.CreateInstance(typeof(StringEnumDdbConverter<>).MakeGenericType(typeToConvert));
        }
    }
}