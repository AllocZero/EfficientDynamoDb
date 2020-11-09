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
using System.IO;
using System.Net;
using System.Threading;
using Amazon.Runtime.Internal;
using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using AWSSDK.Runtime.Internal.Util;

namespace Amazon.Runtime
{
    /// <summary>
    /// A retry policy specifies all aspects of retry behavior. This includes conditions when the request should be retried,
    /// checks of retry limit, preparing the request before retry and introducing delay (backoff) before retries.
    /// </summary>
    public abstract partial class RetryPolicy
    {
        /// <summary>
        /// Maximum number of retries to be performed.
        /// This does not count the initial request.
        /// </summary>
        public int MaxRetries { get; protected set; }
                
        /// <summary>
        /// The logger used to log messages.
        /// </summary>
        public ILogger Logger { get; set; }

        /// <summary>
        /// The standard set of throttling error codes
        /// </summary>
        public virtual ICollection<string> ThrottlingErrorCodes { get; protected set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Throttling",
            "ThrottlingException",
            "ThrottledException",
            "RequestThrottledException",
            "TooManyRequestsException",
            "ProvisionedThroughputExceededException",
            "TransactionInProgressException",
            "RequestLimitExceeded",
            "BandwidthLimitExceeded",
            "LimitExceededException",
            "RequestThrottled",
            "SlowDown",
            "PriorRequestNotComplete"                
        };
                
        /// <summary>
        /// The standard set of timeout error codes to retry on.
        /// </summary>
        public ICollection<string> TimeoutErrorCodesToRetryOn { get; protected set; } = new HashSet<string>()
        {
            "RequestTimeout",
            "RequestTimeoutException"
        };

        /// <summary>
        /// List of AWS specific error codes which are returned as part of the error response.
        /// These error codes will be retried.
        /// </summary>
        public ICollection<string> ErrorCodesToRetryOn { get; protected set; } = new HashSet<string>();

        #region Transient errors

        /// <summary>
        /// The standard set of transient error, HTTP status codes to retry on.
        /// 502 and 504 are returned by proxies. These can also be returned for 
        /// S3 accelerate requests which are served by CloudFront.
        /// </summary>
        public ICollection<HttpStatusCode> HttpStatusCodesToRetryOn { get; protected set; } = new HashSet<HttpStatusCode>
        {
            HttpStatusCode.InternalServerError,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.BadGateway,
            HttpStatusCode.GatewayTimeout
        };

        /// <summary>
        /// Set of web exception status codes to retry on.
        /// </summary>
        public ICollection<WebExceptionStatus> WebExceptionStatusesToRetryOn { get; protected set; } = new HashSet<WebExceptionStatus>
        {
            WebExceptionStatus.ConnectFailure,
            WebExceptionStatus.ConnectionClosed,
            WebExceptionStatus.KeepAliveFailure,
            WebExceptionStatus.NameResolutionFailure,
            WebExceptionStatus.ReceiveFailure,
            WebExceptionStatus.SendFailure,
            WebExceptionStatus.Timeout,
        };

        #endregion

        /// <summary>
        /// This parameter serves as the value to the CapacityManager.Container datastructure.
        /// Its properties include the available capacity left for making a retry request and the maximum
        /// capacity size.
        /// </summary>
        protected RetryCapacity RetryCapacity { get; set; }

        /// <summary>
        /// Checks if a retry should be performed with the given execution context and exception.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <param name="exception">The exception thrown after issuing the request.</param>
        /// <returns>Returns true if the request should be retried, else false. The exception is retried if it matches with clockskew error codes.</returns>
        public bool Retry(IExecutionContext executionContext, Exception exception)
        {
            // Boolean that denotes retries have not exceeded maxretries and request is rewindable
            bool canRetry = !RetryLimitReached(executionContext) && CanRetry(executionContext);
            // If canRetry is false, we still want to evaluate the exception if its retryable or not,
            // is CSM is enabled. This is necessary to set the IsLastExceptionRetryable property on 
            // CSM Call Attempt. For S3, with the BucketRegion mismatch exception, an overhead of 100-
            // 115 ms was added(because of GetPreSignedUrl and Http HEAD requests).
            if (canRetry || executionContext.RequestContext.CSMEnabled)
            {
                var isClockSkewError = IsClockskew(executionContext, exception);
                if (isClockSkewError || RetryForException(executionContext, exception))
                {
                    executionContext.RequestContext.IsLastExceptionRetryable = true;
                    // If CSM is enabled but canRetry was false, we should not retry the request.
                    // Return false after successfully evaluating the last exception for retryable.
                    if (!canRetry)
                    {
                        return false;
                    }

                    executionContext.RequestContext.LastCapacityType = IsServiceTimeoutError(exception) ? 
                        CapacityManager.CapacityType.Timeout : CapacityManager.CapacityType.Retry;
                    return OnRetry(executionContext, isClockSkewError,  IsThrottlingError(exception));
                }
            }
            return false;
        }
                
