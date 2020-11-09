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

namespace Amazon.Runtime
{
    /// <summary>
    /// This exception is thrown when there is a parse error on the response back from AWS.
    /// </summary>
#if !NETSTANDARD
    [Serializable]
#endif
    public class AmazonUnmarshallingException : AmazonServiceException
    {
        #region Constructors

        public AmazonUnmarshallingException(string requestId, string lastKnownLocation, Exception innerException)
            : base("Error unmarshalling response back from AWS.", innerException)
        {
            this.RequestId = requestId;
            this.LastKnownLocation = lastKnownLocation;
        }

        public AmazonUnmarshallingException(string requestId, string lastKnownLocation, string responseBody, Exception innerException)
            : base("Error unmarshalling response back from AWS.", innerException)
        {
            this.RequestId = requestId;
            this.LastKnownLocation = lastKnownLocation;
            this.ResponseBody = responseBody;
        }

        public AmazonUnmarshallingException(string requestId, string lastKnownLocation,
            string responseBody, string message, Exception innerException)
            : base("Error unmarshalling response back from AWS. " + message, innerException)
        {
            this.RequestId = requestId;
            this.LastKnownLocation = lastKnownLocation;
            this.ResponseBody = responseBody;
        }

        public AmazonUnmarshallingException(string requestId, string lastKnownLocation, Exception innerException, HttpStatusCode statusCode)
            : base("Error unmarshalling response back from AWS.", innerException, statusCode)
        {
            this.RequestId = requestId;
            this.LastKnownLocation = lastKnownLocation;
        }

        public AmazonUnmarshallingException(string requestId, string lastKnownLocation, string responseBody, Exception innerException, HttpStatusCode statusCode)
            : base("Error unmarshalling response back from AWS.", innerException, statusCode)
        {
            this.RequestId = requestId;
            this.LastKnownLocation = lastKnownLocation;
            this.ResponseBody = responseBody;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Last known location in the response that was parsed, if available.
        /// </summary>
        public string LastKnownLocation { get; private set; }

        /// <summary>
        /// The entire response body that caused this exception, if available.
        /// </summary>
        public string ResponseBody { get; private set; }

        #endregion

        #region Overrides

        public override string Message
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                AppendFormat(sb, "Request ID: {0}", this.RequestId);
                AppendFormat(sb, "Response Body: {0}", this.ResponseBody);
                AppendFormat(sb, "Last Parsed Path: {0}", this.LastKnownLocation);
                AppendFormat(sb, "HTTP Status Code: {0}", (int)(this.StatusCode) + " " + this.StatusCode.ToString());

                var partialMessage = sb.ToString();

                return base.Message + " " + partialMessage;
            }
        }

        #endregion

        #region Private methods

        private static void AppendFormat(StringBuilder sb, string format, string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            if (sb.Length > 0)
                sb.Append(", ");

            sb.AppendFormat(format, value);
        }

        #endregion

#if !NETSTANDARD
        /// <summary>
        /// Constructs a new instance of the AmazonSimpleDBException class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected AmazonUnmarshallingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.LastKnownLocation = info.GetString("LastKnownLocation");
                this.ResponseBody = info.GetString("ResponseBody");
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
                info.AddValue("LastKnownLocation", this.LastKnownLocation);
                info.AddValue("ResponseBody", this.ResponseBody);
            }
        }
#endif
    }
}
