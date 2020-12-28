using System.Buffers.Text;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    internal sealed class BoolDdbConverter : DdbConverter<bool>
    {
        public override bool Read(in AttributeValue attributeValue) => attributeValue.AsBool();

        public override AttributeValue Write(ref bool value) => new BoolAttributeValue(value);
        
        public override bool Read(ref Utf8JsonReader reader, AttributeType attributeType)
        {
            return reader.GetBoolean();
        }
    }
}