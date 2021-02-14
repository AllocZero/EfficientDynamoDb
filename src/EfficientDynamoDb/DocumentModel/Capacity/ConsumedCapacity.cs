using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.DocumentModel.Capacity
{
    [DdbConverter(typeof(JsonObjectDdbConverter<ConsumedCapacity>))]
    public class ConsumedCapacity
    {
        [DynamoDBProperty("CapacityUnits")]
        public float CapacityUnits { get; set; }
    }
}