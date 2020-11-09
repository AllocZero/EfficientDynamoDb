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
using System.Text;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// Class that captures the CSM Api Call Monitoring Events
    /// for the entire SDK call. This is processed once.
    /// </summary>
    public class MonitoringAPICallEvent : MonitoringAPICall
    {
        public MonitoringAPICallEvent(IRequestContext requestContext)
            :base(requestContext)
        {
            Type = CSMType.ApiCall.ToString();
        }
        /// <summary>
        /// Contains the total number of attempts that were made 
        /// by the service client to fulfill this request before succeeding or failing
        /// </summary>
        public int AttemptCount { get; internal set; }

        /// <summary>
        /// Contains the elapsed time, in milliseconds, 
        /// between when the Api Call was begun and when a 
        /// final response or error is manifested to the caller
        /// </summary>
        public long Latency { get; internal set; }

        /// <summary>
        /// a boolean (0/1) value that is 0 unless the Api call failed
        /// and the final attempt returned a retryable error.This entry should be
        /// serialized as a numeric 0/1 value. This is mapped to MaxRetriesExceeded
        /// when serialized to a UDP datagram.
        /// </summary>
        public bool IsLastExceptionRetryable { get; internal set; }

        /// <summary>
        /// Contains the full text (exception object
        /// converted to String, for example) for an attempt-level failure that is due to
        /// something other than an Aws exception that occurred on the last attempt to
        /// fulfill the Api call.The value of this entry has a maximum length of 512.
        /// </summary>
        public string FinalSdkExceptionMessage { get; internal set; }

        /// <summary>
        /// Contains the short error name (exception
        /// class name, for example) for a failure that is due to something other than an
        /// Aws exception that occurred on the last attempt to fulfill an Api call.See
        /// the SdkException entry for more details.The value of this entry has a
        /// maximum length of 128.
        /// </summary>
        public string FinalSdkException { get; internal set; }

        /// <summary>
        /// Contains the Aws exception code
        /// returned in the response to the final attempt at fulfilling this API call.
        /// The value of this entry has a maximum length of 128.
        /// </summary>
        public string FinalAWSException { get; internal set; }

        /// <summary>
        /// Contains the full text of the
        /// Aws exception message returned in the response to the final attempt at
        /// fulfilling this API call.The value of this entry has a maximum length limit
     
        /// of 512.
        /// </summary>
        public string FinalAWSExceptionMessage { get; internal set; }

        /// <summary>
        /// Contains the attempt's response status code, 
        /// as returned by the http client.
        /// </summary>
        public int? FinalHttpStatusCode { get; internal set; } = null;
    }
}
