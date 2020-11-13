using System.Buffers;
using System.Net.Http;
using System.Threading.Tasks;
using Amazon.Runtime;

namespace EfficientDynamoDb.Internal
{
    public class HttpApi
    {
        private const string ServiceName = "dynamodb";
        
        private readonly HttpClient _httpClient = new HttpClient();

        public async ValueTask<string> SendAsync(string region, ImmutableCredentials credentials, HttpContent httpContent)
        {
            var response = await _httpClient.PostAsync($"https://{ServiceName}.{region}.amazonaws.com", httpContent, region, ServiceName, credentials).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
        
    }
}