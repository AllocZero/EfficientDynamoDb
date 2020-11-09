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

namespace Amazon.Runtime.Internal.Transform
{
    /// <summary>
    /// Interface for unmarshallers which unmarshall service responses.
    /// The Unmarshallers are stateless, and only encode the rules for what data 
    /// in the XML stream goes into what members of an object. 
    /// </summary>
    /// <typeparam name="T">The type of object the unmarshaller returns</typeparam>
    /// <typeparam name="R">The type of the XML unmashaller context, which contains the
    /// state of parsing the XML stream. Uaually an instance of 
    /// <c>Amazon.Runtime.Internal.Transform.UnmarshallerContext</c>.</typeparam>
    public interface IResponseUnmarshaller<T, R> : IUnmarshaller<T, R>
    {
        /// <summary>
        /// Extracts an exeption with data from an ErrorResponse.
        /// </summary>
        /// <param name="input">The XML parsing context.</param>
        /// <param name="innerException">An inner exception to be included with the returned exception</param>
        /// <param name="statusCode">The HttpStatusCode from the ErrorResponse</param>
        /// <returns>Either an exception based on the ErrorCode from the ErrorResponse, or the 
        /// general service exception for the service in question.</returns>
        AmazonServiceException UnmarshallException(R input, Exception innerException, HttpStatusCode statusCode);
    }
}