        /// <summary>
        /// Returns true if the request is in a state where it can be retried, else false.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <returns>Returns true if the request is in a state where it can be retried, else false.</returns>
        public abstract bool CanRetry(IExecutionContext executionContext);

        /// <summary>
        /// Return true if the request should be retried for the given exception.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <param name="exception">The exception thrown by the previous request.</param>
        /// <returns>Return true if the request should be retried.</returns>
        public abstract bool RetryForException(IExecutionContext executionContext, Exception exception);

        /// <summary>
        /// Checks if the retry limit is reached.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <returns>Return false if the request can be retried, based on number of retries.</returns>
        public abstract bool RetryLimitReached(IExecutionContext executionContext);

        /// <summary>
        /// Waits before retrying a request.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        public abstract void WaitBeforeRetry(IExecutionContext executionContext);

        /// <summary>
        /// Virtual method that gets called on a successful request response.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        public virtual void NotifySuccess(IExecutionContext executionContext)
        {            
        }
        
        /// <summary>
         /// Virtual method that gets called before a retry request is initiated. The value 
         /// returned is True by default(retry throttling feature is disabled).
         /// </summary>
         /// <param name="executionContext">The execution context which contains both the
         /// requests and response context.</param>
         public virtual bool OnRetry(IExecutionContext executionContext)
         {
             return true;
         }
         
        /// <summary>
        /// Virtual method that gets called before a retry request is initiated. The value 
        /// returned is True by default(retry throttling feature is disabled).
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <param name="bypassAcquireCapacity">true to bypass any attempt to acquire capacity on a retry</param>
        public virtual bool OnRetry(IExecutionContext executionContext, bool bypassAcquireCapacity)
        {
            return true;
        }

        /// <summary>
        /// Virtual method that gets called before a retry request is initiated. The value 
        /// returned is True by default(retry throttling feature is disabled).
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <param name="bypassAcquireCapacity">true to bypass any attempt to acquire capacity on a retry</param>
        /// <param name="isThrottlingError">true if the error that will be retried is a throttling error</param>
        public virtual bool OnRetry(IExecutionContext executionContext, bool bypassAcquireCapacity, bool isThrottlingError)
        {
            return OnRetry(executionContext, bypassAcquireCapacity);
        }


        /// <summary>
        /// This method uses a token bucket to enforce the maximum sending rate.
        /// </summary>
        /// <param name="executionContext">The execution context which contains both the
        /// requests and response context.</param>
        /// <param name="exception">If the prior request failed, this exception is expected to be 
        /// the exception that occurred during the prior request failure.</param>
        public virtual void ObtainSendToken(IExecutionContext executionContext, Exception exception)
        {
        }

        /// <summary>
        /// Determines if an AmazonServiceException is a throttling error
        /// </summary>
        /// <param name="exception">The current exception to check.</param>
        /// <returns>true if it is a throttling error else false.</returns>
        public virtual bool IsThrottlingError(Exception exception)
        {
            var serviceException = exception as AmazonServiceException;

            return serviceException?.Retryable?.Throttling == true ||
                ThrottlingErrorCodes.Contains(serviceException?.ErrorCode);
        }

