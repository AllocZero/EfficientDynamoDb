/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */

using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
#if AWS_ASYNC_API
using System.Threading;
using System.Threading.Tasks;
#endif

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// The request factory for System.Net.HttpWebRequest.
    /// </summary>    
    public class HttpWebRequestFactory : IHttpRequestFactory<Stream>
    {
        private ILogger _logger;
        private static volatile bool _isProtocolUpdated;

        /// <summary>
        /// Some AWS services like Cloud 9 require at least TLS 1.1. Version of .NET Framework 4.5 and earlier 
        /// do not eanble TLS 1.1 and TLS 1.2 by default. This code adds those protocols if using an earlier 
        /// version of .NET that explicitly set the protocol and didn't have TLS 1.1 and TLS 1.2. 
        /// </summary>
        /// <param name="amazonSecurityProtocolManager"></param>
        public HttpWebRequestFactory(IAmazonSecurityProtocolManager amazonSecurityProtocolManager)
            : this(amazonSecurityProtocolManager, null)
        {
        }

        /// <summary>
        /// Some AWS services like Cloud 9 require at least TLS 1.1. Version of .NET Framework 4.5 and earlier 
        /// do not eanble TLS 1.1 and TLS 1.2 by default. This code adds those protocols if using an earlier 
        /// version of .NET that explicitly set the protocol and didn't have TLS 1.1 and TLS 1.2. 
        /// </summary>
        /// <param name="amazonSecurityProtocolManager"></param>
        /// <param name="logger"></param>
        public HttpWebRequestFactory(IAmazonSecurityProtocolManager amazonSecurityProtocolManager, ILogger logger)
        {
            _logger = logger ?? Logger.GetLogger(typeof(HttpWebRequestFactory));

            if (_isProtocolUpdated) return;

            if (!amazonSecurityProtocolManager.IsSecurityProtocolSystemDefault())
            {
                try
                {
                    amazonSecurityProtocolManager.UpdateProtocolsToSupported();
                }
                catch (Exception ex)
                {
                    if (ex is NotSupportedException)
                    {
                        _logger.InfoFormat(ex.Message);
                    }
                    else
                    {
                        _logger.InfoFormat("Unexpected error " + ex.GetType().Name +
                                           " encountered when trying to set Security Protocol.\n" + ex);
                    }
                }
            }
            _isProtocolUpdated = true;
        }

        /// <summary>
        /// Creates an HTTP request for the given URI.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        /// <returns>An HTTP request.</returns>
        public IHttpRequest<Stream> CreateHttpRequest(Uri requestUri)
        {
            return new HttpRequest(requestUri);
        }

        /// <summary>
        /// This method is used for unit testing purposes. It allows setting of the flag
        /// that indicates protocol setting was attempted.
        /// </summary>
        /// <param name="value">The new value</param>
        public static void SetIsProtocolUpdated(bool value)
        {
            _isProtocolUpdated = value;
        }

        /// <summary>
        /// Disposes the HttpWebRequestFactory.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }

    /// <summary>
    /// HTTP request wrapper for System.Net.HttpWebRequest.
    /// </summary>    
    public class HttpRequest : IHttpRequest<Stream>
    {
        private bool _isAborted = false;
        private HttpWebRequest _request;

        /// <summary>
        /// Constructor for HttpRequest.
        /// </summary>
        /// <param name="requestUri">The request URI.</param>
        public HttpRequest(Uri requestUri)
        {
            _request = WebRequest.Create(requestUri) as HttpWebRequest;
        }

        /// <summary>
        /// The underlying HTTP web request.
        /// </summary>
        public HttpWebRequest Request
        {
            get { return _request; }
        }

        /// <summary>
        /// The HTTP method or verb.
        /// </summary>
        public string Method
        {
            get { return _request.Method; }
            set { _request.Method = value; }
        }

        /// <summary>
        /// The request URI.
        /// </summary>
        public Uri RequestUri
        {
            get { return _request.RequestUri; }
        }

        /// <summary>
        /// Returns the HTTP response.
        /// </summary>
        /// <returns>The HTTP response.</returns>
        public virtual IWebResponseData GetResponse()
        {
            try
            {
                var response = _request.GetResponse() as HttpWebResponse;
                return new HttpWebRequestResponseData(response);
            }
            catch (WebException webException)
            {
                var errorResponse = webException.Response as HttpWebResponse;
                if (errorResponse != null)
                {
                    throw new HttpErrorResponseException(webException.Message,
                        webException,
                        new HttpWebRequestResponseData(errorResponse));
                }
                throw;
            }
        }

        /// <summary>
        /// Gets a handle to the request content.
        /// </summary>
        /// <returns>The request content.</returns>
        public Stream GetRequestContent()
        {
            return _request.GetRequestStream();
        }

        /// <summary>
        /// Writes a stream to the request body.
        /// </summary>
        /// <param name="requestContent">The destination where the content stream is written.</param>
        /// <param name="contentStream">The content stream to be written.</param>
        /// <param name="contentHeaders">HTTP content headers.</param>
        /// <param name="requestContext">The request context.</param>
        public void WriteToRequestBody(Stream requestContent, Stream contentStream,
            IDictionary<string, string> contentHeaders, IRequestContext requestContext)
        {
            bool gotException = false;
            try
            {
                var buffer = new byte[requestContext.ClientConfig.BufferSize];
                int bytesRead = 0;
                int bytesToRead = buffer.Length;

                while ((bytesRead = contentStream.Read(buffer, 0, bytesToRead)) > 0)
                {
                    requestContent.Write(buffer, 0, bytesRead);
                }
            }
            catch
            {
                gotException = true;

                // If an exception occurred while reading the input stream,
                // Abort the request to signal failure to the server and prevent
                // potentially writing an incomplete stream to the server.
                this.Abort();
                throw;
            }
            finally
            {
                // Only bubble up exception from the close method if we haven't already got an exception
                // reading and writing from the streams.
                try
                {
                    requestContent.Close();
                }
                catch
                {
                    if (!gotException)
                        throw;
                }
            }
        }

        /// <summary>
        /// Writes a byte array to the request body.
        /// </summary>
        /// <param name="requestContent">The destination where the content stream is written.</param>
        /// <param name="content">The content stream to be written.</param>
        /// <param name="contentHeaders">HTTP content headers.</param>
        public void WriteToRequestBody(Stream requestContent, byte[] content, IDictionary<string, string> contentHeaders)
        {
            using (requestContent)
            {
                requestContent.Write(content, 0, content.Length);
            }
        }

        /// <summary>
        /// Aborts the HTTP request.
        /// </summary>
        public void Abort()
        {
            if (!_isAborted)
            {
                _request.Abort();
                _isAborted = true;
            }
        }

#if AWS_ASYNC_API

        /// <summary>
        /// Writes a stream to the request body.
        /// </summary>
        /// <param name="requestContent">The destination where the content stream is written.</param>
        /// <param name="contentStream">The content stream to be written.</param>
        /// <param name="contentHeaders">HTTP content headers.</param>
        /// <param name="requestContext">The request context.</param>
        public async Task WriteToRequestBodyAsync(Stream requestContent, Stream contentStream,
            IDictionary<string, string> contentHeaders, IRequestContext requestContext)
        {
            bool gotException = false;
            try
            {
                var buffer = new byte[requestContext.ClientConfig.BufferSize];
                int bytesRead = 0;
                int bytesToRead = buffer.Length;

                while ((bytesRead = await contentStream.ReadAsync(buffer, 0, bytesToRead, requestContext.CancellationToken).ConfigureAwait(false)) > 0)
                {
                    requestContext.CancellationToken.ThrowIfCancellationRequested();
                    await requestContent.WriteAsync(buffer, 0, bytesRead, requestContext.CancellationToken).ConfigureAwait(false);
                }
            }
            catch
            {
                gotException = true;

                // If an exception occurred while reading the input stream,
                // Abort the request to signal failure to the server and prevent
                // potentially writing an incomplete stream to the server.
                this.Abort();
                throw;
            }
            finally
            {
                // Only bubble up exception from the close method if we haven't already got an exception
                // reading and writing from the streams.
                try
                {
                    requestContent.Close();
                }
                catch
                {
                    if (!gotException)
                        throw;
                }
            }
        }

        /// <summary>
        /// Writes a byte array to the request body.
        /// </summary>
        /// <param name="requestContent">The destination where the content stream is written.</param>
        /// <param name="content">The content stream to be written.</param>
        /// <param name="contentHeaders">HTTP content headers.</param>
        public async Task WriteToRequestBodyAsync(Stream requestContent, byte[] content, IDictionary<string, string> contentHeaders, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (requestContent)
            {
                await requestContent.WriteAsync(content, 0, content.Length, cancellationToken).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Gets a handle to the request content.
        /// </summary>
        /// <returns></returns>
        public async Task<Stream> GetRequestContentAsync()
        {
            return await GetRequestContentAsync(CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a handle to the request content.
        /// </summary>
        /// <param name="cancellationToken">Used to cancel the request on demand</param>
        /// <returns></returns>
        public async Task<Stream> GetRequestContentAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (cancellationToken.Register(() => this.Abort(), useSynchronizationContext:false))
            {
                try
                {
                    return await _request.GetRequestStreamAsync().ConfigureAwait(false);
                }
                catch (WebException webException)
                {
                    // After HttpWebRequest.Abort() is called, GetRequestStreamAsync throws a WebException.
                    // If request has been cancelled using cancellationToken, wrap the
                    // WebException in an OperationCancelledException.
                    if (cancellationToken.IsCancellationRequested)
                    {
                        throw new OperationCanceledException(webException.Message, webException, cancellationToken);
                    }
                    var errorResponse = webException.Response as HttpWebResponse;
                    if (errorResponse != null)
                    {
                        throw new HttpErrorResponseException(webException.Message,
                            webException,
                            new HttpWebRequestResponseData(errorResponse));
                    }

                    throw;

                }
            }
            
        }

        /// <summary>
        /// Returns the HTTP response.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
        /// <returns></returns>
        public virtual async Task<IWebResponseData> GetResponseAsync(System.Threading.CancellationToken cancellationToken)
        {
            using (var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
            using (linkedTokenSource.Token.Register(() => this.Abort(), useSynchronizationContext: false))
            {
                linkedTokenSource.CancelAfter(_request.Timeout);
                
                try
                {
                    var response = await _request.GetResponseAsync().ConfigureAwait(false) as HttpWebResponse;
                    return new HttpWebRequestResponseData(response);
                }
                catch (WebException webException)
                {
                    // After HttpWebRequest.Abort() is called, GetResponseAsync throws a WebException.
                    // If request has been cancelled using cancellationToken, wrap the
                    // WebException in an OperationCancelledException.
                    if (cancellationToken.IsCancellationRequested)
                    {
                        throw new OperationCanceledException(webException.Message, webException, cancellationToken);
                    }
                    
                    if (linkedTokenSource.Token.IsCancellationRequested)
                    {
                        throw new OperationCanceledException(webException.Message, webException, linkedTokenSource.Token);
                    }                    

                    var errorResponse = webException.Response as HttpWebResponse;
                    if (errorResponse != null)
                    {
                        throw new HttpErrorResponseException(webException.Message,
                            webException,
                            new HttpWebRequestResponseData(errorResponse));
                    }

                    throw;
                }
            }
        }

#elif AWS_APM_API

        /// <summary>
        /// Initiates the operation to gets a handle to the request content.
        /// </summary>
        /// <param name="callback">The async callback invoked when the operation completes.</param>
        /// <param name="state">The state object to be passed to the async callback.</param>
        /// <returns>IAsyncResult that represents an async operation.</returns>
        public IAsyncResult BeginGetRequestContent(AsyncCallback callback, object state)
        {
            return _request.BeginGetRequestStream(callback, state);
        }

        /// <summary>
        /// Ends the operation to gets a handle to the request content.
        /// </summary>
        /// <param name="asyncResult">IAsyncResult that represents an async operation.</param>
        /// <returns>The request content.</returns>
        public Stream EndGetRequestContent(IAsyncResult asyncResult)
        {
            return _request.EndGetRequestStream(asyncResult);
        }

        /// <summary>
        /// Initiates the operation to Returns the HTTP response.
        /// </summary>
        /// <param name="callback">The async callback invoked when the operation completes.</param>
        /// <param name="state">The state object to be passed to the async callback.</param>
        /// <returns>IAsyncResult that represents an async operation.</returns>
        public IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
        {
            return _request.BeginGetResponse(callback, state);
        }

        /// <summary>
        /// Ends the operation to Returns the HTTP response.
        /// </summary>
        /// <param name="asyncResult">IAsyncResult that represents an async operation.</param>
        /// <returns>The HTTP response.</returns>
        public virtual IWebResponseData EndGetResponse(IAsyncResult asyncResult)
        {
            try
            {
                var response = _request.EndGetResponse(asyncResult) as HttpWebResponse;
                return new HttpWebRequestResponseData(response);
            }
            catch (WebException webException)
            {
                var errorResponse = webException.Response as HttpWebResponse;
                if (errorResponse != null)
                {
                    throw new HttpErrorResponseException(webException.Message,
                        webException,
                        new HttpWebRequestResponseData(errorResponse));
                }
                throw;
            }
        }
#endif

        /// <summary>
        /// Configures a request as per the request context.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        public virtual void ConfigureRequest(IRequestContext requestContext)
        {
            // Set HttpWebRequest specific properties which are
            // not exposed in the IHttpRequest interface.

            var clientConfig = requestContext.ClientConfig;
            var originalRequest = requestContext.OriginalRequest;

            // If System.Net.WebRequest.AllowAutoRedirect is set to true (default value),
            // redirects for GET requests are automatically followed and redirects for POST
            // requests are thrown back as exceptions.

            // If System.Net.WebRequest.AllowAutoRedirect is set to false (e.g. S3),
            // redirects are returned as responses.
            _request.AllowAutoRedirect = clientConfig.AllowAutoRedirect;

            // Configure timeouts.
            if (requestContext.Request.ContentStream != null)
            {
                _request.Timeout = int.MaxValue;
                _request.ReadWriteTimeout = int.MaxValue;
                _request.AllowWriteStreamBuffering = false;
            }

            // Override the Timeout and ReadWriteTimeout values if set at the request or config level.
            // Public Timeout and ReadWriteTimeout properties are present on client config objects.
            var timeout = ClientConfig.GetTimeoutValue(clientConfig.Timeout,
                originalRequest.TimeoutInternal);
            var readWriteTimeout = ClientConfig.GetTimeoutValue(clientConfig.ReadWriteTimeout,
                originalRequest.ReadWriteTimeoutInternal);
            if (timeout != null)
            {
                _request.Timeout = (int)timeout.Value.TotalMilliseconds;
            }
            if (readWriteTimeout != null)
            {
                _request.ReadWriteTimeout = (int)readWriteTimeout.Value.TotalMilliseconds;
            }

            // Set proxy related properties
            IWebProxy proxy = requestContext.ClientConfig.GetWebProxy();
            if (proxy != null)
            {
                requestContext.Metrics.AddProperty(Metric.ProxyHost, requestContext.ClientConfig.ProxyHost);
                requestContext.Metrics.AddProperty(Metric.ProxyPort, requestContext.ClientConfig.ProxyPort);
                _request.Proxy = proxy;
            }

            // Set service point properties.
            _request.ServicePoint.ConnectionLimit = clientConfig.ConnectionLimit;
            _request.ServicePoint.UseNagleAlgorithm = clientConfig.UseNagleAlgorithm;
            _request.ServicePoint.MaxIdleTime = clientConfig.MaxIdleTime;
            _request.ServicePoint.Expect100Continue = originalRequest.GetExpect100Continue();

            var tcpKeepAlive = clientConfig.TcpKeepAlive;
            _request.ServicePoint.SetTcpKeepAlive(tcpKeepAlive.Enabled, (int)tcpKeepAlive.Timeout.Value.TotalMilliseconds, 
                    (int)tcpKeepAlive.Interval.Value.TotalMilliseconds);
        }

        /// <summary>
        /// Sets the headers on the request.
        /// </summary>
        /// <param name="headers">A dictionary of header names and values.</param>
        public void SetRequestHeaders(IDictionary<string, string> headers)
        {
            AddHeaders(_request, headers);
        }

        private static System.Reflection.MethodInfo _addWithoutValidateHeadersMethod =
            typeof(WebHeaderCollection).GetMethod("AddWithoutValidate", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        // As per MSDN documentation (http://msdn.microsoft.com/en-us/library/system.net.webheadercollection%28v=VS.80%29.aspx)
        // some headers are restricted, cannot be set through the request.Headers property and must be
        // set through properties on the HttpWebRequest
        internal static void AddHeaders(HttpWebRequest request, IDictionary<string, string> headersToAdd)
        {
            var headers = request.Headers;
            foreach (var kvp in headersToAdd)
            {
                if (WebHeaderCollection.IsRestricted(kvp.Key) || string.Equals(kvp.Key, HeaderKeys.RangeHeader, StringComparison.OrdinalIgnoreCase))
                {
                    if (string.Equals(kvp.Key, HeaderKeys.AcceptHeader, StringComparison.OrdinalIgnoreCase))
                        request.Accept = kvp.Value;
                    else if (string.Equals(kvp.Key, HeaderKeys.ContentTypeHeader, StringComparison.OrdinalIgnoreCase))
                        request.ContentType = kvp.Value;
                    else if (string.Equals(kvp.Key, HeaderKeys.ContentLengthHeader, StringComparison.OrdinalIgnoreCase))
                        request.ContentLength = long.Parse(kvp.Value, CultureInfo.InvariantCulture);
                    else if (string.Equals(kvp.Key, HeaderKeys.UserAgentHeader, StringComparison.OrdinalIgnoreCase))
                        request.UserAgent = kvp.Value;
                    else if (string.Equals(kvp.Key, HeaderKeys.TransferEncodingHeader, StringComparison.OrdinalIgnoreCase) 
                        && string.Equals(kvp.Value, "chunked", StringComparison.OrdinalIgnoreCase))
                        request.SendChunked = true;
                    // Date accessor is only present in .NET 4.0, so using reflection
                    else if (string.Equals(kvp.Key, HeaderKeys.DateHeader, StringComparison.OrdinalIgnoreCase))
                        _addWithoutValidateHeadersMethod.Invoke(request.Headers, new[] { HeaderKeys.DateHeader, kvp.Value });
                    // Host accessor is only present in .NET 4.0, so using reflection
                    else if (string.Equals(kvp.Key, HeaderKeys.HostHeader, StringComparison.OrdinalIgnoreCase))
                        _addWithoutValidateHeadersMethod.Invoke(request.Headers, new[] { HeaderKeys.HostHeader, kvp.Value });
                    else if (string.Equals(kvp.Key, HeaderKeys.RangeHeader, StringComparison.OrdinalIgnoreCase))
                        _addWithoutValidateHeadersMethod.Invoke(request.Headers, new[] { HeaderKeys.RangeHeader, kvp.Value });
                    else if (string.Equals(kvp.Key, HeaderKeys.IfModifiedSinceHeader, StringComparison.OrdinalIgnoreCase))
                        _addWithoutValidateHeadersMethod.Invoke(request.Headers, new[] { kvp.Key, kvp.Value });
                    else
                        throw new NotSupportedException("Header with name " + kvp.Key + " is not suppored");
                }
                else
                {
                    headers[kvp.Key] = kvp.Value;
                }
            }
        }

        /// <summary>
        /// Disposes the HttpRequest.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // NOP since HttWebRequest does not implement IDisposable.
        }

        /// <summary>
        /// Sets up the progress listeners
        /// </summary>
        /// <param name="originalStream">The content stream</param>
        /// <param name="progressUpdateInterval">The interval at which progress needs to be published</param>
        /// <param name="sender">The objects which is trigerring the progress changes</param>
        /// <param name="callback">The callback which will be invoked when the progress changed event is trigerred</param>
        /// <returns>an <see cref="EventStream"/> object, incase the progress is setup, else returns the original stream</returns>
        public Stream SetupProgressListeners(Stream originalStream, long progressUpdateInterval, object sender, EventHandler<StreamTransferProgressArgs> callback)
        {
            var eventStream = new EventStream(originalStream, true);
            var tracker = new StreamReadTracker(sender, callback, originalStream.Length,
                progressUpdateInterval);
            eventStream.OnRead += tracker.ReadProgress;
            return eventStream;
        }
    }
}
