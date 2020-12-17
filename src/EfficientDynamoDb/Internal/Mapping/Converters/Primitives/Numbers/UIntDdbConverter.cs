using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class UIntDdbConverter : NumberDdbConverter<uint>
    {
        public override uint Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToUInt();
    }
}