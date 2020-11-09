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
using System.IO;
using System.Net;

namespace Amazon.Runtime.Internal
{
    public class ErrorResponse
    {
        /// <summary>
        /// Error type, one of Sender, Receiver, Unknown
        /// Only applies to XML-based services.
        /// </summary>
        public ErrorType Type { get; set; }

        /// <summary>
        /// Name of the exception class to return
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// RequestId of the error.
        /// Only applies to XML-based services.
        /// </summary>
        public string RequestId { get; set; }

        public Exception InnerException { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
