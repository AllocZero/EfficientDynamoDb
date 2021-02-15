using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    internal sealed class BoolDdbConverter : DdbConverter<bool>
    {
        public BoolDdbConverter() : base(true)
        {
        }

        public override bool Read(in AttributeValue attributeValue) => attributeValue.AsBool();
        
        public override AttributeValue Write(ref bool value) => new AttributeValue(new BoolAttributeValue(value));
        
        public override bool Read(ref DdbReader reader)
        {
            return reader.JsonReaderValue.GetBoolean();
        }
        
        public override void Write(in DdbWriter writer, ref bool value)
        {
            writer.WriteDdbBool(value);
        }
    }
}