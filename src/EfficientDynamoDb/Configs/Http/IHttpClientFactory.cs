using System.Net.Http;

namespace EfficientDynamoDb.Configs.Http
{
    public interface IHttpClientFactory
    {
        HttpClient CreateHttpClient();
    }
}