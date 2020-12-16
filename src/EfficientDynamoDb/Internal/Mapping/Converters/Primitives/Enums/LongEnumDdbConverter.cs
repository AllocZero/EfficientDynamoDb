using System;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Enums
{
    internal sealed class LongEnumDdbConverter<TEnum> : DdbConverter<TEnum> where TEnum : struct, Enum
    {
        public override TEnum Read(AttributeValue attributeValue)
        {
            var value = attributeValue.AsNumberAttribute().ToLong();

            return Unsafe.As<long, TEnum>(ref value);
        }
    }
}