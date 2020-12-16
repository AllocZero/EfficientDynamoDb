using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class ShortDdbConverter : DdbConverter<short>
    {
        public override short Read(AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToShort();
    }
}