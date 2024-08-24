using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Converters.Json;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Internal.Reader;
using EfficientDynamoDb.Operations.Shared;
using Microsoft.IO;

namespace EfficientDynamoDb.Internal
{
    internal static class ErrorHandler
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        
        public static async Task<Exception> ProcessErrorAsync(DynamoDbContextMetadata metadata, HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            try
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                    return new ServiceUnavailableException("DynamoDB is currently unavailable. (This should be a temporary state.)");

                var recyclableStream = new RecyclableMemoryStream(DynamoDbHttpContent.MemoryStreamManager);
                try
                {
                    await responseStream.CopyToAsync(recyclableStream, cancellationToken).ConfigureAwait(false);

                    recyclableStream.Position = 0;
                    var error = await JsonSerializer.DeserializeAsync<Error>(recyclableStream, SerializerOptions, cancellationToken).ConfigureAwait(false);
                    recyclableStream.Position = 0;
                    
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            return await ProcessBadRequestAsync(metadata, recyclableStream, error, cancellationToken).ConfigureAwait(false);
                        case HttpStatusCode.InternalServerError:
                            return new InternalServerErrorException(error.Message);
                        default:
                            return new DdbException(error.Message);
                    }
                }
                finally
                {
                    await recyclableStream.DisposeAsync().ConfigureAwait(false);
                }
            }
            finally
            {
                response.Dispose();
            }
        }

        private static ValueTask<Exception> ProcessBadRequestAsync(DynamoDbContextMetadata metadata, MemoryStream recyclableStream, Error error, CancellationToken cancellationToken)
        {
            if (error.Type is null)
                return new ValueTask<Exception>(new DdbException(string.Empty));

            var exceptionStart = error.Type.LastIndexOf('#');
            var type = exceptionStart != -1 ? error.Type.AsSpan(exceptionStart + 1) : error.Type.AsSpan();
            
            if (type.Equals("TransactionCanceledException", StringComparison.Ordinal))
                return ParseTransactionCancelledException();
            
            if (type.Equals("ConditionalCheckFailedException", StringComparison.Ordinal))
                return ParseConditionalCheckFailedException();
            
            if (type.Equals("ProvisionedThroughputExceededException", StringComparison.Ordinal))
                return new ValueTask<Exception>(new ProvisionedThroughputExceededException(error.Message));
            if (type.Equals("AccessDeniedException", StringComparison.Ordinal))
                return new ValueTask<Exception>(new AccessDeniedException(error.Message));
            if (type.Equals("IncompleteSignatureException", StringComparison.Ordinal))
                return new ValueTask<Exception>(new IncompleteSignatureException(error.Message));
            if (type.Equals("ItemCollectionSizeLimitExceededException", StringComparison.Ordinal))
                return new ValueTask<Exception>(new ItemCollectionSizeLimitExceededException(error.Message));
            if (type.Equals("LimitExceededException", StringComparison.Ordinal))
                return new ValueTask<Exception>(new LimitExceededException(error.Message));
            if (type.Equals("MissingAuthenticationTokenException", StringComparison.Ordinal))
                return new ValueTask<Exception>(new MissingAuthenticationTokenException(error.Message));
            if (type.Equals("RequestLimitExceeded", StringComparison.Ordinal))
                return new ValueTask<Exception>(new RequestLimitExceededException(error.Message));
            if (type.Equals("ResourceInUseException", StringComparison.Ordinal))
                return new ValueTask<Exception>(new ResourceInUseException(error.Message));
            if (type.Equals("ResourceNotFoundException", StringComparison.Ordinal))
                return new ValueTask<Exception>(new ResourceNotFoundException(error.Message));
            if (type.Equals("ThrottlingException", StringComparison.Ordinal))
                return new ValueTask<Exception>(new ThrottlingException(error.Message));
            if (type.Equals("UnrecognizedClientException", StringComparison.Ordinal))
                return new ValueTask<Exception>(new UnrecognizedClientException(error.Message));
            if (type.Equals("ValidationException", StringComparison.Ordinal))
                return new ValueTask<Exception>(new ValidationException(error.Message));
            if (type.Equals("IdempotentParameterMismatchException", StringComparison.Ordinal))
                return new ValueTask<Exception>(new IdempotentParameterMismatchException(error.Message));
            if (type.Equals("TransactionInProgressException", StringComparison.Ordinal))
                return new ValueTask<Exception>(new TransactionInProgressException(error.Message));

            return new ValueTask<Exception>(new DdbException(error.Message ?? type.ToString()));

            async ValueTask<Exception> ParseTransactionCancelledException()
            {
                var classInfo = metadata.GetOrAddClassInfo(typeof(TransactionCancelledResponse), typeof(JsonObjectDdbConverter<TransactionCancelledResponse>));
                var transactionCancelledResponse = await EntityDdbJsonReader.ReadAsync<TransactionCancelledResponse>(recyclableStream, classInfo, metadata, 
                        false, cancellationToken: cancellationToken).ConfigureAwait(false);
                return new TransactionCanceledException(transactionCancelledResponse.Value!.CancellationReasons, error.Message);
            }

            async ValueTask<Exception> ParseConditionalCheckFailedException()
            {
                var classInfo = metadata.GetOrAddClassInfo(typeof(ConditionalCheckFailedResponse),
                    typeof(JsonObjectDdbConverter<ConditionalCheckFailedResponse>));
                var conditionalCheckFailedResponse = await EntityDdbJsonReader.ReadAsync<ConditionalCheckFailedResponse>(recyclableStream,
                    classInfo, metadata, false, cancellationToken: cancellationToken).ConfigureAwait(false);
                return new ConditionalCheckFailedException(conditionalCheckFailedResponse.Value!.Item, error.Message);
            }
        }

        private readonly struct Error
        {
            [JsonPropertyName("__type")]
            public string? Type { get; }
        
            [JsonPropertyName("message")]
            public string Message { get; }

            [JsonConstructor]
            public Error(string? type, string message)
            {
                Type = type;
                Message = message;
            }
        }
    }
}