using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using EfficientDynamoDb.Exceptions;

namespace EfficientDynamoDb.Operations
{
    // TODO: Apply proper nullability attributes after migration to .net8
    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/attributes/nullable-analysis
    [StructLayout(LayoutKind.Auto)]
    public readonly struct OpResult
    {
        public OpErrorType ErrorType => Exception?.OpErrorType ?? OpErrorType.None;

        public DdbException? Exception { get; }

        public OpResult(DdbException? exception)
        {
            Exception = exception;
        }

        public void EnsureSuccess()
        {
            if (Exception is not null)
                throw Exception;
        }

        public bool IsSuccess => Exception is null;

        public ServiceUnavailableException AsServiceUnavailableException() => 
            AsException<ServiceUnavailableException>(ServiceUnavailableException.ErrorType);

        public InternalServerErrorException AsInternalServerErrorException() =>
            AsException<InternalServerErrorException>(InternalServerErrorException.ErrorType);

        public TransactionCanceledException AsTransactionCanceledException() =>
            AsException<TransactionCanceledException>(TransactionCanceledException.ErrorType);

        public ConditionalCheckFailedException AsConditionalCheckFailedException() =>
            AsException<ConditionalCheckFailedException>(ConditionalCheckFailedException.ErrorType);

        public ProvisionedThroughputExceededException AsProvisionedThroughputExceededException() =>
            AsException<ProvisionedThroughputExceededException>(ProvisionedThroughputExceededException.ErrorType);

        public AccessDeniedException AsAccessDeniedException() => 
            AsException<AccessDeniedException>(AccessDeniedException.ErrorType);

        public IncompleteSignatureException AsIncompleteSignatureException() =>
            AsException<IncompleteSignatureException>(IncompleteSignatureException.ErrorType);

        public ItemCollectionSizeLimitExceededException AsItemCollectionSizeLimitExceededException() =>
            AsException<ItemCollectionSizeLimitExceededException>(ItemCollectionSizeLimitExceededException.ErrorType);

        public LimitExceededException AsLimitExceededException() => 
            AsException<LimitExceededException>(LimitExceededException.ErrorType);

        public MissingAuthenticationTokenException AsMissingAuthenticationTokenException() =>
            AsException<MissingAuthenticationTokenException>(MissingAuthenticationTokenException.ErrorType);

        public RequestLimitExceededException AsRequestLimitExceededException() =>
            AsException<RequestLimitExceededException>(RequestLimitExceededException.ErrorType);

        public ResourceInUseException AsResourceInUseException() => 
            AsException<ResourceInUseException>(ResourceInUseException.ErrorType);

        public ResourceNotFoundException AsResourceNotFoundException() => 
            AsException<ResourceNotFoundException>(ResourceNotFoundException.ErrorType);

        public ThrottlingException AsThrottlingException() => 
            AsException<ThrottlingException>(ThrottlingException.ErrorType);

        public UnrecognizedClientException AsUnrecognizedClientException() => 
            AsException<UnrecognizedClientException>(UnrecognizedClientException.ErrorType);

        public ValidationException AsValidationException() => 
            AsException<ValidationException>(ValidationException.ErrorType);

        public IdempotentParameterMismatchException AsIdempotentParameterMismatchException() =>
            AsException<IdempotentParameterMismatchException>(IdempotentParameterMismatchException.ErrorType);

        public TransactionInProgressException AsTransactionInProgressException() =>
            AsException<TransactionInProgressException>(TransactionInProgressException.ErrorType);

        private T AsException<T>(OpErrorType expectedType) where T : DdbException => Exception switch
        {
            T ex => ex,
            _ => throw new InvalidOperationException($"Operation error contains '{ErrorType}' value instead of '{expectedType}'.")
        };
    }

