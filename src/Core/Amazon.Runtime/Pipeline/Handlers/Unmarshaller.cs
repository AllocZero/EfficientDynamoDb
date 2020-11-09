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

using System;
using Amazon.Runtime.Internal.Transform;
using Amazon.Util;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// This handler unmarshalls the HTTP response.
    /// </summary>
    public class Unmarshaller : PipelineHandler
    {
        private bool _supportsResponseLogging;

        /// <summary>
        /// The constructor for Unmarshaller.
        /// </summary>
        /// <param name="supportsResponseLogging">
        /// Boolean value which indicated if the unmarshaller 
        /// handler supports response logging.
        /// </param>
        public Unmarshaller(bool supportsResponseLogging)
        {
            _supportsResponseLogging = supportsResponseLogging;
        }

        /// <summary>
        /// Unmarshalls the response returned by the HttpHandler.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        public override void InvokeSync(IExecutionContext executionContext)
        {
            base.InvokeSync(executionContext);

            if (executionContext.ResponseContext.HttpResponse.IsSuccessStatusCode)
            {
                // Unmarshall the http response.
                Unmarshall(executionContext);  
            }                      
        }

#if BCL45

        /// <summary>
        /// Unmarshalls the response returned by the HttpHandler.
        /// </summary>
        /// <typeparam name="T">The response type for the current request.</typeparam>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async System.Threading.Tasks.Task<T> InvokeAsync<T>(IExecutionContext executionContext)
        {
            await base.InvokeAsync<T>(executionContext).ConfigureAwait(false);
            // Unmarshall the response
            Unmarshall(executionContext);            
            return (T)executionContext.ResponseContext.Response;
        }

#elif NETSTANDARD

        /// <summary>
        /// Unmarshalls the response returned by the HttpHandler.
        /// </summary>
        /// <typeparam name="T">The response type for the current request.</typeparam>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async System.Threading.Tasks.Task<T> InvokeAsync<T>(IExecutionContext executionContext)
        {
            await base.InvokeAsync<T>(executionContext).ConfigureAwait(false);
            // Unmarshall the response
            await UnmarshallAsync(executionContext).ConfigureAwait(false);            
            return (T)executionContext.ResponseContext.Response;
        }

#elif AWS_APM_API

        /// <summary>
        /// Unmarshalls the response returned by the HttpHandler.
        /// </summary>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        protected override void InvokeAsyncCallback(IAsyncExecutionContext executionContext)
        {
            // Unmarshall the response if an exception hasn't occured
            if (executionContext.ResponseContext.AsyncResult.Exception == null)
            {
                Unmarshall(ExecutionContext.CreateFromAsyncContext(executionContext));
            }            
            base.InvokeAsyncCallback(executionContext);
        }
#endif

        /// <summary>
        /// Unmarshalls the HTTP response.
        /// </summary>
        /// <param name="executionContext">
        /// The execution context, it contains the request and response context.
        /// </param>
        private void Unmarshall(IExecutionContext executionContext)
        {
            var requestContext = executionContext.RequestContext;
            var responseContext = executionContext.ResponseContext;

            using (requestContext.Metrics.StartEvent(Metric.ResponseProcessingTime))
            {
                var unmarshaller = requestContext.Unmarshaller;
                try
                {
                    var readEntireResponse = _supportsResponseLogging;

                    var context = unmarshaller.CreateContext(responseContext.HttpResponse,
                        readEntireResponse,
                        responseContext.HttpResponse.ResponseBody.OpenResponse(),
                        requestContext.Metrics,
                        false);

                    try
                    {
                        var response = UnmarshallResponse(context, requestContext);
                        responseContext.Response = response;
                    }
                    catch(Exception e)
                    {
                        // Rethrow Amazon service or client exceptions
                        if (e is AmazonServiceException ||
                            e is AmazonClientException)
                        {
                            throw;
                        }

                        // Else, there was an issue with the response body, throw AmazonUnmarshallingException
                        var requestId = responseContext.HttpResponse.GetHeaderValue(HeaderKeys.RequestIdHeader);
                        var body = context.ResponseBody;
                        throw new AmazonUnmarshallingException(requestId, lastKnownLocation: null, responseBody: body, innerException: e,
                            statusCode: responseContext.HttpResponse.StatusCode);
                    }
                }
                finally
                {
                    if (!unmarshaller.HasStreamingProperty)
                        responseContext.HttpResponse.ResponseBody.Dispose();                 
                }
            }
        }

#if NETSTANDARD

        /// <summary>
        /// Unmarshalls the HTTP response.
        /// </summary>
        /// <param name="executionContext">
        /// The execution context, it contains the request and response context.
        /// </param>
        private async System.Threading.Tasks.Task UnmarshallAsync(IExecutionContext executionContext)
        {
            var requestContext = executionContext.RequestContext;
            var responseContext = executionContext.ResponseContext;

            using (requestContext.Metrics.StartEvent(Metric.ResponseProcessingTime))
            {
                var unmarshaller = requestContext.Unmarshaller;
                try
                {
                    var readEntireResponse = _supportsResponseLogging &&
                        (requestContext.ClientConfig.LogResponse || requestContext.ClientConfig.ReadEntireResponse
                        || AWSConfigs.LoggingConfig.LogResponses != ResponseLoggingOption.Never);

                    var responseStream = await responseContext.HttpResponse.
                        ResponseBody.OpenResponseAsync().ConfigureAwait(false);
                    var context = unmarshaller.CreateContext(responseContext.HttpResponse,
                        readEntireResponse,
                        responseStream,
                        requestContext.Metrics,
                        false);

                    var response = UnmarshallResponse(context, requestContext);
                    responseContext.Response = response;
                }
                finally
                {
                    if (!unmarshaller.HasStreamingProperty)
                        responseContext.HttpResponse.ResponseBody.Dispose();
                }
            }
        }
#endif

        private AmazonWebServiceResponse UnmarshallResponse(UnmarshallerContext context,
            IRequestContext requestContext)
        {
            try
            {
                var unmarshaller = requestContext.Unmarshaller;
                AmazonWebServiceResponse response = null;
                using (requestContext.Metrics.StartEvent(Metric.ResponseUnmarshallTime))
                {
                    response = unmarshaller.UnmarshallResponse(context);
                }

                requestContext.Metrics.AddProperty(Metric.StatusCode, response.HttpStatusCode);
                requestContext.Metrics.AddProperty(Metric.BytesProcessed, response.ContentLength);
                if (response.ResponseMetadata != null)
                {
                    requestContext.Metrics.AddProperty(Metric.AWSRequestID, response.ResponseMetadata.RequestId);
                }

                context.ValidateCRC32IfAvailable();
                return response;
            }
            finally
            {
                var logResponseBody = ShouldLogResponseBody(_supportsResponseLogging, requestContext);

                if (logResponseBody)
                {
                    this.Logger.DebugFormat("Received response (truncated to {0} bytes): [{1}]",
                        AWSConfigs.LoggingConfig.LogResponsesSizeLimit,
                        context.ResponseBody);
                }
            }
        }

        private static bool ShouldLogResponseBody(bool supportsResponseLogging, IRequestContext requestContext)
        {
            return supportsResponseLogging &&
                (requestContext.ClientConfig.LogResponse || AWSConfigs.LoggingConfig.LogResponses == ResponseLoggingOption.Always);
        }
    }
}
