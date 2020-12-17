using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class IntDdbConverter : NumberDdbConverter<int>
    {
        public override int Read(in AttributeValue attributeValue) => attributeValue.ToInt();
    }
}