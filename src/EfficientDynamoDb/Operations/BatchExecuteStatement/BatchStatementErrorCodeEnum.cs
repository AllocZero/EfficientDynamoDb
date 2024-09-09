namespace EfficientDynamoDb.Operations.BatchExecuteStatement
{
    public enum BatchStatementErrorCodeEnum
    {
        AccessDenied,
        ConditionalCheckFailed,
        DuplicateItem,
        InternalServerError,
        ItemCollectionSizeLimitExceeded,
        ProvisionedThroughputExceeded,
        RequestLimitExceeded,
        ResourceNotFound,
        ThrottlingError,
        TransactionConflict,
        ValidationError
    }
}
