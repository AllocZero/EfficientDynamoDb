using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Context.Requests
{
    public class DdbPrimaryKey
    {
        public string? PartitionKeyName { get; }

        public AttributeValue PartitionKeyValue { get; }

        public string? SortKeyName { get; }

        public AttributeValue? SortKeyValue { get; }

        internal bool HasKeyNames => PartitionKeyName != null && (SortKeyValue == null || SortKeyName != null);

        public DdbPrimaryKey(AttributeValue partitionKeyValue)
        {
            PartitionKeyValue = partitionKeyValue;
        }

        public DdbPrimaryKey(string partitionKeyName, AttributeValue partitionKeyValue)
        {
            PartitionKeyName = partitionKeyName;
            PartitionKeyValue = partitionKeyValue;
        }

        public DdbPrimaryKey(AttributeValue partitionKeyValue, AttributeValue sortKeyValue)
        {
            PartitionKeyValue = partitionKeyValue;
            SortKeyValue = sortKeyValue;
        }

        public DdbPrimaryKey(string partitionKeyName, AttributeValue partitionKeyValue, string sortKeyName, AttributeValue sortKeyValue)
        {
            PartitionKeyName = partitionKeyName;
            PartitionKeyValue = partitionKeyValue;
            SortKeyName = sortKeyName;
            SortKeyValue = sortKeyValue;
        }
    }
}