    public readonly struct OpResult<T>
    {
        public T? Value { get; }

        public OpErrorType ErrorType => Exception?.OpErrorType ?? OpErrorType.None;
        
        public DdbException? Exception { get; }
        
        public OpResult(DdbException exception)
        {
            Value = default;
            Exception = exception;
        }
        
        public OpResult(T value)
        {
            Value = value;
            Exception = null;
        }

        public T EnsureSuccess()
        {
            if (ErrorType != OpErrorType.None && Exception is not null)
                throw Exception;

            return Value!;
        }

        public bool IsSuccess => Exception is null;
        
        public OpResult DiscardValue() => new(Exception);

        public ServiceUnavailableException AsServiceUnavailableException() => 
            AsException<ServiceUnavailableException>(ServiceUnavailableException.ErrorType);

        public InternalServerErrorException AsInternalServerErrorException() =>
            AsException<InternalServerErrorException>(InternalServerErrorException.ErrorType);

        public TransactionCanceledException AsTransactionCanceledException() =>
            AsException<TransactionCanceledException>(TransactionCanceledException.ErrorType);

        public ConditionalCheckFailedException AsConditionalCheckFailedException() =>
            AsException<ConditionalCheckFailedException>(ConditionalCheckFailedException.ErrorType);

        public ProvisionedThroughputExceededException AsProvisionedThroughputExceededException() =>
            AsException<ProvisionedThroughputExceededException>(ProvisionedThroughputExceededException.ErrorType);

        public AccessDeniedException AsAccessDeniedException() => 
            AsException<AccessDeniedException>(AccessDeniedException.ErrorType);

        public IncompleteSignatureException AsIncompleteSignatureException() =>
            AsException<IncompleteSignatureException>(IncompleteSignatureException.ErrorType);

        public ItemCollectionSizeLimitExceededException AsItemCollectionSizeLimitExceededException() =>
            AsException<ItemCollectionSizeLimitExceededException>(ItemCollectionSizeLimitExceededException.ErrorType);

        public LimitExceededException AsLimitExceededException() => 
            AsException<LimitExceededException>(LimitExceededException.ErrorType);

        public MissingAuthenticationTokenException AsMissingAuthenticationTokenException() =>
            AsException<MissingAuthenticationTokenException>(MissingAuthenticationTokenException.ErrorType);

        public RequestLimitExceededException AsRequestLimitExceededException() =>
            AsException<RequestLimitExceededException>(RequestLimitExceededException.ErrorType);

        public ResourceInUseException AsResourceInUseException() => 
            AsException<ResourceInUseException>(ResourceInUseException.ErrorType);

        public ResourceNotFoundException AsResourceNotFoundException() => 
            AsException<ResourceNotFoundException>(ResourceNotFoundException.ErrorType);

        public ThrottlingException AsThrottlingException() => 
            AsException<ThrottlingException>(ThrottlingException.ErrorType);

        public UnrecognizedClientException AsUnrecognizedClientException() => 
            AsException<UnrecognizedClientException>(UnrecognizedClientException.ErrorType);

        public ValidationException AsValidationException() => 
            AsException<ValidationException>(ValidationException.ErrorType);

        public IdempotentParameterMismatchException AsIdempotentParameterMismatchException() =>
            AsException<IdempotentParameterMismatchException>(IdempotentParameterMismatchException.ErrorType);

        public TransactionInProgressException AsTransactionInProgressException() =>
            AsException<TransactionInProgressException>(TransactionInProgressException.ErrorType);

        private TException AsException<TException>(OpErrorType expectedType) where TException : DdbException => Exception switch
        {
            TException ex => ex,
            _ => throw new InvalidOperationException($"Operation error contains '{ErrorType}' value instead of '{expectedType}'.")
        };
    }

    internal static class OpResultExtensions
    {
        public static async Task<T> EnsureSuccess<T>(this Task<OpResult<T>> task)
        {
            var result = await task.ConfigureAwait(false);
            return result.EnsureSuccess();
        }

        public static async Task EnsureSuccess(this Task<OpResult> task)
        {
            var result = await task.ConfigureAwait(false);
            result.EnsureSuccess();
        }
    }

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
}