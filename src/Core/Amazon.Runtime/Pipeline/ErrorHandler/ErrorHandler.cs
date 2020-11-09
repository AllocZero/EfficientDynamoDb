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
using Amazon.Util.Internal;
using System;
using System.Collections.Generic;
using System.Net;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// This handler processes exceptions thrown from the HTTP handler and
    /// unmarshalls error responses.
    /// </summary>
    public class ErrorHandler : PipelineHandler
    {
        /// <summary>
        /// Default set of exception handlers.
        /// </summary>
        private IDictionary<Type, IExceptionHandler> _exceptionHandlers;

        /// <summary>
        /// Default set of exception handlers.
        /// </summary>
        public IDictionary<Type, IExceptionHandler> ExceptionHandlers
        {
            get
            {
                return _exceptionHandlers;
            }
        }

        /// <summary>
        /// Constructor for ErrorHandler.
        /// </summary>
        /// <param name="logger">an ILogger instance.</param>
        public ErrorHandler(ILogger logger)
        {
            this.Logger = logger;

            _exceptionHandlers = new Dictionary<Type, IExceptionHandler>
            {
#if BCL
                {typeof(WebException), new WebExceptionHandler(this.Logger)},
#endif
                {typeof(HttpErrorResponseException), new HttpErrorResponseExceptionHandler(this.Logger)}
            };
        }

        /// <summary>
        /// Handles and processes any exception thrown from underlying handlers.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        public override void InvokeSync(IExecutionContext executionContext)
        {
            try
            {
                base.InvokeSync(executionContext);
                return;
            }
            catch (Exception exception)
            {
                DisposeReponse(executionContext.ResponseContext);
                bool rethrowOriginalException = ProcessException(executionContext, exception);
                if (rethrowOriginalException)
                {
                    throw;
                }
            }
        }

#if AWS_ASYNC_API

        /// <summary>
        /// Handles and processes any exception thrown from underlying handlers.
        /// </summary>
        /// <typeparam name="T">The response type for the current request.</typeparam>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async System.Threading.Tasks.Task<T> InvokeAsync<T>(IExecutionContext executionContext)
        {
            try
            {
                return await base.InvokeAsync<T>(executionContext).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                DisposeReponse(executionContext.ResponseContext);
                bool rethrowOriginalException = ProcessException(executionContext, exception);
                if (rethrowOriginalException)
                {
                    throw;
                }
            }

            // If response if set and an exception is not rethrown, return the response.
            // E.g. S3 GetLifecycleConfiguration, GetBucket policy and few other operations
            // return a 404 which is not returned back as an exception but as a empty response with 
            // error code.
            if(executionContext.ResponseContext != null && executionContext.ResponseContext.Response != null)
            {
                return executionContext.ResponseContext.Response as T;
            }

            return null;
        }        

#elif AWS_APM_API

        /// <summary>
        ///  Handles and processes any exception thrown from underlying handlers.
        /// </summary>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        protected override void InvokeAsyncCallback(IAsyncExecutionContext executionContext)
        {
            var requestContext = executionContext.RequestContext;
            var responseContext = executionContext.ResponseContext;
            var exception = responseContext.AsyncResult.Exception;
            if (exception != null)
            {
                try
                {
                    DisposeReponse(executionContext.ResponseContext);

                    bool rethrow = ProcessException(
                        ExecutionContext.CreateFromAsyncContext(executionContext),
                        exception);

                    // Suppress exception
                    if (!rethrow)
                        responseContext.AsyncResult.Exception = null;
                }
                catch (Exception processedException)
                {
                    // Catch any new exception thrown by ProcessException()
                    responseContext.AsyncResult.Exception = processedException;
                }
            }

            // Call outer handler
            base.InvokeAsyncCallback(executionContext);
        }
#endif

        /// <summary>
        /// Disposes the response body.
        /// </summary>
        /// <param name="responseContext">The response context.</param>
        private static void DisposeReponse(IResponseContext responseContext)
        {
            if (responseContext.HttpResponse != null &&
                responseContext.HttpResponse.ResponseBody != null)
            {
                responseContext.HttpResponse.ResponseBody.Dispose();
            }
        }

        /// <summary>
        /// Processes an exception by invoking a matching exception handler
        /// for the given exception.
        /// </summary>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        /// <param name="exception">The exception to be processed.</param>
        /// <returns>
        /// This method returns a boolean value which indicates if the original exception
        /// should be rethrown.
        /// This method can also throw a new exception that may be thrown by exception
        /// processing by a matching exception handler.
        /// </returns>
        private bool ProcessException(IExecutionContext executionContext, Exception exception)
        {
            // Log the exception
            this.Logger.Error(exception, "An exception of type {0} was handled in ErrorHandler.", exception.GetType().Name);
            executionContext.RequestContext.Metrics.AddProperty(Metric.Exception, exception);

            // Find the matching handler which can process the exception
            // Start by checking if there is a matching handler for the specific exception type,
            // if not check for handlers for it's base type till we find a match.
            var exceptionType = exception.GetType();
            var exceptionTypeInfo = TypeFactory.GetTypeInfo(exception.GetType());
            do
            {
                IExceptionHandler exceptionHandler = null;

                if (this.ExceptionHandlers.TryGetValue(exceptionType, out exceptionHandler))
                {
                    return exceptionHandler.Handle(executionContext, exception);
                }
                exceptionType = exceptionTypeInfo.BaseType;
                exceptionTypeInfo = TypeFactory.GetTypeInfo(exceptionTypeInfo.BaseType);

            } while (exceptionType != typeof(Exception));

            // No match found, rethrow the original exception.
            return true;
        }
    }
}
