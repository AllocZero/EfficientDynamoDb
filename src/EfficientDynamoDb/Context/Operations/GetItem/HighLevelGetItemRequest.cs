namespace EfficientDynamoDb.Context.Operations.GetItem
{
    public class HighLevelGetItemRequest : GetItemRequestBase
    {
        public object PartitionKey { get; }

        public object? SortKey { get; }

        public HighLevelGetItemRequest(object partitionKey) => PartitionKey = partitionKey;

        public HighLevelGetItemRequest(object partitionKey, object sortKey)
        {
            PartitionKey = partitionKey;
            SortKey = sortKey;
        }
    }
}