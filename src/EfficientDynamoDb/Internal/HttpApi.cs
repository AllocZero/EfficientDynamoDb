using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Configs.Http;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.Exceptions;
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
            using var request = new HttpRequestMessage(HttpMethod.Post, config.RegionEndpoint.RequestUri)
            {
                Content = httpContent
            };

            int internalServerErrorRetries = 0,
                limitExceededRetries = 0,
                provisionedThroughputExceededRetries = 0,
                requestLimitExceededRetries = 0,
                serviceUnavailableRetries = 0,
                throttlingRetries = 0;
            while (true)
            {
                TimeSpan delay;
                try
                {
                    var httpClient = _httpClientFactory.CreateHttpClient();
                    var metadata = new SigningMetadata(config.RegionEndpoint, config.Credentials, DateTime.UtcNow, httpClient.DefaultRequestHeaders, httpClient.BaseAddress);
                    var stream = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
                    AwsRequestSigner.Sign(request, (RecyclableMemoryStream)stream, metadata);

                    var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
          
                    if (!response.IsSuccessStatusCode)
                        await ErrorHandler.ProcessErrorAsync(response, cancellationToken).ConfigureAwait(false);

                    return response; 
                }
                catch (InternalServerErrorException)
                {
                    if (!config.RetryStrategies.InternalServerErrorStrategy.TryGetRetryDelay(internalServerErrorRetries++, out delay))
                        throw;
                }
                catch (LimitExceededException)
                {
                    if (!config.RetryStrategies.LimitExceededStrategy.TryGetRetryDelay(limitExceededRetries++, out delay))
                        throw;
                }
                catch (ProvisionedThroughputExceededException)
                {
                    if (!config.RetryStrategies.ProvisionedThroughputExceededStrategy.TryGetRetryDelay(provisionedThroughputExceededRetries++, out delay))
                        throw;
                }
                catch (RequestLimitExceeded)
                {
                    if (!config.RetryStrategies.RequestLimitExceededStrategy.TryGetRetryDelay(requestLimitExceededRetries++, out delay))
                        throw;
                }
                catch (ServiceUnavailableException)
                {
                    if (!config.RetryStrategies.ServiceUnavailableStrategy.TryGetRetryDelay(serviceUnavailableRetries++, out delay))
                        throw;
                }
                catch (ThrottlingException)
                {
                    if (!config.RetryStrategies.ThrottlingStrategy.TryGetRetryDelay(throttlingRetries++, out delay))
                        throw;
                }
                
                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
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