namespace EfficientDynamoDb.Context.RequestBuilders
{
    public static class RequestsBuilder
    {
        public static GetItemRequestBuilder GetItem(string tableName) => new GetItemRequestBuilder(tableName);
    }
}