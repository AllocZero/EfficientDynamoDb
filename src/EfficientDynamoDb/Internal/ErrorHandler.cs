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
                            var type = error.Type;
                            var exceptionStart = error.Type?.LastIndexOf('#') ?? -1;
                            if (exceptionStart != -1)
                                type = error.Type!.Substring(exceptionStart + 1);
                            
                            if (type == "TransactionCanceledException")
                            {
                                var classInfo = metadata.GetOrAddClassInfo(typeof(TransactionCancelledResponse), typeof(JsonObjectDdbConverter<TransactionCancelledResponse>));
                                var transactionCancelledResponse = await EntityDdbJsonReader
                                    .ReadAsync<TransactionCancelledResponse>(recyclableStream, classInfo, metadata, false, cancellationToken: cancellationToken)
                                    .ConfigureAwait(false);
                                throw new TransactionCanceledException(transactionCancelledResponse.Value!.CancellationReasons, error.Message);
                            }
                            
                            throw type switch
                            {
                                "AccessDeniedException" => new AccessDeniedException(error.Message),
                                "ConditionalCheckFailedException" => new ConditionalCheckFailedException(error.Message),
                                "IncompleteSignatureException" => new IncompleteSignatureException(error.Message),
                                "ItemCollectionSizeLimitExceededException" => new ItemCollectionSizeLimitExceededException(error.Message),
                                "LimitExceededException" => new LimitExceededException(error.Message),
                                "MissingAuthenticationTokenException" => new MissingAuthenticationTokenException(error.Message),
                                "ProvisionedThroughputExceededException" => new ProvisionedThroughputExceededException(error.Message),
                                "RequestLimitExceeded" => new RequestLimitExceeded(error.Message),
                                "ResourceInUseException" => new ResourceInUseException(error.Message),
                                "ResourceNotFoundException" => new ResourceNotFoundException(error.Message),
                                "ThrottlingException" => new ThrottlingException(error.Message),
                                "UnrecognizedClientException" => new UnrecognizedClientException(error.Message),
                                "ValidationException" => new ValidationException(error.Message),
                                "IdempotentParameterMismatchException" => new IdempotentParameterMismatchException(error.Message),
                                "TransactionInProgressException" => new TransactionInProgressException(error.Message),
                                _ => new DdbException(error.Message ?? type ?? string.Empty)
                            };
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