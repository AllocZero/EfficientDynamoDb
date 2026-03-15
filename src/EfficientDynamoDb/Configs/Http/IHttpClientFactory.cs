using System.Net.Http;

namespace EfficientDynamoDb.Configs.Http
{
    public interface IHttpClientFactory
    {
        /// <summary>
        /// Creates or returns an <see cref="HttpClient"/> used for DynamoDB requests.
        /// </summary>
        /// <remarks>
        /// The returned <see cref="HttpClient"/> must have <c>AutomaticDecompression</c> set to <see cref="System.Net.DecompressionMethods.None"/>.
        /// EfficientDynamoDb handles gzip decompression manually, and enabling automatic decompression on the handler
        /// will interfere with CRC verification and error response parsing, leading to incorrect behavior.
        /// </remarks>
        HttpClient CreateHttpClient();
    }
}