using System;
using System.Threading.Tasks;
using EfficientDynamoDb.Exceptions;

namespace EfficientDynamoDb.Operations;

public static class OpResultExtensions
{
    public static ServiceUnavailableException AsServiceUnavailableException<T>(this T opResult) where T : IOpResult =>
        AsException<T, ServiceUnavailableException>(opResult, ServiceUnavailableException.ErrorType);

    public static InternalServerErrorException AsInternalServerErrorException<T>(this T opResult) where T : IOpResult =>
        AsException<T, InternalServerErrorException>(opResult, InternalServerErrorException.ErrorType);

    public static TransactionCanceledException AsTransactionCanceledException<T>(this T opResult) where T : IOpResult =>
        AsException<T, TransactionCanceledException>(opResult, TransactionCanceledException.ErrorType);

    public static ConditionalCheckFailedException AsConditionalCheckFailedException<T>(this T opResult) where T : IOpResult =>
        AsException<T, ConditionalCheckFailedException>(opResult, ConditionalCheckFailedException.ErrorType);

    public static ProvisionedThroughputExceededException AsProvisionedThroughputExceededException<T>(this T opResult) where T : IOpResult =>
        AsException<T, ProvisionedThroughputExceededException>(opResult, ProvisionedThroughputExceededException.ErrorType);

    public static AccessDeniedException AsAccessDeniedException<T>(this T opResult) where T : IOpResult =>
        AsException<T, AccessDeniedException>(opResult, AccessDeniedException.ErrorType);

    public static IncompleteSignatureException AsIncompleteSignatureException<T>(this T opResult) where T : IOpResult =>
        AsException<T, IncompleteSignatureException>(opResult, IncompleteSignatureException.ErrorType);

    public static ItemCollectionSizeLimitExceededException AsItemCollectionSizeLimitExceededException<T>(this T opResult) where T : IOpResult =>
        AsException<T, ItemCollectionSizeLimitExceededException>(opResult, ItemCollectionSizeLimitExceededException.ErrorType);

    public static LimitExceededException AsLimitExceededException<T>(this T opResult) where T : IOpResult =>
        AsException<T, LimitExceededException>(opResult, LimitExceededException.ErrorType);

    public static MissingAuthenticationTokenException AsMissingAuthenticationTokenException<T>(this T opResult) where T : IOpResult =>
        AsException<T, MissingAuthenticationTokenException>(opResult, MissingAuthenticationTokenException.ErrorType);

    public static RequestLimitExceededException AsRequestLimitExceededException<T>(this T opResult) where T : IOpResult =>
        AsException<T, RequestLimitExceededException>(opResult, RequestLimitExceededException.ErrorType);

    public static ResourceInUseException AsResourceInUseException<T>(this T opResult) where T : IOpResult =>
        AsException<T, ResourceInUseException>(opResult, ResourceInUseException.ErrorType);

    public static ResourceNotFoundException AsResourceNotFoundException<T>(this T opResult) where T : IOpResult =>
        AsException<T, ResourceNotFoundException>(opResult, ResourceNotFoundException.ErrorType);

    public static ThrottlingException AsThrottlingException<T>(this T opResult) where T : IOpResult =>
        AsException<T, ThrottlingException>(opResult, ThrottlingException.ErrorType);

    public static UnrecognizedClientException AsUnrecognizedClientException<T>(this T opResult) where T : IOpResult =>
        AsException<T, UnrecognizedClientException>(opResult, UnrecognizedClientException.ErrorType);

    public static ValidationException AsValidationException<T>(this T opResult) where T : IOpResult =>
        AsException<T, ValidationException>(opResult, ValidationException.ErrorType);

    public static IdempotentParameterMismatchException AsIdempotentParameterMismatchException<T>(this T opResult) where T : IOpResult =>
        AsException<T, IdempotentParameterMismatchException>(opResult, IdempotentParameterMismatchException.ErrorType);

    public static TransactionInProgressException AsTransactionInProgressException<T>(this T opResult) where T : IOpResult =>
        AsException<T, TransactionInProgressException>(opResult, TransactionInProgressException.ErrorType);

    private static TException AsException<T, TException>(this T opResult, OpErrorType expectedType)
        where TException : DdbException
        where T : IOpResult
        => opResult.Exception switch
        {
            TException ex => ex,
            _ => throw new InvalidOperationException($"Operation error contains '{opResult.ErrorType}' value instead of '{expectedType}'.")
        };

    internal static async Task<T> EnsureSuccess<T>(this Task<OpResult<T>> task)
    {
        var result = await task.ConfigureAwait(false);
        return result.EnsureSuccess();
    }

    internal static async Task EnsureSuccess(this Task<OpResult> task)
    {
        var result = await task.ConfigureAwait(false);
        result.EnsureSuccess();
    }
}