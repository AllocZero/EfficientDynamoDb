namespace EfficientDynamoDb.Operations.DescribeTable.Models.Enums
{
    public enum TableStatus
    {
        _UNKNOWN = 0,
        CREATING = 10,
        UPDATING = 20,
        DELETING = 30,
        ACTIVE = 40,
        INACCESSIBLE_ENCRYPTION_CREDENTIALS = 50,
        ARCHIVING = 60,
        ARCHIVED = 70,
    }
}