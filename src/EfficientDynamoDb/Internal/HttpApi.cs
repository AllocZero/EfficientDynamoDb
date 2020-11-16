using System;
using System.Net.Http;
using System.Threading.Tasks;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Internal.Signing;

namespace EfficientDynamoDb.Internal
{
    public class HttpApi
    {
        private const string ServiceName = "dynamodb";
        
        private readonly HttpClient _httpClient = new HttpClient();

        public async ValueTask<string> SendAsync(string region, ImmutableCredentials credentials, HttpContent httpContent)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{ServiceName}.{region}.amazonaws.com")
            {
                Content = httpContent
            };

            var metadata = new SigningMetadata(region, ServiceName, credentials, DateTime.UtcNow, _httpClient.DefaultRequestHeaders, _httpClient.BaseAddress);
            var contentHash = await AwsRequestSigner.CalculateContentHashAsync(httpContent).ConfigureAwait(false);
            AwsRequestSigner.Sign(request, contentHash, metadata);

            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        
    }
}