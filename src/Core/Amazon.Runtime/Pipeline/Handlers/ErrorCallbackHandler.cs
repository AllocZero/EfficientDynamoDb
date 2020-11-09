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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// This handler provides an OnError action that can be used as hook for
    /// external code to handle exceptions in the runtime pipeline.
    /// </summary>
    public class ErrorCallbackHandler : PipelineHandler
    {
        /// <summary>
        /// Action to execute if an exception occurs during the 
        /// execution of any underlying handlers.
        /// </summary>
        public Action<IExecutionContext,Exception> OnError { get; set; }

        public override void InvokeSync(IExecutionContext executionContext)
        {
            try
            {
                base.InvokeSync(executionContext);
            }
            catch (Exception exception)
            {
                HandleException(executionContext, exception);
                throw;
            }
        }

#if AWS_ASYNC_API 
        public override async System.Threading.Tasks.Task<T> InvokeAsync<T>(IExecutionContext executionContext)
        {
            try
            {   
                return await base.InvokeAsync<T>(executionContext).ConfigureAwait(false);
            }
            catch(Exception exception)
            {
                HandleException(executionContext, exception);
                throw;
            }
        }
#elif AWS_APM_API
        protected override void InvokeAsyncCallback(IAsyncExecutionContext executionContext)
        {
            var exception = executionContext.ResponseContext.AsyncResult.Exception;
            if (executionContext.ResponseContext.AsyncResult.Exception != null)
            {
                HandleException(ExecutionContext.CreateFromAsyncContext(executionContext), exception);
            }
            // Call outer handler
            base.InvokeAsyncCallback(executionContext);
        }
#endif

        /// <summary>
        /// Executes the OnError action if an exception occurs during the 
        /// execution of any underlying handlers.
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="exception"></param>
        protected void HandleException(IExecutionContext executionContext, Exception exception)
        {
            OnError(executionContext, exception);
        }
    }
}
