namespace EfficientDynamoDb.Internal.Reader
{
    public enum DdbStackFrameObjectState : byte
    {
        None,
        StartToken,
        ReadAheadNameOrEndObject,
        ReadNameOrEndObject,
        ReadAheadIdValue,
        ReadAheadRefValue,
        ReadIdValue,
        ReadRefValue,
        ReadAheadRefEndObject,
        ReadRefEndObject,
        ReadAheadValuesName,
        ReadValuesName,
        ReadAheadValuesStartArray,
        ReadValuesStartArray,
        PropertyValue,
        CreatedObject,
        ReadElements,
        EndToken,
        EndTokenValidation,
    }
}