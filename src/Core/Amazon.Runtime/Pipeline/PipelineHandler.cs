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

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// An abstract pipeline handler that has implements IPipelineHandler,
    /// and has the default implmentation. This is the base class for most of
    /// the handler implementations.
    /// </summary>    
    public abstract partial class PipelineHandler : IPipelineHandler
    {

        /// <summary>
        /// The logger used to log messages.
        /// </summary>
        public virtual ILogger Logger { get; set; }

        /// <summary>
        /// The inner handler which is called after the current 
        /// handler completes it's processing.
        /// </summary>
        public IPipelineHandler InnerHandler { get; set; }

        /// <summary>
        /// The outer handler which encapsulates the current handler.
        /// </summary>
        public IPipelineHandler OuterHandler { get; set; }

        /// <summary>
        /// Contains the processing logic for a synchronous request invocation.
        /// This method calls InnerHandler.InvokeSync to continue processing of the
        /// request by the pipeline.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
//        [System.Diagnostics.DebuggerHidden]
        public virtual void InvokeSync(IExecutionContext executionContext)
        {
            if (this.InnerHandler != null)
            {
                InnerHandler.InvokeSync(executionContext);
                return;
            }
            throw new InvalidOperationException("Cannot invoke InnerHandler. InnerHandler is not set.");
        }

#if AWS_APM_API

        /// <summary>
        /// Contains the processing logic for an asynchronous request invocation.
        /// This method should calls InnerHandler.InvokeSync to continue processing of the
        /// request by the pipeline.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <returns>IAsyncResult which represent an async operation.</returns>
        [System.Diagnostics.DebuggerHidden]
        public virtual IAsyncResult InvokeAsync(IAsyncExecutionContext executionContext)
        {
            if (this.InnerHandler != null)
            {
                return InnerHandler.InvokeAsync(executionContext);
            }
            throw new InvalidOperationException("Cannot invoke InnerHandler. InnerHandler is not set.");
        }

        /// <summary>
        /// This callback method is called by the callback method of the inner handler
        /// as part of asynchronous processing after any underlying asynchronous
        /// operations complete. 
        /// </summary>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        [System.Diagnostics.DebuggerHidden]
        public void AsyncCallback(IAsyncExecutionContext executionContext)
        {
            try
            {
                this.InvokeAsyncCallback(executionContext);
            }
            catch (Exception exception)
            {
                // Log exception
                this.Logger.Error(exception, "An exception of type {0} was thrown from InvokeAsyncCallback().",
                    exception.GetType().Name);
                executionContext.RequestContext.Metrics.AddProperty(Metric.Exception, exception);

                // An unhandled exception occured in the callback implementation.
                // Capture the exception and end the callback processing by signalling the
                // wait handle.
                executionContext.RequestContext.Metrics.StopEvent(Metric.ClientExecuteTime);
                LogMetrics(ExecutionContext.CreateFromAsyncContext(executionContext));
                
                // Sets the exception on the AsyncResult, signals the wait handle and calls the user callback.
                executionContext.ResponseContext.AsyncResult.HandleException(exception);
            }
        }

        /// <summary>
        /// This callback method contains the processing logic that should be executed 
        /// after the underlying asynchronous operation completes.
        /// This method is called as part of a callback chain which starts 
        /// from the InvokeAsyncCallback method of the bottommost handler and then invokes
        /// each callback method of the handler above it.
        /// This method calls OuterHandler.AsyncCallback to continue processing of the
        /// request by the pipeline, unless it's the topmost handler.
        /// </summary>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        [System.Diagnostics.DebuggerHidden]
        protected virtual void InvokeAsyncCallback(IAsyncExecutionContext executionContext)
        {
            if (this.OuterHandler!=null)
            {
                this.OuterHandler.AsyncCallback(executionContext);    
            }
            else
            {
                // No more outer handlers to process, signal completion
                executionContext.ResponseContext.AsyncResult.Response =

                    executionContext.ResponseContext.Response;

                // Signals the wait handle and calls the user callback.
                executionContext.ResponseContext.AsyncResult.InvokeCallback();
            }
        }
#endif

#if AWS_ASYNC_API

        /// <summary>
        /// Contains the processing logic for an asynchronous request invocation.
        /// This method calls InnerHandler.InvokeSync to continue processing of the
        /// request by the pipeline.
        /// </summary>
        /// <typeparam name="T">The response type for the current request.</typeparam>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
#if !NETSTANDARD
        [System.Diagnostics.DebuggerHidden]
#endif
        public virtual System.Threading.Tasks.Task<T> InvokeAsync<T>(IExecutionContext executionContext)
            where T : AmazonWebServiceResponse, new()
        {
            if (this.InnerHandler != null)
            {
                return InnerHandler.InvokeAsync<T>(executionContext);                
            }
            throw new InvalidOperationException("Cannot invoke InnerHandler. InnerHandler is not set.");
        }
#endif

        /// <summary>
        /// Logs the metrics for the current execution context.
        /// </summary>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        protected void LogMetrics(IExecutionContext executionContext)
        {
            var metrics = executionContext.RequestContext.Metrics;
            if (executionContext.RequestContext.ClientConfig.LogMetrics)
            {
                string errors = metrics.GetErrors();
                if (!string.IsNullOrEmpty(errors))
                    this.Logger.InfoFormat("Request metrics errors: {0}", errors);
                this.Logger.InfoFormat("Request metrics: {0}", metrics);
            }
        }
    }
}
