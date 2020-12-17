using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class DecimalDdbConverter : NumberDdbConverter<decimal>
    {
        public override decimal Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToDecimal();
    }
}