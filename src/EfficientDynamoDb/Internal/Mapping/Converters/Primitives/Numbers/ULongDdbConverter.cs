using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class ULongDdbConverter : DdbConverter<ulong>
    {
        public override ulong Read(AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToULong();
    }
}