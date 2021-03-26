using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Operations.Shared.Capacity
{
    [DynamoDBConverter(typeof(JsonObjectDdbConverter<TableConsumedCapacity>))]
    public class TableConsumedCapacity : ConsumedCapacity
    {
        [DynamoDBProperty("TableName")]
        public string? TableName { get; set; }
    }
}