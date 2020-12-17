using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class ShortDdbConverter : NumberDdbConverter<short>
    {
        public override short Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToShort();
    }
}