using System;
using System.Net.Http;
using System.Threading.Tasks;
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
        
        public static async ValueTask SignAsync(HttpRequestMessage request, SigningMetadata metadata)
        {
            if (metadata.ServiceName == ServiceNames.S3 && request.Method == HttpMethod.Post) throw new NotSupportedException("S3 does not support POST. Use PUT instead.");
            if (request.Headers.Contains(HeaderKeys.XAmzDateHeader))
                throw new ArgumentException(GetHeaderExistsErrorMessage(HeaderKeys.XAmzDateHeader), nameof(request));
            if (request.Headers.Authorization != null || request.Headers.Contains(HeaderKeys.AuthorizationHeader))
                throw new ArgumentException(GetHeaderExistsErrorMessage(HeaderKeys.AuthorizationHeader), nameof(request));

            UpdateRequestUri(metadata.BaseAddress, request);

            var contentHash = await CalculateContentHashAsync(request.Content);

            // Add required headers
            request.Headers.Add(HeaderKeys.XAmzDateHeader, metadata.Timestamp.ToIso8601BasicDateTime());

            // Add conditional headers
            if (metadata.Credentials.UseToken)
                request.Headers.Add(HeaderKeys.XAmzSecurityTokenHeader, metadata.Credentials.Token);
            if (!request.Headers.Contains(HeaderKeys.HostHeader))
                request.Headers.Add(HeaderKeys.HostHeader, request.RequestUri.Host);
            if (metadata.ServiceName == ServiceNames.S3)
                request.Headers.Add(HeaderKeys.XAmzContentSha256Header, contentHash);


            // Build the canonical request
            var (canonicalRequest, signedHeaders) = CanonicalRequestBuilder.Build(request, contentHash, metadata);

            // Build the string to sign
            var (stringToSign, credentialScope) = StringToSignBuilder.Build(canonicalRequest, metadata);

            // Build the authorization header
            var authorizationHeader = AuthorizationHeaderBuilder.Build(signedHeaders, credentialScope, stringToSign, metadata);

            // Add the authorization header
            request.Headers.TryAddWithoutValidation(HeaderKeys.AuthorizationHeader, authorizationHeader);
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

        private static async ValueTask<string> CalculateContentHashAsync(HttpContent? content)
        {
            if (content == null)
                return SigningConstants.EmptyBodySha256;

            var stream = await content.ReadAsStreamAsync().ConfigureAwait(false);
            var hash = CryptoService.ComputeSha256Hash(stream);
            return hash.ToHex(true);
        }

        private static string GetHeaderExistsErrorMessage(string header) => $"Request contains a header with name '{header}'";
    }
}