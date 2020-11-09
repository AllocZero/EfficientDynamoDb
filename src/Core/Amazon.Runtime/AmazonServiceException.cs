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
using System.Net;
using System.Text;

namespace Amazon.Runtime
{
    /// <summary>
    /// A base exception for some Amazon Web Services.
    /// <para>
    /// Most exceptions thrown to client code will be service-specific exceptions, though some services
    /// may throw this exception if there is a problem which is caught in the core client code.
    /// </para>
    /// </summary>
#if !NETSTANDARD
    [Serializable]
#endif
    public class AmazonServiceException : Exception
    {
        private ErrorType errorType;
        private string errorCode;
        private string requestId;
        private HttpStatusCode statusCode;

        public AmazonServiceException()
            : base()
        {
        }

        public AmazonServiceException(string message)
            : base(message)
        {
        }

        public AmazonServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public AmazonServiceException(string message, Exception innerException, HttpStatusCode statusCode)
            : base(message, innerException)
        {
            this.statusCode = statusCode;
        }

        public AmazonServiceException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        public AmazonServiceException(string message, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
            : base(message ??
                BuildGenericErrorMessage(errorCode, statusCode))
        {
            this.errorCode = errorCode;
            this.errorType = errorType;
            this.requestId = requestId;
            this.statusCode = statusCode;
        }

        public AmazonServiceException(string message, Exception innerException, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode)
            : base(message ??
                BuildGenericErrorMessage(errorCode, statusCode), 
                innerException)
        {
            this.errorCode = errorCode;
            this.errorType = errorType;
            this.requestId = requestId;
            this.statusCode = statusCode;
        }

        static string BuildGenericErrorMessage(string errorCode, HttpStatusCode statusCode)
        {
            return string.Format(CultureInfo.InvariantCulture,  
                "Error making request with Error Code {0} and Http Status Code {1}. No further error information was returned by the service.", errorCode, statusCode);
        }

        /// <summary>
        /// Whether the error was attributable to <c>Sender</c> or <c>Reciever</c>.
        /// </summary>
        public ErrorType ErrorType
        {
            get { return this.errorType; }
            set { this.errorType = value; }
        }

        /// <summary>
        /// The error code returned by the service
        /// </summary>
        public string ErrorCode
        {
            get { return this.errorCode; }
            set { this.errorCode = value; }
        }

        /// <summary>
        /// The id of the request which generated the exception.
        /// </summary>
        public string RequestId
        {
            get { return this.requestId; }
            set { this.requestId = value; }
        }

        /// <summary>
        /// The HTTP status code from the service response
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return this.statusCode; }
            set { this.statusCode = value; }
        }

        /// <summary>
        /// Flag indicating if the exception is retryable and the associated retry
        /// details. A null value indicates that the exception is not retryable.
        /// </summary>
        public virtual RetryableDetails Retryable
        {
            get
            {
                return null;
            }
        }


#if !NETSTANDARD
        /// <summary>
        /// Constructs a new instance of the AmazonServiceException class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected AmazonServiceException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.errorCode = info.GetString("errorCode");
                this.errorType = (ErrorType)info.GetValue("errorType", typeof(ErrorType));
                this.requestId = info.GetString("requestId");
                this.statusCode = (HttpStatusCode)info.GetValue("statusCode", typeof(HttpStatusCode));
            }
        }

        /// <summary>
        /// Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is a null reference (Nothing in Visual Basic). </exception>
#if BCL35
        [System.Security.Permissions.SecurityPermission(
            System.Security.Permissions.SecurityAction.LinkDemand,
            Flags = System.Security.Permissions.SecurityPermissionFlag.SerializationFormatter)]
#endif
        [System.Security.SecurityCritical]
        // These FxCop rules are giving false-positives for this method
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2123:OverrideLinkDemandsShouldBeIdenticalToBase")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2134:MethodsMustOverrideWithConsistentTransparencyFxCopRule")]
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info != null)
            {
                info.AddValue("errorCode", this.errorCode);
                info.AddValue("errorType", this.errorType);
                info.AddValue("requestId", this.requestId);
                info.AddValue("statusCode", this.statusCode);
            }
        }
#endif
    }

    /// <summary>
    /// Class containing the retryable details for an AmazonServiceException
    /// </summary>
    public class RetryableDetails
    {
        public RetryableDetails(bool throttling)
        {
            Throttling = throttling;
        }

        /// <summary>
        /// This property indicates that this exception is a 
        /// throttling exception and should be subject to congestion
        /// control throttling.
        /// </summary>
        public bool Throttling { get; private set; }
    }
}
