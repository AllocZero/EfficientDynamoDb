using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.DocumentModel.Capacity
{
    [DdbConverter(typeof(JsonObjectDdbConverter<TableConsumedCapacity>))]
    public class TableConsumedCapacity : ConsumedCapacity
    {
        [DynamoDBProperty("TableName")]
        public string? TableName { get; set; }
    }
}