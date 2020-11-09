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
using System.Net;
using System.Text;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// Class that captures the CSM Api Call Attempt Monitoring Events
    /// for each http request and its subsequent response.
    /// </summary>
    public class MonitoringAPICallAttempt : MonitoringAPICall
    {
        public MonitoringAPICallAttempt(IRequestContext requestContext)
            :base(requestContext)
        {
            Type = CSMType.ApiCallAttempt.ToString();
        }

        /// <summary>
        /// Contains the fully-qualified domain name of the endpoint that 
        /// the request attempt was submitted to.
        /// </summary>
        public string Fqdn { get; internal set; }

        /// <summary>
        /// Contains the session token passed 
        /// in the x-amz-security-token header.
        /// </summary>
        public string SessionToken { get; internal set; }

        /// <summary>
        /// Contains the aws_access_key value that 
        /// was used to sign the http request.
        /// </summary>
        public string AccessKey { get; internal set; }

        /// <summary>
        /// Contains the attempt's response status code, 
        /// as returned by the http client.
        /// </summary>
        public int? HttpStatusCode { get; internal set; } = null;

        /// <summary>
        /// Contains the full text (exception object converted to String, 
        /// for example) for any attempt-level failure that is due to 
        /// something other than an Aws exception. The value of this entry 
        /// has a maximum length of 512.
        /// </summary>
        public string SdkExceptionMessage { get; internal set; }

        /// <summary>
        /// Contains the short error name (exception class name, for example) 
        /// for any attempt-level failure that is due to something other 
        /// than an Aws exception.The value of this entry has a maximum length of 128.
        /// </summary>
        public string SdkException { get; internal set; }

        /// <summary>
        /// Contains the Aws exception code returned in the response.
        /// </summary>
        public string AWSException { get; internal set; }
        /// <summary>
        /// Contains the full text of the Aws exception message.
        /// </summary>
        public string AWSExceptionMessage { get; internal set; }

        /// <summary>
        /// Contains the value of the response's x-amzn-RequestId header.
        /// </summary>
        public string XAmznRequestId { get; internal set; }

        /// <summary>
        /// Contains the value of the response's x-amz-request-id header.
        /// </summary>
        public string XAmzRequestId { get; internal set; }

        /// <summary>
        /// Contains the value of the response's x-amz-id-2 header.
        /// </summary>
        public string XAmzId2 { get; internal set; }

        /// <summary>
        /// Contains the elapsed time, in milliseconds, 
        /// between the construction of the http request and the 
        /// point in time where the http response has been parsed 
        /// or the attempt has definitively failed.
        /// </summary>
        public long AttemptLatency { get; internal set; }
    }
}
