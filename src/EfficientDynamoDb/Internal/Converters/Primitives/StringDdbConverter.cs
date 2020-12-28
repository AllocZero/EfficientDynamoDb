using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Primitives
{
    internal sealed class StringDdbConverter : DdbConverter<string>
    {
        public override string Read(in AttributeValue attributeValue) => attributeValue.AsString();

        public override AttributeValue Write(ref string value) => new StringAttributeValue(value);

        public override string Read(ref DdbReader reader)
        {
            return reader.JsonReaderValue.GetString()!;
        }
    }
}