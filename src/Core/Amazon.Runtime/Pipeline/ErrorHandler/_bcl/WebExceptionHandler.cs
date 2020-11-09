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

using Amazon.Runtime.Internal.Util;
using System.Collections.Generic;
using System.Globalization;
using System.Net;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// The exception handler for HttpErrorResponseException exception.
    /// </summary>
    public class WebExceptionHandler : ExceptionHandler<WebException>
    {
        public WebExceptionHandler(ILogger logger) :
            base(logger)
        {
        }

        public override bool HandleException(IExecutionContext executionContext, WebException exception)
        {
            var requestContext = executionContext.RequestContext;
            var responseContext = executionContext.ResponseContext;
            var httpErrorResponse = exception.Response as HttpWebResponse;


            var message = string.Format(CultureInfo.InvariantCulture,
                    "A WebException with status {0} was thrown.", exception.Status);

            if (httpErrorResponse != null)
            {
                requestContext.Metrics.AddProperty(Metric.StatusCode, httpErrorResponse.StatusCode);
                throw new AmazonServiceException(message, exception, httpErrorResponse.StatusCode);
            }
            throw new AmazonServiceException(message, exception);
        }
    }
}
