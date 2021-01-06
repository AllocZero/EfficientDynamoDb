using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.DocumentModel.Capacity
{
    [DdbConverter(typeof(JsonObjectDdbConverter<ConsumedCapacity>))]
    public class ConsumedCapacity
    {
        public float CapacityUnits { get; set; }
    }
}