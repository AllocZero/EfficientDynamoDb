namespace EfficientDynamoDb.Api.DescribeTable.Models
{
    public class ProvisionedThroughputOverride
    {
        public int ReadCapacityUnits { get; }

        public ProvisionedThroughputOverride(int readCapacityUnits) => ReadCapacityUnits = readCapacityUnits;
    }
}