using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Operations.Shared.Capacity
{
    [DynamoDbConverter(typeof(JsonObjectDdbConverter<TableConsumedCapacity>))]
    public class TableConsumedCapacity : ConsumedCapacity
    {
        [DynamoDbProperty("TableName")]
        public string? TableName { get; set; }
    }
}