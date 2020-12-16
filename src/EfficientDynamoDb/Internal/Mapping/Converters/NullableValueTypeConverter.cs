using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters
{
    internal sealed class NullableValueTypeConverter<T> : DdbConverter<T?> where T: struct
    {
        private readonly DdbConverter<T> _converter;

        public NullableValueTypeConverter(DdbConverter<T> converter)
        {
            _converter = converter;
        }

        public override T? Read(AttributeValue attributeValue) => _converter.Read(attributeValue);
    }
}