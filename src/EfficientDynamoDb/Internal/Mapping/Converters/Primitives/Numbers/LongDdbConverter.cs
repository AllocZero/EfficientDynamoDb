using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class LongDdbConverter : DdbConverter<long>
    {
        public override long Read(AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToLong();
    }
}