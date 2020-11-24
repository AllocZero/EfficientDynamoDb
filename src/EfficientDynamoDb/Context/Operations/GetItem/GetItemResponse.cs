using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Capacity;

namespace EfficientDynamoDb.Context.Operations.GetItem
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