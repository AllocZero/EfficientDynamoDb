using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Operations.Shared.Capacity
{
    [DynamoDBConverter(typeof(JsonObjectDdbConverter<ConsumedCapacity>))]
    public class ConsumedCapacity
    {
        [DynamoDBProperty("CapacityUnits")]
        public float CapacityUnits { get; set; }
    }
}