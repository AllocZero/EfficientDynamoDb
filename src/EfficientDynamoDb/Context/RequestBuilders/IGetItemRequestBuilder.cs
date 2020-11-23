using EfficientDynamoDb.Context.Requests.GetItem;

namespace EfficientDynamoDb.Context.RequestBuilders
{
    public interface IGetItemRequestBuilder
    {
        internal GetItemRequest Build();
    }
}