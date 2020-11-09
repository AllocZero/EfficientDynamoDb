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

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// A pipeline handler which provides hooks to run
    /// external code in the pre-invoke and post-invoke phases.
    /// </summary>
    public class CallbackHandler : PipelineHandler
    {
        /// <summary>
        /// Action to execute on the pre invoke phase.
        /// </summary>
        public Action<IExecutionContext> OnPreInvoke { get; set; }

        /// <summary>
        /// Action to execute on the post invoke phase.
        /// </summary>
        public Action<IExecutionContext> OnPostInvoke { get; set; }

        /// <summary>
        /// Calls the PreInvoke and PostInvoke methods before and after calling the next handler 
        /// in the pipeline.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        public override void InvokeSync(IExecutionContext executionContext)
        {
            PreInvoke(executionContext);
            base.InvokeSync(executionContext);
            PostInvoke(executionContext);
        }
#if AWS_ASYNC_API 

        /// <summary>
        /// Calls the PreInvoke and PostInvoke methods before and after calling the next handler 
        /// in the pipeline.
        /// </summary>
        /// <typeparam name="T">The response type for the current request.</typeparam>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async System.Threading.Tasks.Task<T> InvokeAsync<T>(IExecutionContext executionContext)
        {
            PreInvoke(executionContext);
            var response = await base.InvokeAsync<T>(executionContext).ConfigureAwait(false);
            PostInvoke(executionContext);
            return response;
        }

#elif AWS_APM_API

        /// <summary>
        /// Calls the PreInvoke method before calling the next handler in the pipeline.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <returns>IAsyncResult which represent an async operation.</returns>
        public override IAsyncResult InvokeAsync(IAsyncExecutionContext executionContext)
        {
            PreInvoke(ExecutionContext.CreateFromAsyncContext(executionContext));
            return base.InvokeAsync(executionContext);
        }

        /// <summary>
        /// Calls the PostInvoke methods after calling the next handler 
        /// in the pipeline.
        /// </summary>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        protected override void InvokeAsyncCallback(IAsyncExecutionContext executionContext)
        {
            // Process the response if an exception hasn't occured
            if (executionContext.ResponseContext.AsyncResult.Exception == null)
            {
                PostInvoke(ExecutionContext.CreateFromAsyncContext(executionContext));
            }
            base.InvokeAsyncCallback(executionContext);
        }
#endif

        /// <summary>
        /// Executes the OnPreInvoke action as part of pre-invoke.
        /// </summary>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        protected void PreInvoke(IExecutionContext executionContext)
        {
            RaiseOnPreInvoke(executionContext);
        }

        /// <summary>
        /// Executes the OnPreInvoke action as part of post-invoke.
        /// </summary>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        protected void PostInvoke(IExecutionContext executionContext)
        {
            RaiseOnPostInvoke(executionContext);
        }

        private void RaiseOnPreInvoke(IExecutionContext context)
        {
            if (this.OnPreInvoke != null)
                this.OnPreInvoke(context);
        }

        private void RaiseOnPostInvoke(IExecutionContext context)
        {
            if (this.OnPostInvoke != null)
                this.OnPostInvoke(context);
        }
    }
}
