using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Context.RequestBuilders
{
    public class GetItemRequestKeysBuilder
    {
        public string TableName { get; }

        public string? PartitionKeyName { get; }

        public AttributeValue PartitionKeyValue { get; }
        
        public string? SortKeyName { get; }
        
        public AttributeValue SortKeyValue { get; }

        internal GetItemRequestKeysBuilder(string tableName, string? partitionKeyName, AttributeValue partitionKeyValue, string? sortKeyName, AttributeValue sortKeyValue)
        {
            TableName = tableName;
            PartitionKeyName = partitionKeyName;
            PartitionKeyValue = partitionKeyValue;
            SortKeyName = sortKeyName;
            SortKeyValue = sortKeyValue;
        }
    }

    public class GetItemRequestPartitionKeyBuilder
    {
        public string TableName { get; }
        
        public string? PartitionKeyName { get; }
        
        public AttributeValue PartitionKeyValue { get; }

        internal GetItemRequestPartitionKeyBuilder(string tableName, string? partitionKeyName, AttributeValue partitionKeyValue)
        {
            TableName = tableName;
            PartitionKeyName = partitionKeyName;
            PartitionKeyValue = partitionKeyValue;
        }

        public GetItemRequestKeysBuilder WithSortKey(string attributeName, AttributeValue value) =>
            new GetItemRequestKeysBuilder(TableName, PartitionKeyName, PartitionKeyValue, attributeName, value);
        
        public GetItemRequestKeysBuilder WithSortKey(AttributeValue value) =>
            new GetItemRequestKeysBuilder(TableName, PartitionKeyName, PartitionKeyValue, null, value);
    }

    public class GetItemRequestBuilder
    {
        public string TableName { get; }
        
        internal GetItemRequestBuilder(string tableName) => TableName = tableName;

        public GetItemRequestPartitionKeyBuilder WithPartitionKey(string attributeName, AttributeValue value) =>
            new GetItemRequestPartitionKeyBuilder(TableName, attributeName, value);
        
        public GetItemRequestPartitionKeyBuilder WithPartitionKey(AttributeValue value) =>
            new GetItemRequestPartitionKeyBuilder(TableName, null, value);
    }
}