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

using Amazon.Runtime.Internal.Auth;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using System;
using System.IO;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// This handler retrieved the AWS credentials to be used for the current call.
    /// </summary>
    public class CredentialsRetriever : PipelineHandler
    {
        /// <summary>
        /// The constructor for CredentialsRetriever.
        /// </summary>
        /// <param name="credentials">An AWSCredentials instance.</param>
        public CredentialsRetriever(AWSCredentials credentials)
        {
            this.Credentials = credentials;
        }

        protected AWSCredentials Credentials
        {
            get;
            private set;
        }

        /// <summary>
        /// Retrieves the credentials to be used for the current call before 
        /// invoking the next handler.
        /// </summary>
        /// <param name="executionContext"></param>
        protected virtual void PreInvoke(IExecutionContext executionContext)
        {
            ImmutableCredentials ic = null;
            if (Credentials != null && !(Credentials is AnonymousAWSCredentials))
            {
                using(executionContext.RequestContext.Metrics.StartEvent(Metric.CredentialsRequestTime))
                {
                    ic = Credentials.GetCredentials();
                }
            }

            executionContext.RequestContext.ImmutableCredentials = ic;
        }

        /// <summary>
        /// Calls pre invoke logic before calling the next handler 
        /// in the pipeline.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        public override void InvokeSync(IExecutionContext executionContext)
        {
            PreInvoke(executionContext);
            base.InvokeSync(executionContext);
        }

#if AWS_ASYNC_API 

        /// <summary>
        /// Calls pre invoke logic before calling the next handler 
        /// in the pipeline.
        /// </summary>
        /// <typeparam name="T">The response type for the current request.</typeparam>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async System.Threading.Tasks.Task<T> InvokeAsync<T>(IExecutionContext executionContext)
        {
            ImmutableCredentials ic = null;
            if (Credentials != null && !(Credentials is AnonymousAWSCredentials))
            {
                using(executionContext.RequestContext.Metrics.StartEvent(Metric.CredentialsRequestTime))
                {
                    ic = await Credentials.GetCredentialsAsync().ConfigureAwait(false);
                }
            }

            executionContext.RequestContext.ImmutableCredentials = ic;

            return await base.InvokeAsync<T>(executionContext).ConfigureAwait(false);
        }

#elif AWS_APM_API

        /// <summary>
        /// Calls pre invoke logic before calling the next handler 
        /// in the pipeline.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <returns>IAsyncResult which represent an async operation.</returns>
        public override IAsyncResult InvokeAsync(IAsyncExecutionContext executionContext)
        {
            PreInvoke(ExecutionContext.CreateFromAsyncContext(executionContext));
            return base.InvokeAsync(executionContext);
        }
#endif

    }
}
