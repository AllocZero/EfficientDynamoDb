namespace EfficientDynamoDb.Context.Operations.GetItem
{
    public class GetItemHighLevelRequest : GetItemRequestBase
    {
        public object PartitionKey { get; }

        public object? SortKey { get; }

        public GetItemHighLevelRequest(object partitionKey) => PartitionKey = partitionKey;

        public GetItemHighLevelRequest(object partitionKey, object sortKey)
        {
            PartitionKey = partitionKey;
            SortKey = sortKey;
        }
    }
}