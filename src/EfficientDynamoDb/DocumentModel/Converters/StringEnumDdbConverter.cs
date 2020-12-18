using System;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.TypeParsers;

namespace EfficientDynamoDb.DocumentModel.Converters
{
    public sealed class StringEnumDdbConverter<TEnum> : DdbConverter<TEnum> where TEnum : struct, Enum
    {
        public override TEnum Read(in AttributeValue attributeValue)
        {
            var enumString = attributeValue.AsString();
            
            if (!EnumParser.TryParseCaseInsensitive(enumString, out TEnum value))
                throw new DdbException($"Couldn't parse '{typeof(TEnum).Name}' enum '{enumString}' value.");

            return value;
        }
        
        public override AttributeValue Write(ref TEnum value) => new StringAttributeValue(value.ToString());
    }
}