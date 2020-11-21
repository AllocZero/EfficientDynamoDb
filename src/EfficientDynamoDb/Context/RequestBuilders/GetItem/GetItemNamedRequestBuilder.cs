using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Builder;

namespace EfficientDynamoDb.Context.RequestBuilders
{
    public class GetItemNamedRequestBuilder<TPk, TSk> : IGetItemRequestBuilder<TPk, TSk>
        where TPk : IAttributeValue
        where TSk : IAttributeValue
    {
        private readonly DdbKey<TPk> _pk;
        private readonly DdbKey<TSk> _sk;
        
        public string TableName { get; }
        
        public string? PartitionKeyName => _pk.AttributeName;
        
        public TPk PartitionKeyValue => _pk.Value;
        
        public string? SortKeyName => _sk.AttributeName;
        
        public TSk SortKeyValue => _sk.Value;

        internal GetItemNamedRequestBuilder(string tableName, DdbKey<TPk> partitionKey, DdbKey<TSk> sortKey)
        {
            TableName = tableName;
            _pk = partitionKey;
            _sk = sortKey;
        }

        GetItemRequestData<TPk, TSk> IGetItemRequestBuilder<TPk, TSk>.Build() =>
            new GetItemRequestData<TPk, TSk>(TableName, _pk.AttributeName, _pk.Value, _sk.AttributeName, _sk.Value);
    }

    public class GetItemNamedRequestBuilder<TPk> : IGetItemRequestBuilder<TPk>
        where TPk : IAttributeValue
    {
        private readonly DdbKey<TPk> _pk;
        
        public string TableName { get; }
        
        public string? PartitionKeyName => _pk.AttributeName;
        
        public TPk PartitionKeyValue => _pk.Value;

        internal GetItemNamedRequestBuilder(string tableName, DdbKey<TPk> pk)
        {
            TableName = tableName;
            _pk = pk;
        }

        public GetItemNamedRequestBuilder<TPk, TSk> WithSortKey<TSk>(string attributeName, TSk value) where TSk : IAttributeValue =>
            new GetItemNamedRequestBuilder<TPk, TSk>(TableName, _pk, new DdbKey<TSk>(attributeName, value));

        
        GetItemRequestData<TPk> IGetItemRequestBuilder<TPk>.Build() => new GetItemRequestData<TPk>(TableName, _pk.AttributeName, _pk.Value);
    }

    public class GetItemNamedRequestBuilder : IGetItemRequestBuilder
    {
        public string TableName { get; }
        
        internal GetItemNamedRequestBuilder(string tableName) => TableName = tableName;

        public GetItemNamedRequestBuilder<T> WithPartitionKey<T>(string attributeName, T value) where T : IAttributeValue =>
            new GetItemNamedRequestBuilder<T>(TableName, new DdbKey<T>(attributeName, value));
    }
}