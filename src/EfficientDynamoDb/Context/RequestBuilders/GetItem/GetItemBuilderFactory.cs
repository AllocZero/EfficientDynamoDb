namespace EfficientDynamoDb.Context.RequestBuilders
{
    public static class GetItemBuilderFactory
    {
        public static GetItemNamedRequestBuilder BuildNamed(string tableName) => new GetItemNamedRequestBuilder(tableName);

        public static GetItemRequestBuilder Build(string tableName) => new GetItemRequestBuilder(tableName);
    }
}