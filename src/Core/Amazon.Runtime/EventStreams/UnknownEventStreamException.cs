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
    /// This exception is thrown if an exception is retrieved from the event stream and should be modeled,
    /// but a generator function for the strongly-typed exception is not defined.
    /// </summary>
#if !NETSTANDARD
    [Serializable]
#endif
    public sealed class UnknownEventStreamException : EventStreamException
    {
        /// <summary>
        /// The exception type.
        /// </summary>
        public string ExceptionType
        {
            get { return Data[nameof(ExceptionType)].ToString(); }
            private set { Data[nameof(ExceptionType)] = value; }
        }

        /// <summary>
        /// Constructs an EventStreamException.
        /// </summary>
        /// <param name="exceptionType">The exception type.</param>
        public UnknownEventStreamException(string exceptionType)
        {
            ExceptionType = exceptionType;
        }

#if !NETSTANDARD
        /// <summary>
        /// Constructs a new instance of the UnknownEventStreamException class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        private UnknownEventStreamException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}