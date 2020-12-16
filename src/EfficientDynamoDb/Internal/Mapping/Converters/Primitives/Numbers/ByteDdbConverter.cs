using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class ByteDdbConverter : DdbConverter<byte>
    {
        public override byte Read(AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToByte();
    }
}