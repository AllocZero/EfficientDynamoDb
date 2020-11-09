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

using Amazon.Runtime.Internal.Util;
using System;
using System.Net;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// The retry handler has the generic logic for retrying requests.
    /// It uses a retry policy which specifies when 
    /// a retry should be performed.
    /// </summary>
    public class RetryHandler : PipelineHandler
    {
        private ILogger _logger;

        /// <summary>
        /// The logger used to log messages.
        /// </summary>
        public override ILogger Logger
        {
            get { return _logger; }
            set
            {
                _logger = value;
                this.RetryPolicy.Logger = value;
            }
        }

        /// <summary>
        /// The retry policy which specifies when 
        /// a retry should be performed.
        /// </summary>
        public RetryPolicy RetryPolicy { get; private set; }
        
        /// <summary>
        /// Constructor which takes in a retry policy.
        /// </summary>
        /// <param name="retryPolicy">Retry Policy</param>
        public RetryHandler(RetryPolicy retryPolicy)
        {
            this.RetryPolicy = retryPolicy;
        }

        /// <summary>
        /// Invokes the inner handler and performs a retry, if required as per the
        /// retry policy.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        public override void InvokeSync(IExecutionContext executionContext)
        {
            var requestContext = executionContext.RequestContext;
            var responseContext = executionContext.ResponseContext;
            bool shouldRetry = false;
            this.RetryPolicy.ObtainSendToken(executionContext, null);
            do
            {
                try
                {                    
                    base.InvokeSync(executionContext);                    
                    this.RetryPolicy.NotifySuccess(executionContext);                    
                    return;
                }
                catch (Exception exception)
                {
                    shouldRetry = this.RetryPolicy.Retry(executionContext, exception);
                    if (!shouldRetry)
                    {
                        LogForError(requestContext, exception);
                        throw;
                    }
                    else
                    {
                        requestContext.Retries++;
                        requestContext.Metrics.SetCounter(Metric.AttemptCount, requestContext.Retries);
                        LogForRetry(requestContext, exception);
                    }

                    this.RetryPolicy.ObtainSendToken(executionContext, exception);
                }

                PrepareForRetry(requestContext);

                using (requestContext.Metrics.StartEvent(Metric.RetryPauseTime))
                    this.RetryPolicy.WaitBeforeRetry(executionContext);

            } while (shouldRetry);
        }

#if AWS_ASYNC_API 

        /// <summary>
        /// Invokes the inner handler and performs a retry, if required as per the
        /// retry policy.
        /// </summary>
        /// <typeparam name="T">The response type for the current request.</typeparam>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async System.Threading.Tasks.Task<T> InvokeAsync<T>(IExecutionContext executionContext)
        {
            var requestContext = executionContext.RequestContext;
            var responseContext = executionContext.ResponseContext;
            bool shouldRetry = false;
            await this.RetryPolicy.ObtainSendTokenAsync(executionContext, null).ConfigureAwait(false);

            do
            {
                System.Runtime.ExceptionServices.ExceptionDispatchInfo capturedException = null;                

                try
                {                
                    T result = await base.InvokeAsync<T>(executionContext).ConfigureAwait(false);
                    this.RetryPolicy.NotifySuccess(executionContext);
                    return result;
                }
                catch (Exception e)
                {
                    capturedException = System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(e);
                }

                if (capturedException != null)
                {
                    shouldRetry = await this.RetryPolicy.RetryAsync(executionContext, capturedException.SourceException).ConfigureAwait(false);
                    if (!shouldRetry)
                    {
                        LogForError(requestContext, capturedException.SourceException);
                        capturedException.Throw();
                    }
                    else
                    {
                        requestContext.Retries++;
                        requestContext.Metrics.SetCounter(Metric.AttemptCount, requestContext.Retries);
                        LogForRetry(requestContext, capturedException.SourceException);
                    }

                    await this.RetryPolicy.ObtainSendTokenAsync(executionContext, capturedException.SourceException).ConfigureAwait(false);
                }

                PrepareForRetry(requestContext);

                using (requestContext.Metrics.StartEvent(Metric.RetryPauseTime))
                    await RetryPolicy.WaitBeforeRetryAsync(executionContext).ConfigureAwait(false);

            } while (shouldRetry);
            throw new AmazonClientException("Neither a response was returned nor an exception was thrown in the Runtime RetryHandler.");
        }

#endif

#if AWS_APM_API 

        /// <summary>
        /// Invokes the inner handler and performs a retry, if required as per the
        /// retry policy.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        protected override void InvokeAsyncCallback(IAsyncExecutionContext executionContext)
        {
            var requestContext = executionContext.RequestContext;
            var responseContext = executionContext.ResponseContext;
            var exception = responseContext.AsyncResult.Exception;
            var syncExecutionContext = ExecutionContext.CreateFromAsyncContext(executionContext);

            if (exception != null)
            {   
                var shouldRetry = this.RetryPolicy.Retry(syncExecutionContext, exception);
                if (shouldRetry)
                {
                    requestContext.Retries++;
                    requestContext.Metrics.SetCounter(Metric.AttemptCount, requestContext.Retries);
                    LogForRetry(requestContext, exception);

                    PrepareForRetry(requestContext);

                    // Clear out current exception
                    responseContext.AsyncResult.Exception = null;

                    using (requestContext.Metrics.StartEvent(Metric.RetryPauseTime))
                        this.RetryPolicy.WaitBeforeRetry(syncExecutionContext);

                    // Retry by calling InvokeAsync
                    this.InvokeAsync(executionContext);
                    return;
                }
                else
                {
                    LogForError(requestContext, exception);
                }
            }
            else
            {
                this.RetryPolicy.NotifySuccess(syncExecutionContext);
            }

            // Call outer handler
            base.InvokeAsyncCallback(executionContext);
        }
#endif

        /// <summary>
        /// Prepares the request for retry.
        /// </summary>
        /// <param name="requestContext">Request context containing the state of the request.</param>
        internal static void PrepareForRetry(IRequestContext requestContext)
        {
            if (requestContext.Request.ContentStream != null &&
                requestContext.Request.OriginalStreamPosition >= 0)
            {
                var stream = requestContext.Request.ContentStream;

                // If the stream is wrapped in a HashStream, reset the HashStream
                var hashStream = stream as HashStream;
                if (hashStream != null)
                {
                    hashStream.Reset();
                    stream = hashStream.GetSeekableBaseStream();
                }
                stream.Position = requestContext.Request.OriginalStreamPosition;
            }
        }
        
        private void LogForRetry(IRequestContext requestContext, Exception exception)
        {
#if !NETSTANDARD
            var webException = exception as WebException;
            if (webException != null)
            {
                Logger.InfoFormat("WebException ({1}) making request {2} to {3}. Attempting retry {4} of {5}.",
                          webException.Status,
                          requestContext.RequestName,
                          requestContext.Request.Endpoint.ToString(),
                          requestContext.Retries,
                          this.RetryPolicy.MaxRetries);
            }
            else
            {
#endif
            Logger.InfoFormat("{0} making request {1} to {2}. Attempting retry {3} of {4}.",
                          exception.GetType().Name,
                          requestContext.RequestName,
                          requestContext.Request.Endpoint.ToString(),
                          requestContext.Retries,
                          this.RetryPolicy.MaxRetries);
#if !NETSTANDARD
            }
#endif
        }

        private void LogForError(IRequestContext requestContext, Exception exception)
        {
            Logger.Error(exception, "{0} making request {1} to {2}. Attempt {3}.",
                          exception.GetType().Name,
                          requestContext.RequestName,
                          requestContext.Request.Endpoint.ToString(),
                          requestContext.Retries + 1);
        }        
    }
}
