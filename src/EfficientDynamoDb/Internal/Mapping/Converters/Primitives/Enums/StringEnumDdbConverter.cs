using System;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Enums
{
    public sealed class StringEnumDdbConverter<TEnum> : DdbConverter<TEnum> where TEnum : struct, Enum
    {
        public override TEnum Read(in AttributeValue attributeValue)
        {
            var enumString = attributeValue.AsString();
            
            if (!Enum.TryParse(enumString, out TEnum value)
                && !Enum.TryParse(enumString, ignoreCase: true, out value))
            {
                throw new DdbException($"Couldn't parse '{typeof(TEnum).Name}' enum '{enumString}' value.");
            }

            return value;
        }
        
        public override AttributeValue Write(ref TEnum value) => new StringAttributeValue(value.ToString());
    }
}