using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Config;
using EfficientDynamoDb.Internal.JsonConverters;
using EfficientDynamoDb.Internal.Signing;

namespace EfficientDynamoDb.Internal
{
    internal class HttpApi
    {
        private readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler {AutomaticDecompression = DecompressionMethods.GZip});

        public async ValueTask<HttpResponseMessage> SendAsync(RegionEndpoint regionEndpoint, AwsCredentials credentials, HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, regionEndpoint.RequestUri)
            {
                Content = httpContent
            };

            var metadata = new SigningMetadata(regionEndpoint, credentials, DateTime.UtcNow, _httpClient.DefaultRequestHeaders, _httpClient.BaseAddress);
            var contentHash = await AwsRequestSigner.CalculateContentHashAsync(httpContent).ConfigureAwait(false);
            AwsRequestSigner.Sign(request, contentHash, metadata);

            var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
          
            if (!response.IsSuccessStatusCode)
                await ErrorHandler.ProcessErrorAsync(response, cancellationToken).ConfigureAwait(false);

            return response;
        }

        public async ValueTask<TResponse> SendAsync<TResponse>(RegionEndpoint regionEndpoint, AwsCredentials credentials, HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, regionEndpoint.RequestUri)
            {
                Content = httpContent
            };

            var metadata = new SigningMetadata(regionEndpoint, credentials, DateTime.UtcNow, _httpClient.DefaultRequestHeaders, _httpClient.BaseAddress);
            var contentHash = await AwsRequestSigner.CalculateContentHashAsync(httpContent).ConfigureAwait(false);
            AwsRequestSigner.Sign(request, contentHash, metadata);

            using var response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            
            if (!response.IsSuccessStatusCode)
                await ErrorHandler.ProcessErrorAsync(response, cancellationToken).ConfigureAwait(false);

            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return (await JsonSerializer.DeserializeAsync<TResponse>(responseStream,
                new JsonSerializerOptions {Converters = {new DdbEnumJsonConverterFactory(), new UnixDateTimeJsonConverter()}}, cancellationToken).ConfigureAwait(false))!;
        }

    }
}