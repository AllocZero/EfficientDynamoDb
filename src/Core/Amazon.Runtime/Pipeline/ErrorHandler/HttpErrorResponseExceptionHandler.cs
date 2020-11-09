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

using Amazon.Runtime.Internal.Transform;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using System;
using System.Diagnostics;
using System.Net;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// The exception handler for HttpErrorResponseException exception.
    /// </summary>
    public class HttpErrorResponseExceptionHandler : ExceptionHandler<HttpErrorResponseException>
    {
        /// <summary>
        /// The constructor for HttpErrorResponseExceptionHandler.
        /// </summary>
        /// <param name="logger">in instance of ILogger.</param>
        public HttpErrorResponseExceptionHandler(ILogger logger) :
            base(logger)
        {
        }

        /// <summary>
        /// Handles an exception for the given execution context.
        /// </summary>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        /// <param name="exception">The exception to handle.</param>
        /// <returns>
        /// Returns a boolean value which indicates if the original exception
        /// should be rethrown.
        /// This method can also throw a new exception to replace the original exception.
        /// </returns>
        public override bool HandleException(IExecutionContext executionContext, HttpErrorResponseException exception)
        {
            var requestContext = executionContext.RequestContext;
            var httpErrorResponse = exception.Response;

            // If 404 was suppressed and successfully unmarshalled,
            // don't rethrow the original exception.
            if (HandleSuppressed404(executionContext, httpErrorResponse))
                return false;

            requestContext.Metrics.AddProperty(Metric.StatusCode, httpErrorResponse.StatusCode);

            AmazonServiceException errorResponseException = null;
            // Unmarshall the service error response and throw the corresponding service exception.
            try
            {
                using (httpErrorResponse.ResponseBody)
                {
                    var unmarshaller = requestContext.Unmarshaller;

                    var errorContext = unmarshaller.CreateContext(httpErrorResponse,
                        true,
                        httpErrorResponse.ResponseBody.OpenResponse(),
                        requestContext.Metrics,
                        true);

                    try
                    {
                        errorResponseException = unmarshaller.UnmarshallException(errorContext,
                            exception, httpErrorResponse.StatusCode);
                    }
                    catch (Exception e)
                    {
                        // Rethrow Amazon service or client exceptions 
                        if (e is AmazonServiceException ||
                            e is AmazonClientException)
                        {
                            throw;
                        }

                        // Else, there was an issue with the response body, throw AmazonUnmarshallingException
                        var requestId = httpErrorResponse.GetHeaderValue(HeaderKeys.RequestIdHeader);
                        var body = errorContext.ResponseBody;
                        throw new AmazonUnmarshallingException(requestId, lastKnownLocation: null, responseBody: body,
                            innerException: e, statusCode : httpErrorResponse.StatusCode);
                    }

                    requestContext.Metrics.AddProperty(Metric.AWSRequestID, errorResponseException.RequestId);
                    requestContext.Metrics.AddProperty(Metric.AWSErrorCode, errorResponseException.ErrorCode);

                    var logResponseBody = requestContext.ClientConfig.LogResponse ||
                        AWSConfigs.LoggingConfig.LogResponses != ResponseLoggingOption.Never;
                    if (logResponseBody)
                    {
                        this.Logger.Error(errorResponseException, "Received error response: [{0}]",
                            errorContext.ResponseBody);
                    }
                }
            }
            catch (Exception unmarshallException)
            {
                this.Logger.Error(unmarshallException, "Failed to unmarshall a service error response.");
                throw;
            }

            throw errorResponseException;
        }

        /// <summary>
        /// Checks if a HTTP 404 status code is returned which needs to be suppressed and 
        /// processes it.
        /// If a suppressed 404 is present, it unmarshalls the response and returns true to 
        /// indicate that a suppressed 404 was processed, else returns false.
        /// </summary>
        /// <param name="executionContext">The execution context, it contains the
        /// request and response context.</param>
        /// <param name="httpErrorResponse"></param>
        /// <returns>
        /// If a suppressed 404 is present, returns true, else returns false.
        /// </returns>
        private bool HandleSuppressed404(IExecutionContext executionContext, IWebResponseData httpErrorResponse)
        {
            var requestContext = executionContext.RequestContext;
            var responseContext = executionContext.ResponseContext;

            // If the error is a 404 and the request is configured to supress it,
            // then unmarshall as much as we can.
            if (httpErrorResponse != null &&
                httpErrorResponse.StatusCode == HttpStatusCode.NotFound &&
                requestContext.Request.Suppress404Exceptions)
            {
                using (httpErrorResponse.ResponseBody)
                {
                    var unmarshaller = requestContext.Unmarshaller;
                    var readEntireResponse = requestContext.ClientConfig.LogResponse ||
                            AWSConfigs.LoggingConfig.LogResponses != ResponseLoggingOption.Never;

                    UnmarshallerContext errorContext = unmarshaller.CreateContext(
                        httpErrorResponse,
                        readEntireResponse,
                        httpErrorResponse.ResponseBody.OpenResponse(),
                        requestContext.Metrics,
                        true);
                    try
                    {
                        responseContext.Response = unmarshaller.Unmarshall(errorContext);
                        responseContext.Response.ContentLength = httpErrorResponse.ContentLength;
                        responseContext.Response.HttpStatusCode = httpErrorResponse.StatusCode;
                        return true;
                    }
                    catch (Exception unmarshallException)
                    {
                        this.Logger.Debug(unmarshallException, "Failed to unmarshall 404 response when it was supressed.");
                    }
                }
            }
            return false;
        }
    }
}
