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
using AWSSDK.Runtime.Internal.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// The default implementation of the legacy retry policy.
    /// </summary>
    public partial class DefaultRetryPolicy : RetryPolicy
    {
        //The status code returned from a service request when an invalid endpoint is used.
        private const int INVALID_ENDPOINT_EXCEPTION_STATUSCODE = 421;        
        //Holds on to the singleton instance.
        private static readonly CapacityManager _capacityManagerInstance = new CapacityManager(throttleRetryCount: 100, throttleRetryCost: 5, throttleCost: 1);

        private static readonly HashSet<string> _netStandardRetryErrorMessages = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "The server returned an invalid or unrecognized response",
            "The connection with the server was terminated abnormally",
            "An error occurred while sending the request.",
            "Failed sending data to the peer"
        };

        /// <summary>
        /// The maximum value of exponential backoff in milliseconds, which will be used to wait
        /// before retrying a request. The default is 30000 milliseconds.
        /// </summary>
        public int MaxBackoffInMilliseconds { get; set; } = 30000;
                                
        /// <summary>
        /// Constructor for DefaultRetryPolicy.
        /// </summary>
        /// <param name="maxRetries">The maximum number of retries before throwing
        /// back a exception. This does not count the initial request.</param>
        public DefaultRetryPolicy(int maxRetries)
        {
            this.MaxRetries = maxRetries;
        }

        /// <summary>
        /// Constructor for DefaultRetryPolicy.
        /// </summary>
        /// <param name="config">The Client config object. This is used to 
        /// retrieve the maximum number of retries  before throwing
        /// back a exception(This does not count the initial request) and
        /// the service URL for the request.</param>
        public DefaultRetryPolicy(IClientConfig config)
        {
            this.MaxRetries = config.MaxErrorRetry;
            if (config.ThrottleRetries)
            {
                string serviceURL = config.DetermineServiceURL();
                RetryCapacity = _capacityManagerInstance.GetRetryCapacity(serviceURL);
            } 
        }

        /// <summary>
        /// Returns true if the request is in a state where it can be retried, else false.
        /// </summary>
        /// <param name="executionContext">Request context containing the state of the request.</param>
        /// <returns>Returns true if the request is in a state where it can be retried, else false.</returns>
        public override bool CanRetry(IExecutionContext executionContext)
        {
            return executionContext.RequestContext.Request.IsRequestStreamRewindable();
        }

        /// <summary>
        /// Return true if the request should be retried.
        /// </summary>
        /// <param name="executionContext">Request context containing the state of the request.</param>
        /// <param name="exception">The exception thrown by the previous request.</param>
        /// <returns>Return true if the request should be retried.</returns>
        public override bool RetryForException(IExecutionContext executionContext, Exception exception)
        {
            return RetryForExceptionSync(exception, executionContext);
        }
        
        /// <summary>
        /// Virtual method that gets called when a retry request is initiated. If retry throttling is
        /// enabled, the value returned is true if the required capacity is retured, false otherwise. 
        /// If retry throttling is disabled, true is returned.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        public override bool OnRetry(IExecutionContext executionContext)
        {
            return OnRetry(executionContext, false, false);
        }

        /// <summary>
        /// Virtual method that gets called when a retry request is initiated. If retry throttling is
        /// enabled, the value returned is true if the required capacity is retured, false otherwise. 
        /// If retry throttling is disabled, true is returned.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <param name="bypassAcquireCapacity">true to bypass any attempt to acquire capacity on a retry</param>
        public override bool OnRetry(IExecutionContext executionContext, bool bypassAcquireCapacity)
        {
            return OnRetry(executionContext, bypassAcquireCapacity, false);
        }

        /// <summary>
        /// Virtual method that gets called when a retry request is initiated. If retry throttling is
        /// enabled, the value returned is true if the required capacity is retured, false otherwise. 
        /// If retry throttling is disabled, true is returned.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <param name="bypassAcquireCapacity">true to bypass any attempt to acquire capacity on a retry</param>
        /// <param name="isThrottlingError">true if the error that will be retried is a throtting error</param>        
        public override bool OnRetry(IExecutionContext executionContext, bool bypassAcquireCapacity, bool isThrottlingError)
        {
            if (!bypassAcquireCapacity && executionContext.RequestContext.ClientConfig.ThrottleRetries && RetryCapacity != null)
            {
                return _capacityManagerInstance.TryAcquireCapacity(RetryCapacity, executionContext.RequestContext.LastCapacityType);
            }
            
            return true;            
        }

        /// <summary>
        /// Virtual method that gets called on a success Response. If its a retry success response, the entire 
        /// retry acquired capacity is released(default is 5). If its just a success response a lesser value capacity 
        /// is released(default is 1).
        /// </summary>
        /// <param name="executionContext">Request context containing the state of the request.</param>
        public override void NotifySuccess(IExecutionContext executionContext)
        {
            if(executionContext.RequestContext.ClientConfig.ThrottleRetries && RetryCapacity != null)
            {
                _capacityManagerInstance.ReleaseCapacity(executionContext.RequestContext.LastCapacityType, RetryCapacity);
            }
        }
        /// <summary>
        /// Perform the processor-bound portion of the RetryForException logic.
        /// This is shared by the sync, async, and APM versions of the RetryForException method.
        /// </summary>
        /// <param name="exception">The exception thrown by the previous request.</param>
        /// <returns>Return true if the request should be retried.</returns>
        private bool RetryForExceptionSync(Exception exception)
        {
            return RetryForExceptionSync(exception, null);
        }
        /// <summary>
        /// Perform the processor-bound portion of the RetryForException logic.
        /// This is shared by the sync, async, and APM versions of the RetryForException method.
        /// </summary>
        /// <param name="exception">The exception thrown by the previous request.</param>
        /// <param name="executionContext">Request context containing the state of the request.</param>
        /// <returns>Return true if the request should be retried.</returns>
        private bool RetryForExceptionSync(Exception exception, IExecutionContext executionContext)
        {
            // AmazonServiceException is thrown by ErrorHandler if it is this type of exception.
            var serviceException = exception as AmazonServiceException;

            // To try and smooth out an occasional throttling error, we'll pause and 
            // retry, hoping that the pause is long enough for the request to get through
            // the next time. Only the error code should be used to determine if an 
            // error is a throttling error.
            if (IsThrottlingError(exception))
            {
                return true;
            }

            // Check for transient errors, but we need to use
            // an exponential back-off strategy so that we don't overload
            // a server with a flood of retries. If we've surpassed our
            // retry limit we handle the error response as a non-retryable
            // error and go ahead and throw it back to the user as an exception.
            if (IsTransientError(executionContext, exception) || IsServiceTimeoutError(exception))
            {
                return true;
            }                        

            //Check for Invalid Endpoint Exception indicating that the Endpoint Discovery
            //endpoint used was invalid for the request. One retry attempt is allowed for this
            //type of exception.            
            if (serviceException?.StatusCode == (HttpStatusCode)INVALID_ENDPOINT_EXCEPTION_STATUSCODE)
            {
                if (executionContext.RequestContext.EndpointDiscoveryRetries < 1)
                {
                    executionContext.RequestContext.EndpointDiscoveryRetries++;
                    return true;
                }

                return false;
            }

            return false;
        }

        /// <summary>
        /// Checks if the retry limit is reached.
        /// </summary>
        /// <param name="executionContext">Request context containing the state of the request.</param>
        /// <returns>Return false if the request can be retried, based on number of retries.</returns>
        public override bool RetryLimitReached(IExecutionContext executionContext)
        {
            return executionContext.RequestContext.Retries >= this.MaxRetries;
        }

        /// <summary>
        /// Waits before retrying a request. The default policy implements a exponential backoff.
        /// </summary>
        /// <param name="executionContext">Request context containing the state of the request.</param>
        public override void WaitBeforeRetry(IExecutionContext executionContext)
        {
            DefaultRetryPolicy.WaitBeforeRetry(executionContext.RequestContext.Retries, this.MaxBackoffInMilliseconds);
        }

        /// <summary>
        /// Waits for an amount of time using an exponential backoff algorithm.
        /// </summary>
        /// <param name="retries">The request retry index. The first request is expected to be 0 while 
        /// the first retry will be 1.</param>
        /// <param name="maxBackoffInMilliseconds">The max number of milliseconds to wait</param>
        public static void WaitBeforeRetry(int retries, int maxBackoffInMilliseconds)
        {
            AWSSDKUtils.Sleep(CalculateRetryDelay(retries, maxBackoffInMilliseconds));
        }        

        private static int CalculateRetryDelay(int retries, int maxBackoffInMilliseconds)
        {
            int delay;
            
            if (retries < 12 )  delay = Convert.ToInt32(Math.Pow(4, retries) * 100.0);
            else                delay = Int32.MaxValue;

            if (retries > 0 && (delay > maxBackoffInMilliseconds || delay <= 0))
                delay = maxBackoffInMilliseconds;
            return delay;
        }

        [Obsolete("This method is no longer used within DefaultRetryPolicy")]
        protected static bool ContainErrorMessage(Exception exception)
        {
            return ContainErrorMessage(exception, _netStandardRetryErrorMessages);            
        }

        [Obsolete("This method has been moved to AWSSDK.Runtime.Internal.Util.ExceptionUtils")]
        protected static bool IsInnerException<T>(Exception exception)
            where T : Exception
        {
            return ExceptionUtils.IsInnerException<T>(exception);            
        }

        [Obsolete("This method has been moved to AWSSDK.Runtime.Internal.Util.ExceptionUtils")]
        protected static bool IsInnerException<T>(Exception exception, out T inner)
            where T : Exception
        {
            return ExceptionUtils.IsInnerException<T>(exception, out inner);
        }
    }
}
