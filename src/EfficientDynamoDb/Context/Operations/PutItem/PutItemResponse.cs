using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Capacity;

namespace EfficientDynamoDb.Context.Operations.PutItem
{
    public class PutItemResponse
    {
        public Document? Attributes { get; set; }
        
        public FullConsumedCapacity? ConsumedCapacity { get; set; }
        
        public ItemCollectionMetrics? ItemCollectionMetrics { get; set; }
    }
}