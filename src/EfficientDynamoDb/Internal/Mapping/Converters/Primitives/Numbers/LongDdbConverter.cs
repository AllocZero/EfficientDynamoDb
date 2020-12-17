using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class LongDdbConverter : NumberDdbConverter<long>
    {
        public override long Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToLong();
    }
}