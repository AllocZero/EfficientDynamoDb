using System.Buffers.Text;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    internal sealed class BoolDdbConverter : DdbConverter<bool>
    {
        public BoolDdbConverter() : base(true)
        {
        }

        public override bool Read(in AttributeValue attributeValue) => attributeValue.AsBool();

        public override AttributeValue Write(ref bool value) => new BoolAttributeValue(value);
        
        public override bool Read(ref DdbReader reader)
        {
            return reader.JsonReaderValue.GetBoolean();
        }
    }
}