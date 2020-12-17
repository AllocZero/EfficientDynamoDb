using EfficientDynamoDb.DocumentModel.AttributeValues;
using NotImplementedException = System.NotImplementedException;

namespace EfficientDynamoDb.Internal.Mapping.Converters.Primitives
{
    internal sealed class BoolDdbConverter : DdbConverter<bool>
    {
        public override bool Read(in AttributeValue attributeValue) => attributeValue.AsBool();

        public override AttributeValue Write(ref bool value) => new BoolAttributeValue(value);
    }
}