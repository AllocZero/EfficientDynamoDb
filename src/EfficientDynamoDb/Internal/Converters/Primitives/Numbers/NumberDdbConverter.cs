using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;

namespace EfficientDynamoDb.Internal.Converters.Primitives.Numbers
{
    internal abstract class NumberDdbConverter<T> : DdbConverter<T> where T : struct
    {
        protected NumberDdbConverter() : base(true)
        {
        }

        public sealed override AttributeValue Write(ref T value) => new NumberAttributeValue(value.ToString());

        public string WriteStringValue(ref T value) => value.ToString();
    }
}