namespace EfficientDynamoDb.Internal.Reader
{
    public enum DdbStackFramePropertyState : byte
    {
        None,
        ReadName,
        Name,
        ReadValueStart,
        ReadValueType,
        ReadValue,
        TryRead
    }
}