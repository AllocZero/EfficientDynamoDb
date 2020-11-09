// /*******************************************************************************
//  *  Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
//  *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
//  *  this file except in compliance with the License. A copy of the License is located at
//  *
//  *  http://aws.amazon.com/apache2.0
//  *
//  *  or in the "license" file accompanying this file.
//  *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
//  *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
//  *  specific language governing permissions and limitations under the License.
//  * *****************************************************************************
//  *    __  _    _  ___
//  *   (  )( \/\/ )/ __)
//  *   /__\ \    / \__ \
//  *  (_)(_) \/\/  (___/
//  *
//  *  AWS SDK for .NET
//  *
//  */

using System;
using Amazon.Runtime.EventStreams.Internal;

namespace Amazon.Runtime.EventStreams
{
    /// <summary>
    /// Superclass for Exceptions that come over the event stream, or to wrap other kinds of generic exceptions.
    /// </summary>
#if !NETSTANDARD
    [Serializable]
#endif
    public sealed class EventStreamErrorCodeException : EventStreamException
    {
        /// <summary>
        /// The numeric code identifier for the type of error.
        /// </summary>
        public int ErrorCode
        {
            get { return (int) Data[nameof(ErrorCode)]; }
            private set { Data[nameof(ErrorCode)] = value; }
        }

        /// <summary>
        /// Constructs an EventStreamError.
        /// </summary>
        public EventStreamErrorCodeException(int errorCode) : this(errorCode, null)
        {
        
        }

        /// <summary>
        /// Constructs an EventStreamError.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        /// <param name="message">The error message.</param>
        public EventStreamErrorCodeException(int errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }

#if !NETSTANDARD
        /// <summary>
        /// Constructs a new instance of the EventStreamErrorCodeException class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        private EventStreamErrorCodeException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}