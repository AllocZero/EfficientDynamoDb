namespace EfficientDynamoDb.Context.Operations.GetItem
{
    internal sealed class GetItemHighLevelRequest<TPk> : GetItemRequestBase
    {
        public TPk PartitionKey; // non-readonly field instead of property to be able to access it by ref 

        public GetItemHighLevelRequest(TPk partitionKey) => PartitionKey = partitionKey;
    }

    internal sealed class GetItemHighLevelRequest<TPk, TSk> : GetItemRequestBase
    {
        public TPk PartitionKey; // non-readonly field instead of property to be able to access it by ref

        public TSk SortKey; // non-readonly field instead of property to be able to access it by ref

        public GetItemHighLevelRequest(TPk partitionKey, TSk sortKey)
        {
            PartitionKey = partitionKey;
            SortKey = sortKey;
        }
    }
}