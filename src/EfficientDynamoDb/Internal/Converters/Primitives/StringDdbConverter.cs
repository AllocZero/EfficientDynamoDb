using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    internal sealed class StringDdbConverter : DdbConverter<string>
    {
        public override string Read(in AttributeValue attributeValue) => attributeValue.AsString();

        public override AttributeValue Write(ref string value) => new StringAttributeValue(value);

        internal override bool TryRead(ref Utf8JsonReader reader, ref DdbEntityReadStack state, AttributeType dynamoDbAttributeType, out string value)
        {
            value = reader.GetString()!;

            return true;
        }
    }
}