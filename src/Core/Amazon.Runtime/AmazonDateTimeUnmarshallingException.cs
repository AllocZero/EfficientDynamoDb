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

namespace Amazon.Runtime
{
    /// <summary>
    /// This exception is thrown when there is a parse error on the response back from AWS.
    /// </summary>
#if !NETSTANDARD
    [Serializable]
#endif
    public class AmazonDateTimeUnmarshallingException : AmazonUnmarshallingException
    {
        #region Public properties
        /// <summary>
        /// The string value which could not be converted into a valid DateTime instance.
        /// </summary>
        public string InvalidDateTimeToken { get; private set; }
        #endregion

        #region Constructors

        public AmazonDateTimeUnmarshallingException(string requestId, string lastKnownLocation, string invalidDateTimeToken,Exception innerException)
            : base(requestId, lastKnownLocation, innerException)
        {
            this.InvalidDateTimeToken = invalidDateTimeToken;
        }

        public AmazonDateTimeUnmarshallingException(string requestId, string lastKnownLocation, string responseBody, string invalidDateTimeToken,Exception innerException)
            : base(requestId, lastKnownLocation, responseBody, innerException)
        {
            this.InvalidDateTimeToken = invalidDateTimeToken;
        }

        public AmazonDateTimeUnmarshallingException(string requestId, string lastKnownLocation, 
            string responseBody, string invalidDateTimeToken,string message, Exception innerException)
            : base(requestId, lastKnownLocation, 
            responseBody, message, innerException)
        {
            this.InvalidDateTimeToken = invalidDateTimeToken;
        }

        #endregion

#if !NETSTANDARD
        /// <summary>
        /// Constructs a new instance of the AmazonDateTimeUnmarshallingException class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected AmazonDateTimeUnmarshallingException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.InvalidDateTimeToken = info.GetString("InvalidDateTimeToken");
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
                info.AddValue("InvalidDateTimeToken", this.InvalidDateTimeToken);
            }
        }
#endif
    }
}
