namespace EfficientDynamoDb.Context.Operations.DescribeTable.Models.Indexes
{
    public class ReplicaGlobalSecondaryIndexDescription
    {
        public string IndexName { get; }
        
        public ProvisionedThroughputOverride ProvisionedThroughputOverride { get; }

        public ReplicaGlobalSecondaryIndexDescription(string indexName, ProvisionedThroughputOverride provisionedThroughputOverride)
        {
            IndexName = indexName;
            ProvisionedThroughputOverride = provisionedThroughputOverride;
        }
    }
}