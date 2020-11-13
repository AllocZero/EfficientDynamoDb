using System.Text.Json;

namespace EfficientDynamoDb.DocumentModel.AttributeValues
{
    public interface IAttributeValue
    {
        void Write(Utf8JsonWriter writer);
    }
}