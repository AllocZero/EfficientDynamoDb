using System;
using System.Net.Http;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Config;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Signing.Builders;
using EfficientDynamoDb.Internal.Signing.Constants;
using EfficientDynamoDb.Internal.Signing.Crypto;

namespace EfficientDynamoDb.Internal.Signing
{
    internal static class AwsRequestSigner
    {
        private const string InvalidRequestErrorMessage = "Request URI is invalid. It must either be an absolute URI or base address must be set";

        public static async ValueTask<string> CalculateContentHashAsync(HttpContent? content)
        {
            if (content == null)
                return SigningConstants.EmptyBodySha256;

            var stream = await content.ReadAsStreamAsync().ConfigureAwait(false);
            var hash = CryptoService.ComputeSha256Hash(stream);
            return hash.ToHex(true);
        }

        public static void Sign(HttpRequestMessage request, string contentHash, in SigningMetadata metadata)
        {
            ValidateInput(request, RegionEndpoint.ServiceName);
            UpdateRequestUri(metadata.BaseAddress, request);
            
            request.Headers.Add(HeaderKeys.XAmzDateHeader, metadata.Timestamp.ToIso8601BasicDateTime());
            
            AddConditionalHeaders(request, contentHash, in metadata);
            
            var (canonicalRequest, signedHeaders) = CanonicalRequestBuilder.Build(request, contentHash, in metadata);
            var (stringToSign, credentialScope) = StringToSignBuilder.Build(canonicalRequest, in metadata);
            var authorizationHeader = AuthorizationHeaderBuilder.Build(signedHeaders, credentialScope, stringToSign, in metadata);
            
            request.Headers.TryAddWithoutValidation(HeaderKeys.AuthorizationHeader, authorizationHeader);
        }

        private static void AddConditionalHeaders(HttpRequestMessage request, string contentHash, in SigningMetadata metadata)
        {
            if (metadata.Credentials.UseToken)
                request.Headers.Add(HeaderKeys.XAmzSecurityTokenHeader, metadata.Credentials.Token);
            if (!request.Headers.Contains(HeaderKeys.HostHeader))
                request.Headers.Add(HeaderKeys.HostHeader, request.RequestUri.Host);
            // if (RegionEndpoint.ServiceName == ServiceNames.S3)
                // request.Headers.Add(HeaderKeys.XAmzContentSha256Header, contentHash);
        }

        private static void ValidateInput(HttpRequestMessage request, string serviceName)
        {
            if (serviceName == ServiceNames.S3 && request.Method == HttpMethod.Post)
                throw new NotSupportedException("S3 does not support POST. Use PUT instead.");
            if (request.Headers.Contains(HeaderKeys.XAmzDateHeader))
                throw new ArgumentException(GetHeaderExistsErrorMessage(HeaderKeys.XAmzDateHeader), nameof(request));
            if (request.Headers.Authorization != null || request.Headers.Contains(HeaderKeys.AuthorizationHeader))
                throw new ArgumentException(GetHeaderExistsErrorMessage(HeaderKeys.AuthorizationHeader), nameof(request));
        }

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

        private static string GetHeaderExistsErrorMessage(string header) => $"Request contains a header with name '{header}'";
    }
}