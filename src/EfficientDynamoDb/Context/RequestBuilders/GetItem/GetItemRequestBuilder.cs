using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Context.RequestBuilders
{
    public class GetItemRequestBuilder<TPk, TSk> : IGetItemRequestBuilder<TPk, TSk>
        where TPk : IAttributeValue
        where TSk : IAttributeValue
    {
        public string TableName { get; }
        
        public string? PartitionKeyName => null;
        
        public TPk PartitionKeyValue { get; }
        
        public string? SortKeyName => null;
        
        public TSk SortKeyValue { get; }
        
        internal GetItemRequestBuilder(string tableName, TPk partitionKey, TSk sortKey)
        {
            TableName = tableName;
            PartitionKeyValue = partitionKey;
            SortKeyValue = sortKey;
        }

        GetItemRequestData<TPk, TSk> IGetItemRequestBuilder<TPk, TSk>.Build() =>
            new GetItemRequestData<TPk, TSk>(TableName, null, PartitionKeyValue, null, SortKeyValue);
    }

    public class GetItemRequestBuilder<TPk> : IGetItemRequestBuilder<TPk>
        where TPk : IAttributeValue
    {
        public string TableName { get; }
        
        public string? PartitionKeyName => null;
        
        public TPk PartitionKeyValue { get; }

        internal GetItemRequestBuilder(string tableName, TPk partitionKey)
        {
            TableName = tableName;
            PartitionKeyValue = partitionKey;
        }

        public GetItemRequestBuilder<TPk, TSk> WithSortKey<TSk>(TSk sortKeyValue) where TSk : IAttributeValue =>
            new GetItemRequestBuilder<TPk, TSk>(TableName, PartitionKeyValue, sortKeyValue);

        GetItemRequestData<TPk> IGetItemRequestBuilder<TPk>.Build() => new GetItemRequestData<TPk>(TableName, PartitionKeyName, PartitionKeyValue);
    }

    public class GetItemRequestBuilder : IGetItemRequestBuilder
    {
        public string TableName { get; }
        
        internal GetItemRequestBuilder(string tableName) => TableName = tableName;

        public GetItemRequestBuilder<T> WithPartitionKey<T>(T partitionKeyValue) where T : IAttributeValue =>
            new GetItemRequestBuilder<T>(TableName, partitionKeyValue);
    }
}