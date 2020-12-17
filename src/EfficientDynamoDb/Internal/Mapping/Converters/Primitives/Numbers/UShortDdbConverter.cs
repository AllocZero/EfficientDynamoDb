using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class UShortDdbConverter : NumberDdbConverter<ushort>
    {
        public override ushort Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToUShort();
    }
}