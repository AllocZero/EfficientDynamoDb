namespace EfficientDynamoDb.Internal.Metadata
{
    public enum DdbClassType : byte
    {
        None = 0,
        Object = 1,
        Value = 2,
        NewValue = 4,
        Enumerable = 8,
        Dictionary = 16, // 0x10
    }
}