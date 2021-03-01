using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Internal.Reader;

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
                            if (error.Type == "com.amazonaws.dynamodb.v20120810#TransactionCanceledException")
                            {
                                var transactionCancelledResponse = await EntityDdbJsonReader.ReadAsync<TransactionCancelledResponse>(recyclableStream, metadata, false, cancellationToken).ConfigureAwait(false);
                                throw new TransactionCanceledException(transactionCancelledResponse.Value!.CancellationReasons, error.Message);
                            }
                            
                            throw error.Type switch
                            {
                                "com.amazonaws.dynamodb.v20120810#AccessDeniedException" => new AccessDeniedException(error.Message),
                                "com.amazonaws.dynamodb.v20120810#ConditionalCheckFailedException" => new ConditionalCheckFailedException(error.Message),
                                "com.amazonaws.dynamodb.v20120810#IncompleteSignatureException" => new IncompleteSignatureException(error.Message),
                                "com.amazonaws.dynamodb.v20120810#ItemCollectionSizeLimitExceededException" => new ItemCollectionSizeLimitExceededException(error.Message),
                                "com.amazonaws.dynamodb.v20120810#LimitExceededException" => new LimitExceededException(error.Message),
                                "com.amazonaws.dynamodb.v20120810#MissingAuthenticationTokenException" => new MissingAuthenticationTokenException(error.Message),
                                "com.amazonaws.dynamodb.v20120810#ProvisionedThroughputExceededException" => new ProvisionedThroughputExceededException(error.Message),
                                "com.amazonaws.dynamodb.v20120810#RequestLimitExceeded" => new RequestLimitExceeded(error.Message),
                                "com.amazonaws.dynamodb.v20120810#ResourceInUseException" => new ResourceInUseException(error.Message),
                                "com.amazonaws.dynamodb.v20120810#ResourceNotFoundException" => new ResourceNotFoundException(error.Message),
                                "com.amazonaws.dynamodb.v20120810#ThrottlingException" => new ThrottlingException(error.Message),
                                "com.amazonaws.dynamodb.v20120810#UnrecognizedClientException" => new UnrecognizedClientException(error.Message),
                                "com.amazonaws.dynamodb.v20120810#ValidationException" => new ValidationException(error.Message),
                                "com.amazonaws.dynamodb.v20120810#IdempotentParameterMismatchException" => new IdempotentParameterMismatchException(error.Message),
                                "com.amazonaws.dynamodb.v20120810#TransactionInProgressException" => new TransactionInProgressException(error.Message),
                                _ => new DdbException(error.Message)
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
            public string Type { get; }
        
            [JsonPropertyName("message")]
            public string Message { get; }

            [JsonConstructor]
            public Error(string type, string message)
            {
                Type = type;
                Message = message;
            }
        }
    }
}