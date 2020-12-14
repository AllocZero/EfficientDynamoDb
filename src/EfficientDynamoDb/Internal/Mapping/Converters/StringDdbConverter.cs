using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters
{
    public sealed class StringDdbConverter : DdbConverter<string>
    {
        public override string Read(AttributeValue attributeValue) => attributeValue.AsString();
    }
}