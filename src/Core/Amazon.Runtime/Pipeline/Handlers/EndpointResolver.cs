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
    /// This handler resolves the endpoint to be used for the current request.
    /// </summary>
    public class EndpointResolver : PipelineHandler
    {
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
        public override System.Threading.Tasks.Task<T> InvokeAsync<T>(IExecutionContext executionContext)
        {
            PreInvoke(executionContext);
            return base.InvokeAsync<T>(executionContext);                        
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

        /// <summary>
        /// Resolves the endpoint to be used for the current request
        /// before invoking the next handler.
        /// </summary>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        protected void PreInvoke(IExecutionContext executionContext)
        {
            var requestContext = executionContext.RequestContext;
            if(requestContext.Request.Endpoint == null)
                requestContext.Request.Endpoint = DetermineEndpoint(executionContext.RequestContext);
        }

        /// <summary>
        /// Determines the endpoint for the request.
        /// </summary>
        /// <param name="requestContext">The request context.</param>
        /// <returns></returns>
        public virtual Uri DetermineEndpoint(IRequestContext requestContext)
        {
            return DetermineEndpoint(requestContext.ClientConfig, requestContext.Request);
        }

        public static Uri DetermineEndpoint(IClientConfig config, IRequest request)
        {
            Uri endpoint = request.AlternateEndpoint != null
                ? new Uri(ClientConfig.GetUrl(request.AlternateEndpoint, config.RegionEndpointServiceName, config.UseHttp, config.UseDualstackEndpoint))
                : new Uri(config.DetermineServiceURL());

            return InjectHostPrefix(config, request, endpoint);
        }

        private static Uri InjectHostPrefix(IClientConfig config, IRequest request, Uri endpoint)
        {
            if (config.DisableHostPrefixInjection || string.IsNullOrEmpty(request.HostPrefix))
            {
                return endpoint;
            }

            var hostPrefixUri = new UriBuilder(endpoint);
            hostPrefixUri.Host = request.HostPrefix + hostPrefixUri.Host;            
            return hostPrefixUri.Uri;
        }
    }
}
