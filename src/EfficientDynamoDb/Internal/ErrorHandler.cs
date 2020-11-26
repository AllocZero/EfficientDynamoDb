using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.Internal
{
    public static class ErrorHandler
    {
        public static async ValueTask ProcessErrorAsync(HttpResponseMessage response)
        {
            try
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                    throw new ServiceUnavailableException("DynamoDB is currently unavailable. (This should be a temporary state.)");
                
                var error = await JsonSerializer.DeserializeAsync<Error>(responseStream).ConfigureAwait(false);

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
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
                            "com.amazonaws.dynamodb.v20120810#TransactionCanceledException" => new TransactionCanceledException(error.Message),
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