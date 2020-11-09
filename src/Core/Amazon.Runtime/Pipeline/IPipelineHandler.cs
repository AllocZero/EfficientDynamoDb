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

namespace Amazon.Runtime
{
    /// <summary>
    /// Interface for a handler in a pipeline.
    /// </summary>
    public partial interface IPipelineHandler
    {
        /// <summary>
        /// The logger used to log messages.
        /// </summary>
        ILogger Logger { get; set; }

        /// <summary>
        /// The inner handler which is called after the current 
        /// handler completes it's processing.
        /// </summary>
        IPipelineHandler InnerHandler { get; set; }

        /// <summary>
        /// The outer handler which encapsulates the current handler.
        /// </summary>
        IPipelineHandler OuterHandler { get; set; }

        /// <summary>
        /// Contains the processing logic for a synchronous request invocation.
        /// This method should call InnerHandler.InvokeSync to continue processing of the
        /// request by the pipeline, unless it's a terminating handler.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        void InvokeSync(IExecutionContext executionContext);

#if AWS_APM_API

        /// <summary>
        /// Contains the processing logic for an asynchronous request invocation.
        /// This method should call InnerHandler.InvokeSync to continue processing of the
        /// request by the pipeline, unless it's a terminating handler.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <returns>IAsyncResult which represent a async operation.</returns>
        IAsyncResult InvokeAsync(IAsyncExecutionContext executionContext);

        /// <summary>
        /// This callback method contains the processing logic that should be executed 
        /// after the underlying asynchronous operation completes.
        /// This method is called as part of a callback chain which starts 
        /// from the InvokeAsyncCallback method of the bottommost handler and then invokes
        /// each callback method of the handler above it.
        /// This method should call OuterHandler.AsyncCallback to continue processing of the
        /// request by the pipeline, unless it's the topmost handler.
        /// </summary>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        void AsyncCallback(IAsyncExecutionContext executionContext);
#endif

#if AWS_ASYNC_API

        /// <summary>
        /// Contains the processing logic for an asynchronous request invocation.
        /// This method should call InnerHandler.InvokeSync to continue processing of the
        /// request by the pipeline, unless it's a terminating handler.
        /// </summary>
        /// <typeparam name="T">The response type for the current request.</typeparam>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        System.Threading.Tasks.Task<T> InvokeAsync<T>(IExecutionContext executionContext)
            where T : AmazonWebServiceResponse, new();
#endif
    }
}
