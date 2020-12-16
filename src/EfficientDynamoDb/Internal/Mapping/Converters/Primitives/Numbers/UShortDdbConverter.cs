using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class UShortDdbConverter : DdbConverter<ushort>
    {
        public override ushort Read(AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToUShort();
    }
}