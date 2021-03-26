using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Operations.Shared.Capacity
{
    [DynamoDbConverter(typeof(JsonObjectDdbConverter<ConsumedCapacity>))]
    public class ConsumedCapacity
    {
        [DynamoDbProperty("CapacityUnits")]
        public float CapacityUnits { get; set; }
    }
}