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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// The default implementation of the adaptive retry policy.
    /// </summary>
    public partial class AdaptiveRetryPolicy : StandardRetryPolicy
    {        
        protected TokenBucket TokenBucket { get; set; } = new TokenBucket();

        /// <summary>
        /// Constructor for AdaptiveRetryPolicy.
        /// </summary>
        /// <param name="maxRetries">The maximum number of retries before throwing
        /// back a exception. This does not count the initial request.</param>
        public AdaptiveRetryPolicy(int maxRetries) : base(maxRetries)
        {            
        }

        /// <summary>
        /// Constructor for AdaptiveRetryPolicy.
        /// </summary>
        /// <param name="config">The Client config object. This is used to 
        /// retrieve the maximum number of retries  before throwing
        /// back a exception(This does not count the initial request) and
        /// the service URL for the request.</param>
        public AdaptiveRetryPolicy(IClientConfig config) : base(config)
        {            
        }

        /// <summary>
        /// OnRetry is called when a retry request is initiated to determine if the request will be retried.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <param name="bypassAcquireCapacity">true to bypass any attempt to acquire capacity on a retry</param>
        /// <param name="isThrottlingError">true if the error that will be retried is a throtting error</param>    
        /// <returns>True if retry throttling is disabled or retry throttling is enabled and capacity can be obtained.</returns>
        public override bool OnRetry(IExecutionContext executionContext, bool bypassAcquireCapacity, bool isThrottlingError)        
        {
            TokenBucket.UpdateClientSendingRate(isThrottlingError);

            return base.OnRetry(executionContext, bypassAcquireCapacity, isThrottlingError);
        }

        /// <summary>
        /// This method uses a token bucket to enforce the maximum sending rate.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <param name="exception">If the prior request failed, this exception is expected to be 
        /// the exception that occurred during the prior request failure.</param>
        public override void ObtainSendToken(IExecutionContext executionContext, Exception exception)
        {
            if (!TokenBucket.TryAcquireToken(1, executionContext.RequestContext.ClientConfig.FastFailRequests))
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

        /// <summary>
        /// Virtual method that gets called on a success Response. If its a retry success response, the entire 
        /// retry acquired capacity is released(default is 5). If its just a success response a lesser value capacity 
        /// is released(default is 1).
        /// </summary>
        /// <param name="executionContext">Request context containing the state of the request.</param>
        public override void NotifySuccess(IExecutionContext executionContext)
        {
            TokenBucket.UpdateClientSendingRate(false);
            base.NotifySuccess(executionContext);            
        }        
    }
}
