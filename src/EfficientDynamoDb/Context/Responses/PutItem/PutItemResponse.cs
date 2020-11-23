using EfficientDynamoDb.Context.Responses.Misc;
using EfficientDynamoDb.Context.Responses.Misc.Capacity;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.Responses.PutItem
{
    public class PutItemResponse
    {
        public Document? Attributes { get; set; }
        
        public FullConsumedCapacity? ConsumedCapacity { get; set; }
        
        public ItemCollectionMetrics? ItemCollectionMetrics { get; set; }
    }
}