namespace EfficientDynamoDb.Attributes
{
    public enum DynamoDbAttributeType : byte
    {
        Regular = 0,
        PartitionKey = 10,
        SortKey = 20
    }
}