        /// <summary>
        /// Determines if an AmazonServiceException is a transient error that
        /// should be retried.
        /// </summary>
        /// <param name="executionContext">The current execution context</param>
        /// <param name="exception">The current exception to check.</param>
        /// <returns>true if the exception is a transient error else false.</returns>
        public virtual bool IsTransientError(IExecutionContext executionContext, Exception exception)
        {
            // An IOException was thrown by the underlying http client.
            if (exception is IOException)
            {

#if !NETSTANDARD  // ThreadAbortException is not NetStandard

                // Don't retry IOExceptions that are caused by a ThreadAbortException
                if (ExceptionUtils.IsInnerException<ThreadAbortException>(exception))
                    return false;

#endif
                // Retry all other IOExceptions
                return true;
            }
            else if (ExceptionUtils.IsInnerException<IOException>(exception))
            {
                return true;
            }            

            //Check for AmazonServiceExceptions specifically
            var serviceException = exception as AmazonServiceException;
            if(serviceException != null)
            {
                //Check if the exception is marked retryable.
                if (serviceException.Retryable != null)
                {
                    return true;
                }

                //Check for specific HTTP status codes that are associated with transient
                //service errors as long as they are not throttling errors.
                if (HttpStatusCodesToRetryOn.Contains(serviceException.StatusCode) &&
                    !IsThrottlingError(exception))
                {
                    return true;
                }

                //Check for successful responses that couldn't be unmarshalled. These should be considered
                //transient errors because the payload could have been corrupted after OK was sent in the
                //header.
                if(serviceException.StatusCode == HttpStatusCode.OK && serviceException is AmazonUnmarshallingException)
                {
                    return true;
                }
            }

            //Check for WebExceptions that are considered transient
            WebException webException;
            if (ExceptionUtils.IsInnerException(exception, out webException))
            {
                if (WebExceptionStatusesToRetryOn.Contains(webException.Status))
                {
                    return true;
                }
            }

#if NETSTANDARD
            // Version 7.35 libcurl which is the default version installed with Ubuntu 14.04 
            // has issues under high concurrency causing response streams being disposed
            // during unmarshalling. To work around this issue will add the ObjectDisposedException
            // to the list of exceptions to retry.
            if (ExceptionUtils.IsInnerException<ObjectDisposedException>(exception))
                return true;

            //If it isn't a serviceException that we already processed for StatusCode and it
            //is a HttpRequestException, then it is a network type error that did not reach the
            //service and it should be retried.            
            if (serviceException == null && exception is System.Net.Http.HttpRequestException)
            {
                return true;
            }

            if (exception is OperationCanceledException)
            {
                if (!executionContext.RequestContext.CancellationToken.IsCancellationRequested)
                {
                    //OperationCanceledException thrown by HttpClient not the CancellationToken supplied by the user.
                    //This exception can wrap at least IOExceptions, ObjectDisposedExceptions and should be retried
                    return true;
                }
            }
#endif
            return false;
        }

        /// <summary>
        /// Determines if the exception is a known timeout error code that
        /// should be retried under the timeout error category.
        /// </summary>
        /// <param name="exception">The current exception to check.</param>
        /// <returns>true if the exception is considered a timeout else false</returns>
        public virtual bool IsServiceTimeoutError(Exception exception)
        {
            var serviceException = exception as AmazonServiceException;
            return TimeoutErrorCodesToRetryOn.Contains(serviceException?.ErrorCode);
        }
        

        #region Clock skew correction

        private static HashSet<string> clockSkewErrorCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "RequestTimeTooSkewed",
            "RequestExpired",
            "InvalidSignatureException",
            "SignatureDoesNotMatch",
            "AuthFailure",
            "RequestExpired",
            "RequestInTheFuture",
        };

        private const string clockSkewMessageFormat = "Identified clock skew: local time = {0}, local time with correction = {1}, current clock skew correction = {2}, server time = {3}, service endpoint = {4}.";
        private const string clockSkewUpdatedFormat = "Setting clock skew correction: new clock skew correction = {0}, service endpoint = {1}.";
        private const string clockSkewMessageParen = "(";
        private const string clockSkewMessagePlusSeparator = " + ";
        private const string clockSkewMessageMinusSeparator = " - ";
        private static TimeSpan clockSkewMaxThreshold = TimeSpan.FromMinutes(5);

