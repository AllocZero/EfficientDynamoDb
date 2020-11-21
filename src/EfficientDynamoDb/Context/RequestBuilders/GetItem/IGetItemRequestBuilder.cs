using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Context.RequestBuilders
{
    public interface IGetItemRequestBuilder
    {
        public string TableName { get; }
    }
    
    public interface IGetItemRequestBuilder<TPk>
        where TPk : IAttributeValue
    {
        public string TableName { get; }

        public string? PartitionKeyName { get; }

        public TPk PartitionKeyValue { get; }

        internal GetItemRequestData<TPk> Build();
    }

    public interface IGetItemRequestBuilder<TPk, TSk>
        where TPk : IAttributeValue
        where TSk : IAttributeValue
    {
        public string TableName { get; }

        public string? PartitionKeyName { get; }

        public TPk PartitionKeyValue { get; }

        public string? SortKeyName { get; }

        public TSk SortKeyValue { get; }

        internal GetItemRequestData<TPk, TSk> Build();
    }
}