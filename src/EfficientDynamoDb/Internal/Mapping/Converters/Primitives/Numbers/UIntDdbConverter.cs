using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class UIntDdbConverter : DdbConverter<uint>
    {
        public override uint Read(AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToUInt();
    }
}