        private bool IsClockskew(IExecutionContext executionContext, Exception exception)
        {
            var clientConfig = executionContext.RequestContext.ClientConfig;
            var ase = exception as AmazonServiceException;

            var isHead =
                executionContext.RequestContext.Request != null &&
                string.Equals(executionContext.RequestContext.Request.HttpMethod, "HEAD", StringComparison.Ordinal);
            var isClockskewErrorCode =
                ase != null &&
                (ase.ErrorCode == null || clockSkewErrorCodes.Contains(ase.ErrorCode));

            if (isHead || isClockskewErrorCode)
            {
                var endpoint = executionContext.RequestContext.Request.Endpoint.ToString();
                var realNow = AWSConfigs.utcNowSource();
                var correctedNow = CorrectClockSkew.GetCorrectedUtcNowForEndpoint(endpoint);

                DateTime serverTime;

                // Try getting server time from the headers
                bool serverTimeDetermined = TryParseDateHeader(ase, out serverTime);

                // If that fails, try to parse it from the exception message
                if (!serverTimeDetermined)
                    serverTimeDetermined = TryParseExceptionMessage(ase, out serverTime);

                if (serverTimeDetermined)
                {
                    // using accurate server time, calculate correction if local time is off
                    serverTime = serverTime.ToUniversalTime();
                    var diff = correctedNow - serverTime;
                    var absDiff = diff.Ticks < 0 ? -diff : diff;
                    if (absDiff > clockSkewMaxThreshold)
                    {
                        var newCorrection = serverTime - realNow;
                        Logger.InfoFormat(clockSkewMessageFormat,
                            realNow, correctedNow, clientConfig.ClockOffset, serverTime, endpoint);

                        // Always set the correction, for informational purposes
                        CorrectClockSkew.SetClockCorrectionForEndpoint(endpoint, newCorrection);

                        var shouldRetry = AWSConfigs.CorrectForClockSkew && !AWSConfigs.ManualClockCorrection.HasValue;

                        // Only retry if clock skew correction is not disabled
                        if (shouldRetry)
                        {
                            // Set clock skew correction
                            Logger.InfoFormat(clockSkewUpdatedFormat, newCorrection, endpoint);
                            executionContext.RequestContext.IsSigned = false;
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        private static bool TryParseDateHeader(AmazonServiceException ase, out DateTime serverTime)
        {
            var webData = GetWebData(ase);

            if (webData != null)
            {
                // parse server time from "Date" header, if possible
                var dateValue = webData.GetHeaderValue(HeaderKeys.DateHeader);
                if (!string.IsNullOrEmpty(dateValue))
                {
                    if (DateTime.TryParseExact(
                            dateValue,
                            AWSSDKUtils.GMTDateFormat,
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.AssumeUniversal,
                            out serverTime))
                    {
                        return true;
                    }
                }
            }

            serverTime = DateTime.MinValue;
            return false;
        }
        private static bool TryParseExceptionMessage(AmazonServiceException ase, out DateTime serverTime)
        {
            if (ase != null && !string.IsNullOrEmpty(ase.Message))
            {
                var message = ase.Message;

                // parse server time from exception message, if possible
                var parenIndex = message.IndexOf(clockSkewMessageParen, StringComparison.Ordinal);
                if (parenIndex >= 0)
                {
                    parenIndex++;

                    // Locate " + " or " - " separator that follows the server time string
                    var separatorIndex = message.IndexOf(clockSkewMessagePlusSeparator, parenIndex, StringComparison.Ordinal);
                    if (separatorIndex < 0)
                        separatorIndex = message.IndexOf(clockSkewMessageMinusSeparator, parenIndex, StringComparison.Ordinal);

                    // Get the server time string and parse it
                    if (separatorIndex > parenIndex)
                    {
                        var timestamp = message.Substring(parenIndex, separatorIndex - parenIndex);
                        if (DateTime.TryParseExact(
                                timestamp,
                                AWSSDKUtils.ISO8601BasicDateTimeFormat,
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.AssumeUniversal,
                                out serverTime))
                        {
                            return true;
                        }
                    }
                }
            }

            serverTime = DateTime.MinValue;
            return false;
        }

        #endregion

        private static IWebResponseData GetWebData(AmazonServiceException ase)
        {
            if (ase != null)
            {
                Exception e = ase;
                do
                {
                    var here = e as HttpErrorResponseException;
                    if (here != null)
                        return here.Response;
                    e = e.InnerException;
                } while (e != null);
            }

            return null;
        }

        protected static bool ContainErrorMessage(Exception exception, HashSet<string> errorMessages)
        {
            if (exception == null)
                return false;

            if (errorMessages.Contains(exception.Message))
                return true;
            return ContainErrorMessage(exception.InnerException, errorMessages);
        }                
    }
}
