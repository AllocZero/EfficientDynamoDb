namespace EfficientDynamoDb.Operations;

public enum OpErrorType
{
    None,
    Unknown,
    ServiceUnavailable,
    InternalServerError,
    TransactionCanceled,
    ConditionalCheckFailed,
    ProvisionedThroughputExceeded,
    AccessDenied,
    IncompleteSignature,
    ItemCollectionSizeLimitExceeded,
    LimitExceeded,
    MissingAuthenticationToken,
    RequestLimitExceeded,
    ResourceInUse,
    ResourceNotFound,
    Throttling,
    UnrecognizedClient,
    Validation,
    IdempotentParameterMismatch,
    TransactionInProgress
}