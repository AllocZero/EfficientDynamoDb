using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class ByteDdbConverter : NumberDdbConverter<byte>
    {
        public override byte Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToByte();

        
    }
}