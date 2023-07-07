using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Operations.Shared
{
    [DynamoDbConverter(typeof(JsonObjectDdbConverter<ConditionalCheckFailedResponse>))]
    public class ConditionalCheckFailedResponse
    {
        [DynamoDbProperty("Message")]
        public string Message { get; set; } = null!;

        [DynamoDbProperty("Item")]
        public Document? Item { get; set; }
    }
}