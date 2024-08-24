using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Configs.Http;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.JsonConverters;
using EfficientDynamoDb.Internal.Signing;
using Microsoft.IO;

namespace EfficientDynamoDb.Internal
{
    internal class HttpApi
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask<HttpResponseMessage> SendAsync(DynamoDbContextConfig config, HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            try
            {
                int internalServerErrorRetries = 0,
                    limitExceededRetries = 0,
                    provisionedThroughputExceededRetries = 0,
                    requestLimitExceededRetries = 0,
                    serviceUnavailableRetries = 0,
                    throttlingRetries = 0;
                while (true)
                {
                    using var request = new HttpRequestMessage(HttpMethod.Post, config.RegionEndpoint.RequestUri);
                    request.Content = httpContent;

                    try
                    {
                        var httpClient = _httpClientFactory.CreateHttpClient();
                        var stream = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
                        var credentials = await config.CredentialsProvider.GetCredentialsAsync(cancellationToken).ConfigureAwait(false);
                            
                        var metadata = new SigningMetadata(config.RegionEndpoint, credentials, DateTime.UtcNow, httpClient.DefaultRequestHeaders, httpClient.BaseAddress);
                        AwsRequestSigner.Sign(request, (RecyclableMemoryStream) stream, in metadata);

                        var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

                        if (response.IsSuccessStatusCode)
                            return response;
                
                        var error = await ErrorHandler.ProcessErrorAsync(config.Metadata, response, cancellationToken).ConfigureAwait(false);
                        switch (error)
                        {
                            case ProvisionedThroughputExceededException when config.RetryStrategies.ProvisionedThroughputExceededStrategy.TryGetRetryDelay(provisionedThroughputExceededRetries++, out var delay):
                            case LimitExceededException when config.RetryStrategies.LimitExceededStrategy.TryGetRetryDelay(limitExceededRetries++, out delay):
                            case InternalServerErrorException when config.RetryStrategies.InternalServerErrorStrategy.TryGetRetryDelay(internalServerErrorRetries++, out delay):
                            case RequestLimitExceededException when config.RetryStrategies.RequestLimitExceededStrategy.TryGetRetryDelay(requestLimitExceededRetries++, out delay):
                            case ServiceUnavailableException when config.RetryStrategies.ServiceUnavailableStrategy.TryGetRetryDelay(serviceUnavailableRetries++, out delay):
                            case ThrottlingException when config.RetryStrategies.ThrottlingStrategy.TryGetRetryDelay(throttlingRetries++, out delay):
                                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                                break;
                            case not null:
                                throw error;
                        }
                    }
                    finally
                    {
                        request.Content = null;
                    }
                }
            }
            finally
            {
                httpContent.Dispose();
            }
        }

        public async ValueTask<TResponse> SendAsync<TResponse>(DynamoDbContextConfig config, HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            using var response = await SendAsync(config, httpContent, cancellationToken).ConfigureAwait(false);

            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return (await JsonSerializer.DeserializeAsync<TResponse>(responseStream,
                new JsonSerializerOptions {Converters = {new DdbEnumJsonConverterFactory(), new UnixDateTimeJsonConverter()}}, cancellationToken).ConfigureAwait(false))!;
        }
    }
}