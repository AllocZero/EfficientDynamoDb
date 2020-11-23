using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.Responses.GetItem
{
    public class GetItemResponse
    {
        public GetItemConsumedCapacity? ConsumedCapacity { get; }
        
        public Document Item { get; }

        public GetItemResponse(Document item, GetItemConsumedCapacity? consumedCapacity)
        {
            Item = item;
            ConsumedCapacity = consumedCapacity;
        }
    }
}