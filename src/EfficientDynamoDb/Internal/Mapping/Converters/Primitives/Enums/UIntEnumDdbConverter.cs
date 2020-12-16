using System;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Enums
{
    internal sealed class UIntEnumDdbConverter<TEnum> : DdbConverter<TEnum> where TEnum : struct, Enum
    {
        public override TEnum Read(AttributeValue attributeValue)
        {
            var value = attributeValue.AsNumberAttribute().ToUInt();

            return Unsafe.As<uint, TEnum>(ref value);
        }
    }
}