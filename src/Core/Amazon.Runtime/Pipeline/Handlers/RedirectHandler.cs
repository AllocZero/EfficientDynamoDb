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
using System.Net;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// This handler processes HTTP redirects and reissues the call to the
    /// redirected location.
    /// </summary>
    public class RedirectHandler : PipelineHandler
    {
        /// <summary>
        /// Processes HTTP redirects and reissues the call to the
        /// redirected location.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        public override void InvokeSync(IExecutionContext executionContext)
        {
            do
            {
                base.InvokeSync(executionContext);
            } while (HandleRedirect(executionContext));
        }

#if AWS_ASYNC_API 

        /// <summary>
        /// Processes HTTP redirects and reissues the call to the
        /// redirected location.
        /// </summary>
        /// <typeparam name="T">The response type for the current request.</typeparam>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async System.Threading.Tasks.Task<T> InvokeAsync<T>(IExecutionContext executionContext)
        {
            T result = null;
            do
            {
                result = await base.InvokeAsync<T>(executionContext).ConfigureAwait(false);
            } while (HandleRedirect(executionContext));
            return result;
        }

#elif AWS_APM_API

        /// <summary>
        /// Processes HTTP redirects and reissues the call to the
        /// redirected location.
        /// </summary>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        protected override void InvokeAsyncCallback(IAsyncExecutionContext executionContext)
        {
            // Check for redirect if an exception hasn't occured
            if (executionContext.ResponseContext.AsyncResult.Exception == null)
            {
                // Check if a redirect is required
                if (HandleRedirect(ExecutionContext.CreateFromAsyncContext(executionContext)))
                {
                    // Call InvokeAsync to redirect to new location.
                    base.InvokeAsync(executionContext);
                    return;
                }
            }

            // Not a redirect, call outer callbacks to continue processing response.
            base.InvokeAsyncCallback(executionContext);
        }

#endif
        /// <summary>
        /// Checks if an HTTP 307 (temporary redirect) has occured and changes the 
        /// request endpoint to the redirected location.
        /// </summary>
        /// <param name="executionContext">
        /// The execution context, it contains the request and response context.
        /// </param>
        /// <returns>
        /// A boolean value that indicates if a redirect has occured.
        /// </returns>
        private bool HandleRedirect(IExecutionContext executionContext)
        {
            var response = executionContext.ResponseContext.HttpResponse;

            // Handle all HTTP 3xx status codes
            if (response.StatusCode >= HttpStatusCode.Ambiguous &&
                response.StatusCode < HttpStatusCode.BadRequest)
            {
                if (response.StatusCode == HttpStatusCode.TemporaryRedirect &&
                    response.IsHeaderPresent(HeaderKeys.LocationHeader))
                {
                    var requestContext = executionContext.RequestContext;
                    string redirectedLocation = response.GetHeaderValue(HeaderKeys.LocationHeader);
                    requestContext.Metrics.AddProperty(Metric.RedirectLocation, redirectedLocation);

                    var isRewindableStream = executionContext.RequestContext.Request.IsRequestStreamRewindable();
                    if (isRewindableStream &&
                        !string.IsNullOrEmpty(redirectedLocation))
                    {
                        FinalizeForRedirect(executionContext, redirectedLocation);

                        // Dispose the current response body before we redirect.
                        if (response.ResponseBody != null)
                        {
                            response.ResponseBody.Dispose();
                        }

                        return true;
                    }
                }

                // Set HttpResponse on ResponseContext to null, 
                // as the HttpResponse will be wrapped in an exception.
                executionContext.ResponseContext.HttpResponse = null;

                // Throw an exception contain the HTTP response, so that an
                // appropriate exception can be returned to the user.
                throw new HttpErrorResponseException(response);
            }
            return false;
        }

        protected virtual void FinalizeForRedirect(IExecutionContext executionContext, string redirectedLocation)
        {
            this.Logger.InfoFormat("Request {0} is being redirected to {1}.",
                                    executionContext.RequestContext.RequestName, 
                                    redirectedLocation);

            var uri = new Uri(redirectedLocation);

            var requestContext = executionContext.RequestContext;
            requestContext.Request.Endpoint = new UriBuilder(uri.Scheme, uri.Host).Uri;

            RetryHandler.PrepareForRetry(executionContext.RequestContext);
        }
    }
}
