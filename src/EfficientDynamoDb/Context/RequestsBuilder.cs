using EfficientDynamoDb.Context.Operations.GetItem;

namespace EfficientDynamoDb.Context
{
    public static class RequestsBuilder
    {
        public static GetItemRequestBuilder GetItem(string tableName) => new GetItemRequestBuilder(tableName);
    }
}