using EfficientDynamoDb.Context.Operations.GetItem;
using EfficientDynamoDb.Context.Operations.Query;

namespace EfficientDynamoDb.Context
{
    public static class RequestsBuilder
    {
        public static GetItemRequestBuilder GetItem(string tableName) => new GetItemRequestBuilder(tableName);

        public static IQueryRequestBuilder Query(DynamoDbContext context) => new QueryRequestBuilder(context);
    }
}