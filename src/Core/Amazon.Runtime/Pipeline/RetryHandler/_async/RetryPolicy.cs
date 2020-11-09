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
using System.Globalization;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using System.Threading.Tasks;

namespace Amazon.Runtime
{
    /// <summary>
    /// A retry policy specifies all aspects of retry behavior. This includes conditions when the request should be retried,
    /// checks of retry limit, preparing the request before retry and introducing delay (backoff) before retries.
    /// </summary>
    public abstract partial class RetryPolicy
    {
        /// <summary>
        /// Checks if a retry should be performed with the given execution context and exception.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <param name="exception">The exception throw after issuing the request.</param>
        /// <returns>Returns true if the request should be retried, else false. The exception is retried if it matches with clockskew error codes.</returns>
        public async Task<bool> RetryAsync(IExecutionContext executionContext, Exception exception)
        {
            bool canRetry = !RetryLimitReached(executionContext) && CanRetry(executionContext);
            if (canRetry || executionContext.RequestContext.CSMEnabled)
            {
                var isClockSkewError = IsClockskew(executionContext, exception);
                if (isClockSkewError || await RetryForExceptionAsync(executionContext, exception).ConfigureAwait(false))
                {
                    executionContext.RequestContext.IsLastExceptionRetryable = true;
                    if (!canRetry)
                    {
                        return false;
                    }
                    return OnRetry(executionContext, isClockSkewError, IsThrottlingError(exception));
                }
            }
            return false;
        }

        /// <summary>
        /// This method uses a token bucket to enforce the maximum sending rate.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <param name="exception">If the prior request failed, this exception is expected to be 
        /// the exception that occurred during the prior request failure.</param>
        public virtual Task ObtainSendTokenAsync(IExecutionContext executionContext, Exception exception)
        {
#if NETSTANDARD
            return Task.CompletedTask;
#else
            return Task.FromResult(0);
#endif            
        }

        /// <summary>
        /// Return true if the request should be retried for the given exception.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <param name="exception">The exception thrown by the previous request.</param>
        /// <returns>Return true if the request should be retried.</returns>
        public abstract Task<bool> RetryForExceptionAsync(IExecutionContext executionContext, Exception exception);

        /// <summary>
        /// Waits before retrying a request.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        public abstract Task WaitBeforeRetryAsync(IExecutionContext executionContext);

    }
}
