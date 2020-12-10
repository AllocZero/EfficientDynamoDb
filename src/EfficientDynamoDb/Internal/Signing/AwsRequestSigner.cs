using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context.Config;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Signing.Builders;
using EfficientDynamoDb.Internal.Signing.Crypto;
using Microsoft.IO;

namespace EfficientDynamoDb.Internal.Signing
{
    internal static class AwsRequestSigner
    {
        private const string InvalidRequestErrorMessage = "Request URI is invalid. It must either be an absolute URI or base address must be set";

        public static void Sign(HttpRequestMessage request, RecyclableMemoryStream content, in SigningMetadata metadata)
        {
            ValidateInput(request, RegionEndpoint.ServiceName);
            UpdateRequestUri(metadata.BaseAddress, request);

            request.Headers.Add(HeaderKeys.XAmzDateHeader, metadata.TimestampIso8601BasicDateTimeString);

            Span<byte> contentHash = stackalloc byte[32];
            CalculateContentHash(content, ref contentHash);
            AddConditionalHeaders(request, in metadata);

            // By default there are no default headers and 36 bytes are enough for 3 main headers: host;x-amz-date;x-amz-security-token
            Span<char> signedHeadersBuffer = metadata.HasDefaultRequestHeaders ? stackalloc char[96] : stackalloc char[36];
            
            // We don't know exact buffer size, but general tests show amount of chars used less than 200 and 256 bytes should be enough
            // In worst case scenario array from the pool is taken
            Span<char> initialBuffer = stackalloc char[NoAllocStringBuilder.MaxStackAllocSize];
            
            var signedHeadersBuilder = new NoAllocStringBuilder(in signedHeadersBuffer, false);
            var builder = new NoAllocStringBuilder(in initialBuffer, true);
            try
            {
                CanonicalRequestBuilder.Build(request, in contentHash, in metadata, ref builder, ref signedHeadersBuilder);
                StringToSignBuilder.Build(ref builder, in metadata);
                var authorizationHeader = AuthorizationHeaderBuilder.Build(ref builder, ref signedHeadersBuilder, in metadata);
                
                request.Headers.TryAddWithoutValidation(HeaderKeys.AuthorizationHeader, authorizationHeader);
            }
            finally
            {
                builder.Dispose();
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CalculateContentHash(RecyclableMemoryStream stream, ref Span<byte> hash)
        {
            var sequence = stream.GetReadOnlySequence();

            var data = sequence.IsSingleSegment ? sequence.First : stream.GetBuffer();

            CryptoService.ComputeSha256Hash(data.Span, hash, out _);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AddConditionalHeaders(HttpRequestMessage request, in SigningMetadata metadata)
        {
            if (metadata.Credentials.UseToken)
                request.Headers.Add(HeaderKeys.XAmzSecurityTokenHeader, metadata.Credentials.Token);
            if (!request.Headers.Contains(HeaderKeys.HostHeader))
                request.Headers.Add(HeaderKeys.HostHeader, request.RequestUri.Host);
            // if (RegionEndpoint.ServiceName == ServiceNames.S3)
                // request.Headers.Add(HeaderKeys.XAmzContentSha256Header, contentHash);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ValidateInput(HttpRequestMessage request, string serviceName)
        {
            if (serviceName == ServiceNames.S3 && request.Method == HttpMethod.Post)
                throw new NotSupportedException("S3 does not support POST. Use PUT instead.");
            if (request.Headers.Contains(HeaderKeys.XAmzDateHeader))
                throw new ArgumentException(GetHeaderExistsErrorMessage(HeaderKeys.XAmzDateHeader), nameof(request));
            if (request.Headers.Authorization != null || request.Headers.Contains(HeaderKeys.AuthorizationHeader))
                throw new ArgumentException(GetHeaderExistsErrorMessage(HeaderKeys.AuthorizationHeader), nameof(request));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void UpdateRequestUri(Uri? baseAddress, HttpRequestMessage request)
        {
            request.RequestUri = request.RequestUri switch
            {
                null when baseAddress != null => baseAddress,
                null => throw new InvalidOperationException(InvalidRequestErrorMessage),
                _ when !request.RequestUri.IsAbsoluteUri && baseAddress != null => new Uri(baseAddress, request.RequestUri),
                _ when !request.RequestUri.IsAbsoluteUri && baseAddress == null => throw new InvalidOperationException(InvalidRequestErrorMessage),
                { } currentUri => currentUri
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetHeaderExistsErrorMessage(string header) => $"Request contains a header with name '{header}'";
    }
}