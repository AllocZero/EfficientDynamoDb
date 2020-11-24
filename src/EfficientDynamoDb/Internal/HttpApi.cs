using System;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Internal.JsonConverters;
using EfficientDynamoDb.Internal.Signing;

namespace EfficientDynamoDb.Internal
{
    public class HttpApi
    {
        private const string ServiceName = "dynamodb";
        
        private readonly HttpClient _httpClient = new HttpClient();

        public async ValueTask<HttpResponseMessage> SendAsync(string region, AwsCredentials credentials, HttpContent httpContent)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, $"https://{ServiceName}.{region}.amazonaws.com")
            {
                Content = httpContent
            };

            var metadata = new SigningMetadata(region, ServiceName, credentials, DateTime.UtcNow, _httpClient.DefaultRequestHeaders, _httpClient.BaseAddress);
            var contentHash = await AwsRequestSigner.CalculateContentHashAsync(httpContent).ConfigureAwait(false);
            AwsRequestSigner.Sign(request, contentHash, metadata);

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
          
            if (!response.IsSuccessStatusCode)
                await ErrorHandler.ProcessErrorAsync(response).ConfigureAwait(false);

            return response;
        }

        public async ValueTask<TResponse> SendAsync<TResponse>(string region, AwsCredentials credentials, HttpContent httpContent)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, $"https://{ServiceName}.{region}.amazonaws.com")
            {
                Content = httpContent
            };

            var metadata = new SigningMetadata(region, ServiceName, credentials, DateTime.UtcNow, _httpClient.DefaultRequestHeaders, _httpClient.BaseAddress);
            var contentHash = await AwsRequestSigner.CalculateContentHashAsync(httpContent).ConfigureAwait(false);
            AwsRequestSigner.Sign(request, contentHash, metadata);

            using var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            
            if (!response.IsSuccessStatusCode)
                await ErrorHandler.ProcessErrorAsync(response).ConfigureAwait(false);

            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return (await JsonSerializer.DeserializeAsync<TResponse>(responseStream,
                new JsonSerializerOptions {Converters = {new DdbEnumJsonConverterFactory(), new UnixDateTimeJsonConverter()}}).ConfigureAwait(false))!;
        }

    }
}