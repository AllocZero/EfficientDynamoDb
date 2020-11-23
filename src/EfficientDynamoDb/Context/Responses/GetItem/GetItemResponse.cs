using EfficientDynamoDb.Context.Responses.Misc.Capacity;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.Responses.GetItem
{
    public class GetItemResponse
    {
        public TableConsumedCapacity? ConsumedCapacity { get; }
        
        public Document Item { get; }

        public GetItemResponse(Document item, TableConsumedCapacity? consumedCapacity)
        {
            Item = item;
            ConsumedCapacity = consumedCapacity;
        }
    }
}