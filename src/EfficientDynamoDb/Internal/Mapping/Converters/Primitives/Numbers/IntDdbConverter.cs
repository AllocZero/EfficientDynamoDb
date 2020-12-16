using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class IntDdbConverter : DdbConverter<int>
    {
        public override int Read(AttributeValue attributeValue) => attributeValue.ToInt();
    }
}