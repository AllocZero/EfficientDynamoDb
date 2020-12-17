using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal abstract class NumberDdbConverter<T> : DdbConverter<T> where T : struct
    {
        public sealed override AttributeValue Write(ref T value) => new NumberAttributeValue(value.ToString());
    }
}