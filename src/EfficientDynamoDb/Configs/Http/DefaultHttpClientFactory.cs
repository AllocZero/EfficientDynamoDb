using System.Net;
using System.Net.Http;

namespace EfficientDynamoDb.Configs.Http
{
    internal class DefaultHttpClientFactory : IHttpClientFactory
    {
        public static readonly DefaultHttpClientFactory Instance = new DefaultHttpClientFactory();
        
        private readonly HttpClient _httpClient;

        private DefaultHttpClientFactory()
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip
            });
        }

        public DefaultHttpClientFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HttpClient CreateHttpClient()
        {
            return _httpClient;
        }
    }
}