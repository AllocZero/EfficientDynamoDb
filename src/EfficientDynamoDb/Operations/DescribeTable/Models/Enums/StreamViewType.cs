namespace EfficientDynamoDb.Operations.DescribeTable.Models.Enums
{
    public enum StreamViewType
    {
        Undefined = 0,
        KeysOnly = 10,
        NewImage = 20,
        OldImage = 30,
        NewAndOldImages = 40
    }
}