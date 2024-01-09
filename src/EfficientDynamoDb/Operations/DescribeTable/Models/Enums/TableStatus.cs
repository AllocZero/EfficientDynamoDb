namespace EfficientDynamoDb.Operations.DescribeTable.Models.Enums
{
    public enum TableStatus
    {
        Undefined = 0,
        Creating = 10,
        Updating = 20,
        Deleting = 30,
        Active = 40,
        InaccessibleEncryptionCredentials = 50,
        Archiving = 60,
        Archived = 70,
    }
}