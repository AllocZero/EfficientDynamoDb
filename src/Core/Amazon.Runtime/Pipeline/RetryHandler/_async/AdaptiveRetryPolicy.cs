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

using Amazon.Util;
using System;
using System.Threading.Tasks;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// The default implementation of the adaptive policy.
    /// </summary>
    public partial class AdaptiveRetryPolicy : StandardRetryPolicy
    {
        /// <summary>
        /// Return true if the request should be retried.
        /// </summary>
        /// <param name="executionContext">Request context containing the state of the request.</param>
        /// <param name="exception">The exception thrown by the previous request.</param>
        /// <returns>Return true if the request should be retried.</returns>
        public override Task<bool> RetryForExceptionAsync(IExecutionContext executionContext, Exception exception)
        {
            return Task.FromResult(RetryForExceptionSync(exception, executionContext));
        }

        /// <summary>
        /// Waits before retrying a request.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        public override Task WaitBeforeRetryAsync(IExecutionContext executionContext)
        {
            var delay = CalculateRetryDelay(executionContext.RequestContext.Retries, this.MaxBackoffInMilliseconds);
            return Task.Delay(delay, executionContext.RequestContext.CancellationToken);
        }

        /// <summary>
        /// This method uses a token bucket to enforce the maximum sending rate.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <param name="exception">If the prior request failed, this exception is expected to be 
        /// the exception that occurred during the prior request failure.</param>
        public override async Task ObtainSendTokenAsync(IExecutionContext executionContext, Exception exception)
        {
            if (!(await TokenBucket.TryAcquireTokenAsync(1, executionContext.RequestContext.ClientConfig.FastFailRequests,
                executionContext.RequestContext.CancellationToken).ConfigureAwait(false)))
            {
                var whyFail = exception == null ? "The initial request cannot be attempted because capacity could not be obtained"
                    : "While attempting to retry a request error capacity could not be obtained";

                if (executionContext.RequestContext.ClientConfig.FastFailRequests)
                {
                    throw new AmazonClientException($"{whyFail}. The client is configured to fail fast and there is insufficent capacity to attempt the request.", exception);
                }

                //Else we were unable to obtain capacity after looping. 
                throw new AmazonClientException($"{whyFail}. There is insufficent capacity to attempt the request after attempting to obtain capacity multiple times.", exception);
            }                        
        }
    }
}