/*******************************************************************************
 *  Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *  this file except in compliance with the License. A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 *  or in the "license" file accompanying this file.
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *  specific language governing permissions and limitations under the License.
 * *****************************************************************************
 *    __  _    _  ___
 *   (  )( \/\/ )/ __)
 *   /__\ \    / \__ \
 *  (_)(_) \/\/  (___/
 *
 *  AWS SDK for .NET
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Amazon.Util
{
    /// <summary>
    /// AwsHttpClient is a wrapper around HttpClient class of the System.Net.Http library.
    /// The wrapper has been added so as to remove System.Net.Http dependencies from the Services
    /// and migrate it to a Core level dependecy only. This avoids version clashes when a service 
    /// and the Core depends on different versions of the System.Net.Http library. This is a Service 
    /// level utility method
    /// </summary>
    public class AWSHttpClient : IDisposable
    {
        private HttpClient _httpClient;
        private bool disposed = false;

        /// <summary>
        /// AWSHttpClient wrapper that is wrapped around
        /// HttpClient default wrapper.
        /// </summary>
        public AWSHttpClient()
        {
            _httpClient = new HttpClient();
        }

        internal AWSHttpClient(IWebProxy proxy, bool useProxy)
        {
            _httpClient = new HttpClient(new HttpClientHandler
            {
                Proxy = proxy,
                UseProxy = useProxy
            });
        }
        internal AWSHttpClient(HttpMessageHandler handler)
        {
            _httpClient = new HttpClient(handler);
        }

        internal AWSHttpClient(HttpMessageHandler handler, bool disposeHandler)
        {
            _httpClient = new HttpClient(handler, disposeHandler);
        }

        /// <summary>
        /// AWSHttpClient BaseAddress property that is wrapper for
        /// HttpClient BaseAddress property.
        /// </summary>
        public Uri BaseAddress
        {
            get
            {
                return _httpClient.BaseAddress;
            }
            set
            {
                _httpClient.BaseAddress = value;
            }
        }

        /// <summary>
        /// AWSHttpClient Timeout property that is wrapper for
        /// HttpClient Timeout property.
        /// </summary>
        public TimeSpan Timeout
        {
            get
            {
                return _httpClient.Timeout;
            }
            set
            {
                _httpClient.Timeout = value;
            }
        }

        /// <summary>
        /// AWSHttpClient MaxResponseContentBufferSize property that is wrapper for
        /// HttpClient MaxResponseContentBufferSize property.
        /// </summary>
        public long MaxResponseContentBufferSize
        {
            get
            {
                return _httpClient.MaxResponseContentBufferSize;
            }
            set
            {
                _httpClient.MaxResponseContentBufferSize = value;
            }
        }

        /// <summary>
        /// AWSHttpClient GetStreamAsync that accepts the requester's URI
        /// and make a HttpClient.GetStreamAsync call.
        /// </summary>
        /// <param name="requestUri">Requester Uri</param>
        public Task<Stream> GetStreamAsync(string requestUri)
        {
            return _httpClient.GetStreamAsync(requestUri);
        }

        /// <summary>
        /// Wrapper method that accepts a request uri, stream content and headers and makes a sendAsync call.
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        public Task PutRequestUriAsync(string requestUri, AWSStreamContent content, IDictionary<string, string> requestHeaders)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, requestUri);
            request.Content = content.StreamContent;
            foreach (var header in requestHeaders)
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            return _httpClient.SendAsync(request);
        }

        /// <summary>
        /// Wrapper for HttpClient's HttpRequestMessage and SendAsync methods.
        /// </summary>
        /// <param name="httpMethodValue"></param>
        /// <param name="url"></param>
        /// <returns>Returns response headers information- header name,value and status code</returns>
        public async Task<List<Tuple<string, IEnumerable<string>, HttpStatusCode>>> GetResponseHeadersAsync(string httpMethodValue, string url)
        {
            HttpMethod httpMethod = new HttpMethod(httpMethodValue);
            var headers = new List<Tuple<string, IEnumerable<string>, HttpStatusCode>>();
            var request = new HttpRequestMessage(httpMethod, url);
            var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
            foreach (var header in response.Headers)
            {
                headers.Add(new Tuple<string, IEnumerable<string>, HttpStatusCode>(header.Key, header.Value, response.StatusCode));
            }

            return headers;
        }

        /// <summary>
        /// Wrapper for HttpClient Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _httpClient.Dispose();
            }
            disposed = true;

        }

        /// <summary>
        /// Method that checks if the passed exception is of type
        /// HttpRequestException
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static bool IsHttpInnerException(Exception exception)
        {
            return (exception is HttpRequestException);
        }
    }
}
