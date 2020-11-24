using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.DocumentModel
{
    public class PrimaryKey
    {
        public string? PartitionKeyName { get; }

        public AttributeValue PartitionKeyValue { get; }

        public string? SortKeyName { get; }

        public AttributeValue? SortKeyValue { get; }

        internal bool HasKeyNames => PartitionKeyName != null && (SortKeyValue == null || SortKeyName != null);

        public PrimaryKey(AttributeValue partitionKeyValue)
        {
            PartitionKeyValue = partitionKeyValue;
        }

        public PrimaryKey(string partitionKeyName, AttributeValue partitionKeyValue)
        {
            PartitionKeyName = partitionKeyName;
            PartitionKeyValue = partitionKeyValue;
        }

        public PrimaryKey(AttributeValue partitionKeyValue, AttributeValue sortKeyValue)
        {
            PartitionKeyValue = partitionKeyValue;
            SortKeyValue = sortKeyValue;
        }

        public PrimaryKey(string partitionKeyName, AttributeValue partitionKeyValue, string sortKeyName, AttributeValue sortKeyValue)
        {
            PartitionKeyName = partitionKeyName;
            PartitionKeyValue = partitionKeyValue;
            SortKeyName = sortKeyName;
            SortKeyValue = sortKeyValue;
        }
    }
}