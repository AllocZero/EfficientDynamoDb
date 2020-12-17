using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class ULongDdbConverter : NumberDdbConverter<ulong>
    {
        public override ulong Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToULong();
    }
}