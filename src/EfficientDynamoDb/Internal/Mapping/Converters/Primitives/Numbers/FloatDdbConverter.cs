using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives.Numbers
{
    internal sealed class FloatDdbConverter : NumberDdbConverter<float>
    {
        public override float Read(in AttributeValue attributeValue) => attributeValue.AsNumberAttribute().ToFloat();
    }
}