using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    // TODO: Implement overrides for performance
    internal sealed class BoolDdbConverter : DdbConverter<bool>
    {
        public BoolDdbConverter() : base(true)
        {
        }

        public override bool Read(in AttributeValue attributeValue) => attributeValue.AsBool();

        public override bool TryWrite(ref bool value, out AttributeValue attributeValue)
        {
            attributeValue = new AttributeValue(new BoolAttributeValue(value));
            return true;
        }

        public override AttributeValue Write(ref bool value) => new AttributeValue(new BoolAttributeValue(value));
        
        public override bool Read(ref DdbReader reader)
        {
            return reader.JsonReaderValue.GetBoolean();
        }
    }
}