using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class DoubleDdbConverter : NumberDdbConverter<double>
    {
        public override double Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToDouble();
    }
}