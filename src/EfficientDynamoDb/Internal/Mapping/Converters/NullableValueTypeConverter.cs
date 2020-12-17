using System;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters
{
    internal sealed class NullableValueTypeConverter<T> : DdbConverter<T?> where T : struct
    {
        private readonly DdbConverter<T> _converter;

        public NullableValueTypeConverter(DdbConverter<T> converter)
        {
            _converter = converter;
        }

        public override T? Read(in AttributeValue attributeValue) => _converter.Read(in attributeValue);

        public override AttributeValue Write(ref T? value)
        {
            var notNullableValue = value!.Value;
            return _converter.Write(ref notNullableValue);
        }
    }
}