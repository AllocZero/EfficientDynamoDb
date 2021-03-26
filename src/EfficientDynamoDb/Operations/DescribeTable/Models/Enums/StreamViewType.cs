namespace EfficientDynamoDb.Operations.DescribeTable.Models.Enums
{
    public enum StreamViewType
    {
        _UNKNOWN = 0,
        KEYS_ONLY = 10,
        NEW_IMAGE = 20,
        OLD_IMAGE = 30,
        NEW_AND_OLD_IMAGES = 40
    }
}