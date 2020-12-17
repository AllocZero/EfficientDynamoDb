using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    internal sealed class BoolDdbConverter : DdbConverter<bool>
    {
        public override bool Read(in AttributeValue attributeValue) => attributeValue.AsBool();

        public override AttributeValue Write(ref bool value) => new BoolAttributeValue(value);
    }
}