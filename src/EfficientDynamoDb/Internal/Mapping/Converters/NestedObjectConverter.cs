using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Mapping.Extensions;

namespace EfficientDynamoDb.Internal.Mapping.Converters
{
    internal sealed class NestedObjectConverter<T> : DdbConverter<T> where T : class
    {
        public override T Read(in AttributeValue attributeValue) => attributeValue.AsDocument().ToObject<T>();

        public override AttributeValue Write(ref T value) => new MapAttributeValue(value.ToDocument());
    }
}