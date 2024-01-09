using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.JsonConverters;
using EfficientDynamoDb.Internal.Signing;
using Microsoft.IO;

namespace EfficientDynamoDb.Internal
{
    internal class HttpApi
    {
        private readonly DynamoDbContextConfig _config;
        private readonly string _serviceName;
        private readonly string _requestUri;

        public HttpApi(DynamoDbContextConfig config, string serviceName)
        {
            _config = config;
            _serviceName = serviceName;
            _requestUri = config.RegionEndpoint.BuildRequestUri(serviceName);
        }

        public async ValueTask<HttpResponseMessage> SendAsync(HttpContent httpContent, CancellationToken cancellationToken = default)
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
                    TimeSpan delay;
                    try
                    {
                        using var request = new HttpRequestMessage(HttpMethod.Post, _requestUri)
                        {
                            Content = httpContent
                        };

                        try
                        {
                            var httpClient = _config.HttpClientFactory.CreateHttpClient();
                            var stream = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
                            var credentials = await _config.CredentialsProvider.GetCredentialsAsync(cancellationToken).ConfigureAwait(false);
                            
                            var metadata = new SigningMetadata(_config.RegionEndpoint, credentials, DateTime.UtcNow, httpClient.DefaultRequestHeaders,
                                httpClient.BaseAddress, _serviceName);
                            AwsRequestSigner.Sign(request, (RecyclableMemoryStream) stream, in metadata);

                            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

                            if (!response.IsSuccessStatusCode)
                                await ErrorHandler.ProcessErrorAsync(_config.Metadata, response, cancellationToken).ConfigureAwait(false);

                            return response;
                        }
                        finally
                        {
                            request.Content = null;
                        }
                    }
                    catch (InternalServerErrorException)
                    {
                        if (!_config.RetryStrategies.InternalServerErrorStrategy.TryGetRetryDelay(internalServerErrorRetries++, out delay))
                            throw;
                    }
                    catch (LimitExceededException)
                    {
                        if (!_config.RetryStrategies.LimitExceededStrategy.TryGetRetryDelay(limitExceededRetries++, out delay))
                            throw;
                    }
                    catch (ProvisionedThroughputExceededException)
                    {
                        if (!_config.RetryStrategies.ProvisionedThroughputExceededStrategy.TryGetRetryDelay(provisionedThroughputExceededRetries++, out delay))
                            throw;
                    }
                    catch (RequestLimitExceededException)
                    {
                        if (!_config.RetryStrategies.RequestLimitExceededStrategy.TryGetRetryDelay(requestLimitExceededRetries++, out delay))
                            throw;
                    }
                    catch (ServiceUnavailableException)
                    {
                        if (!_config.RetryStrategies.ServiceUnavailableStrategy.TryGetRetryDelay(serviceUnavailableRetries++, out delay))
                            throw;
                    }
                    catch (ThrottlingException)
                    {
                        if (!_config.RetryStrategies.ThrottlingStrategy.TryGetRetryDelay(throttlingRetries++, out delay))
                            throw;
                    }

                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                }
            }
            finally
            {
                httpContent.Dispose();
            }
        }

        public async ValueTask<TResponse> SendAsync<TResponse>(HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            using var response = await SendAsync(httpContent, cancellationToken).ConfigureAwait(false);

            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return (await JsonSerializer.DeserializeAsync<TResponse>(responseStream,
                new JsonSerializerOptions {Converters = {new DdbEnumJsonConverterFactory(), new UnixDateTimeJsonConverter()}}, cancellationToken).ConfigureAwait(false))!;
        }
    }
}