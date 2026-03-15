using System;
using System.IO;
using System.IO.Compression;
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

        public static async Task<DdbException> ProcessErrorAsync(DynamoDbContextMetadata metadata, HttpResponseMessage response, CancellationToken cancellationToken = default)
        {
            try
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                    return new ServiceUnavailableException("DynamoDB is currently unavailable. (This should be a temporary state.)");

                var hasGzipHeader = response.Content.Headers.ContentEncoding.Contains("gzip");
                await using var parsedStreamOwner = await DecodeErrorStreamAsync(responseStream, hasGzipHeader, cancellationToken).ConfigureAwait(false);

                var errorStream = parsedStreamOwner.Stream;
                var error = await JsonSerializer.DeserializeAsync<Error>(errorStream, SerializerOptions, cancellationToken).ConfigureAwait(false);
                errorStream.Position = 0;

                return response.StatusCode switch
                {
                    HttpStatusCode.BadRequest => await ProcessBadRequestAsync(metadata, errorStream, error, cancellationToken).ConfigureAwait(false),
                    HttpStatusCode.InternalServerError => new InternalServerErrorException(error.Message),
                    _ => new DdbException(error.Message)
                };
            }
            finally
            {
                response.Dispose();
            }
        }
        
        private static bool IsGzipEncoded(RecyclableMemoryStream stream)
        {
            Span<byte> magic = stackalloc byte[2];
            _ = stream.Read(magic);
            stream.Position = 0;
            return magic[0] == 0x1F && magic[1] == 0x8B;
        }

        private static async Task<DecodedStreamOwner> DecodeErrorStreamAsync(Stream responseStream, bool hasGzipHeader, CancellationToken ct = default)
        {
            var recyclableStream = new RecyclableMemoryStream(DynamoDbHttpContent.MemoryStreamManager);
            RecyclableMemoryStream? decompressedStream = null;
            try
            {
                await responseStream.CopyToAsync(recyclableStream, ct).ConfigureAwait(false);
                recyclableStream.Position = 0;
                if (!hasGzipHeader || recyclableStream.Length < 2)
                    return new(recyclableStream, null);

                // DynamoDB lies: error responses may carry Content-Encoding: gzip but the body is NOT gzipped.
                // When the header claims gzip, detect actual gzip by inspecting magic bytes (0x1F 0x8B).
                if (!IsGzipEncoded(recyclableStream))
                    return new(recyclableStream, null);

                // Rare path: error response was actually gzip-encoded — decompress eagerly into a pooled stream.
                decompressedStream = new RecyclableMemoryStream(DynamoDbHttpContent.MemoryStreamManager);
                await using (var gz = new GZipStream(recyclableStream, CompressionMode.Decompress, leaveOpen: true))
                {
                    await gz.CopyToAsync(decompressedStream, ct).ConfigureAwait(false);
                }

                recyclableStream.Position = 0;
                decompressedStream.Position = 0;
                return new(recyclableStream, decompressedStream);
            }
            catch (Exception)
            {
                await recyclableStream.DisposeAsync().ConfigureAwait(false);
                if (decompressedStream is not null)
                    await decompressedStream.DisposeAsync().ConfigureAwait(false);
                throw;
            }
        }

        private static ValueTask<DdbException> ProcessBadRequestAsync(DynamoDbContextMetadata metadata, Stream recyclableStream, Error error, CancellationToken cancellationToken)
        {
            if (error.Type is null)
                return new(new DdbException(string.Empty));

            var exceptionStart = error.Type.LastIndexOf('#');
            var type = exceptionStart != -1 ? error.Type.AsSpan(exceptionStart + 1) : error.Type.AsSpan();
            
            if (type.Equals("TransactionCanceledException", StringComparison.Ordinal))
                return ParseTransactionCancelledException();
            
            if (type.Equals("ConditionalCheckFailedException", StringComparison.Ordinal))
                return ParseConditionalCheckFailedException();
            
            if (type.Equals("ProvisionedThroughputExceededException", StringComparison.Ordinal))
                return new(new ProvisionedThroughputExceededException(error.Message));
            if (type.Equals("AccessDeniedException", StringComparison.Ordinal))
                return new(new AccessDeniedException(error.Message));
            if (type.Equals("IncompleteSignatureException", StringComparison.Ordinal))
                return new(new IncompleteSignatureException(error.Message));
            if (type.Equals("ItemCollectionSizeLimitExceededException", StringComparison.Ordinal))
                return new(new ItemCollectionSizeLimitExceededException(error.Message));
            if (type.Equals("LimitExceededException", StringComparison.Ordinal))
                return new(new LimitExceededException(error.Message));
            if (type.Equals("MissingAuthenticationTokenException", StringComparison.Ordinal))
                return new(new MissingAuthenticationTokenException(error.Message));
            if (type.Equals("RequestLimitExceeded", StringComparison.Ordinal))
                return new(new RequestLimitExceededException(error.Message));
            if (type.Equals("ResourceInUseException", StringComparison.Ordinal))
                return new(new ResourceInUseException(error.Message));
            if (type.Equals("ResourceNotFoundException", StringComparison.Ordinal))
                return new(new ResourceNotFoundException(error.Message));
            if (type.Equals("ThrottlingException", StringComparison.Ordinal))
                return new(new ThrottlingException(error.Message));
            if (type.Equals("UnrecognizedClientException", StringComparison.Ordinal))
                return new(new UnrecognizedClientException(error.Message));
            if (type.Equals("ValidationException", StringComparison.Ordinal))
                return new(new ValidationException(error.Message));
            if (type.Equals("IdempotentParameterMismatchException", StringComparison.Ordinal))
                return new(new IdempotentParameterMismatchException(error.Message));
            if (type.Equals("TransactionInProgressException", StringComparison.Ordinal))
                return new(new TransactionInProgressException(error.Message));

            return new(new DdbException(error.Message ?? type.ToString()));

            async ValueTask<DdbException> ParseTransactionCancelledException()
            {
                var classInfo = metadata.GetOrAddClassInfo(typeof(TransactionCancelledResponse), typeof(JsonObjectDdbConverter<TransactionCancelledResponse>));
                var transactionCancelledResponse = await EntityDdbJsonReader.ReadAsync<TransactionCancelledResponse>(recyclableStream, classInfo, metadata, 
                        false, cancellationToken: cancellationToken).ConfigureAwait(false);
                return new TransactionCanceledException(transactionCancelledResponse.Value!.CancellationReasons, error.Message);
            }

            async ValueTask<DdbException> ParseConditionalCheckFailedException()
            {
                var classInfo = metadata.GetOrAddClassInfo(typeof(ConditionalCheckFailedResponse),
                    typeof(JsonObjectDdbConverter<ConditionalCheckFailedResponse>));
                var conditionalCheckFailedResponse = await EntityDdbJsonReader.ReadAsync<ConditionalCheckFailedResponse>(recyclableStream,
                    classInfo, metadata, false, cancellationToken: cancellationToken).ConfigureAwait(false);
                return new ConditionalCheckFailedException(conditionalCheckFailedResponse.Value!.Item, error.Message);
            }
        }
        
        private readonly struct DecodedStreamOwner : IAsyncDisposable, IDisposable
        {
            private readonly RecyclableMemoryStream _original;
            private readonly RecyclableMemoryStream? _decompressed;

            public DecodedStreamOwner(RecyclableMemoryStream original, RecyclableMemoryStream? decompressed)
            {
                _original = original;
                _decompressed = decompressed;
            }

            public Stream Stream => _decompressed ?? _original;

            public async ValueTask DisposeAsync()
            {
                await _original.DisposeAsync().ConfigureAwait(false);
                if (_decompressed is not null)
                    await _decompressed.DisposeAsync().ConfigureAwait(false);
            }

            public void Dispose()
            {
                _original.Dispose();
                _decompressed?.Dispose();
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