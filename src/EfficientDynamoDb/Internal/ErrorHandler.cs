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

namespace EfficientDynamoDb.Internal
{
    internal static class ErrorHandler
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        
        public static async ValueTask ProcessErrorAsync(DynamoDbContextMetadata metadata, HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            try
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                    throw new ServiceUnavailableException("DynamoDB is currently unavailable. (This should be a temporary state.)");

                var recyclableStream = DynamoDbHttpContent.MemoryStreamManager.GetStream();
                try
                {
                    await responseStream.CopyToAsync(recyclableStream, cancellationToken).ConfigureAwait(false);

                    recyclableStream.Position = 0;
                    var error = await JsonSerializer.DeserializeAsync<Error>(recyclableStream, SerializerOptions, cancellationToken).ConfigureAwait(false);
                    recyclableStream.Position = 0;
                    
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            await ProcessBadRequestAsync(metadata, recyclableStream, error, cancellationToken);
                            break;
                        case HttpStatusCode.InternalServerError:
                            throw new InternalServerErrorException(error.Message);
                        default:
                            throw new DdbException(error.Message);
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

        private static ValueTask ProcessBadRequestAsync(DynamoDbContextMetadata metadata, MemoryStream recyclableStream, Error error, CancellationToken cancellationToken)
        {
            if (error.Type is null)
                throw new DdbException(string.Empty);

            var type = error.Type.AsSpan();
            var exceptionStart = type.LastIndexOf('#');
            if (exceptionStart != -1)
                type = type.Slice(exceptionStart + 1);

            if (type.Equals("TransactionCanceledException", StringComparison.Ordinal))
                return ParseTransactionCancelledException();
            
            if (type.Equals("ConditionalCheckFailedException", StringComparison.Ordinal))
                return ParseConditionalCheckFailedException();

            if (type.Equals("ProvisionedThroughputExceededException", StringComparison.Ordinal))
                throw new ProvisionedThroughputExceededException(error.Message);
            if (type.Equals("AccessDeniedException", StringComparison.Ordinal))
                throw new AccessDeniedException(error.Message);
            if (type.Equals("IncompleteSignatureException", StringComparison.Ordinal))
                throw new IncompleteSignatureException(error.Message);
            if (type.Equals("ItemCollectionSizeLimitExceededException", StringComparison.Ordinal))
                throw new ItemCollectionSizeLimitExceededException(error.Message);
            if (type.Equals("LimitExceededException", StringComparison.Ordinal))
                throw new LimitExceededException(error.Message);
            if (type.Equals("MissingAuthenticationTokenException", StringComparison.Ordinal))
                throw new MissingAuthenticationTokenException(error.Message);
            if (type.Equals("RequestLimitExceeded", StringComparison.Ordinal))
                throw new RequestLimitExceededException(error.Message);
            if (type.Equals("ResourceInUseException", StringComparison.Ordinal))
                throw new ResourceInUseException(error.Message);
            if (type.Equals("ResourceNotFoundException", StringComparison.Ordinal))
                throw new ResourceNotFoundException(error.Message);
            if (type.Equals("ThrottlingException", StringComparison.Ordinal))
                throw new ThrottlingException(error.Message);
            if (type.Equals("UnrecognizedClientException", StringComparison.Ordinal))
                throw new UnrecognizedClientException(error.Message);
            if (type.Equals("ValidationException", StringComparison.Ordinal))
                throw new ValidationException(error.Message);
            if (type.Equals("IdempotentParameterMismatchException", StringComparison.Ordinal))
                throw new IdempotentParameterMismatchException(error.Message);
            if (type.Equals("TransactionInProgressException", StringComparison.Ordinal))
                throw new TransactionInProgressException(error.Message);

            throw new DdbException(error.Message ?? type.ToString());

            async ValueTask ParseTransactionCancelledException()
            {
                var classInfo = metadata.GetOrAddClassInfo(typeof(TransactionCancelledResponse), typeof(JsonObjectDdbConverter<TransactionCancelledResponse>));
                var transactionCancelledResponse = await EntityDdbJsonReader
                    .ReadAsync<TransactionCancelledResponse>(recyclableStream, classInfo, metadata, false, cancellationToken: cancellationToken)
                    .ConfigureAwait(false);
                throw new TransactionCanceledException(transactionCancelledResponse.Value!.CancellationReasons, error.Message);
            }

            async ValueTask ParseConditionalCheckFailedException()
            {
                var classInfo = metadata.GetOrAddClassInfo(typeof(ConditionalCheckFailedResponse),
                    typeof(JsonObjectDdbConverter<ConditionalCheckFailedResponse>));
                var conditionalCheckFailedResponse = await EntityDdbJsonReader.ReadAsync<ConditionalCheckFailedResponse>(recyclableStream,
                    classInfo, metadata, false, cancellationToken: cancellationToken).ConfigureAwait(false);
                throw new ConditionalCheckFailedException(conditionalCheckFailedResponse.Value!.Item, error.Message);
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