using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Lambda
{
    [DynamoDbConverter(typeof(JsonObjectDdbConverter<Record>))]
    public class Record
    {
        [DynamoDbProperty("eventID")]
        public string EventId { get; set; } = null!;
        
        [DynamoDbProperty("eventName")]
        public string EventName { get; set; } = null!;
        
        [DynamoDbProperty("eventVersion")]
        public string EventVersion { get; set; } = null!;
        
        [DynamoDbProperty("eventSource")]
        public string EventSource { get; set; } = null!;
        
        [DynamoDbProperty("eventSource")]
        public string AwsRegion { get; set; } = null!;
        
        [DynamoDbProperty("eventSourceARN")]
        public string EventSourceARN { get; set; } = null!;

        [DynamoDbProperty("dynamodb", typeof(JsonObjectDdbConverter<StreamRecord>))] 
        public StreamRecord DynamoDb { get; set; } = null!;
    }
}