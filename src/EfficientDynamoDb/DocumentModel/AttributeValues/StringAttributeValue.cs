using System.Text.Json;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    public readonly struct StringAttributeValue : IAttributeValue
    {
        public string Value { get; }

        public StringAttributeValue(string value)
        {
            Value = value;
        }

        public void Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteString("S", Value);
            writer.WriteEndObject();
        }
    }
}