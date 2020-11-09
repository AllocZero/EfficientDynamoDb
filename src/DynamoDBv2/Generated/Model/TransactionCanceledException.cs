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

/*
 * Do not modify this file. This file is generated from the dynamodb-2012-08-10.normal.json service model.
 */
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Net;

using Amazon.Runtime;
using Amazon.Runtime.Internal;

namespace Amazon.DynamoDBv2.Model
{
    /// <summary>
    /// The entire transaction request was canceled.
    /// 
    ///  
    /// <para>
    /// DynamoDB cancels a <code>TransactWriteItems</code> request under the following circumstances:
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// A condition in one of the condition expressions is not met.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// A table in the <code>TransactWriteItems</code> request is in a different account or
    /// region.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// More than one action in the <code>TransactWriteItems</code> operation targets the
    /// same item.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// There is insufficient provisioned capacity for the transaction to be completed.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// An item size becomes too large (larger than 400 KB), or a local secondary index (LSI)
    /// becomes too large, or a similar validation error occurs because of changes made by
    /// the transaction.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// There is a user error, such as an invalid data format.
    /// </para>
    ///  </li> </ul> 
    /// <para>
    /// DynamoDB cancels a <code>TransactGetItems</code> request under the following circumstances:
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// There is an ongoing <code>TransactGetItems</code> operation that conflicts with a
    /// concurrent <code>PutItem</code>, <code>UpdateItem</code>, <code>DeleteItem</code>
    /// or <code>TransactWriteItems</code> request. In this case the <code>TransactGetItems</code>
    /// operation fails with a <code>TransactionCanceledException</code>.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// A table in the <code>TransactGetItems</code> request is in a different account or
    /// region.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// There is insufficient provisioned capacity for the transaction to be completed.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// There is a user error, such as an invalid data format.
    /// </para>
    ///  </li> </ul> <note> 
    /// <para>
    /// If using Java, DynamoDB lists the cancellation reasons on the <code>CancellationReasons</code>
    /// property. This property is not set for other languages. Transaction cancellation reasons
    /// are ordered in the order of requested items, if an item has no error it will have
    /// <code>NONE</code> code and <code>Null</code> message.
    /// </para>
    ///  </note> 
    /// <para>
    /// Cancellation reason codes and possible error messages:
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// No Errors:
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// Code: <code>NONE</code> 
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Message: <code>null</code> 
    /// </para>
    ///  </li> </ul> </li> <li> 
    /// <para>
    /// Conditional Check Failed:
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// Code: <code>ConditionalCheckFailed</code> 
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Message: The conditional request failed. 
    /// </para>
    ///  </li> </ul> </li> <li> 
    /// <para>
    /// Item Collection Size Limit Exceeded:
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// Code: <code>ItemCollectionSizeLimitExceeded</code> 
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Message: Collection size exceeded.
    /// </para>
    ///  </li> </ul> </li> <li> 
    /// <para>
    /// Transaction Conflict:
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// Code: <code>TransactionConflict</code> 
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Message: Transaction is ongoing for the item.
    /// </para>
    ///  </li> </ul> </li> <li> 
    /// <para>
    /// Provisioned Throughput Exceeded:
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// Code: <code>ProvisionedThroughputExceeded</code> 
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Messages: 
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// The level of configured provisioned throughput for the table was exceeded. Consider
    /// increasing your provisioning level with the UpdateTable API.
    /// </para>
    ///  <note> 
    /// <para>
    /// This Message is received when provisioned throughput is exceeded is on a provisioned
    /// DynamoDB table.
    /// </para>
    ///  </note> </li> <li> 
    /// <para>
    /// The level of configured provisioned throughput for one or more global secondary indexes
    /// of the table was exceeded. Consider increasing your provisioning level for the under-provisioned
    /// global secondary indexes with the UpdateTable API.
    /// </para>
    ///  <note> 
    /// <para>
    /// This message is returned when provisioned throughput is exceeded is on a provisioned
    /// GSI.
    /// </para>
    ///  </note> </li> </ul> </li> </ul> </li> <li> 
    /// <para>
    /// Throttling Error:
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// Code: <code>ThrottlingError</code> 
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Messages: 
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// Throughput exceeds the current capacity of your table or index. DynamoDB is automatically
    /// scaling your table or index so please try again shortly. If exceptions persist, check
    /// if you have a hot key: https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/bp-partition-key-design.html.
    /// </para>
    ///  <note> 
    /// <para>
    /// This message is returned when writes get throttled on an On-Demand table as DynamoDB
    /// is automatically scaling the table.
    /// </para>
    ///  </note> </li> <li> 
    /// <para>
    /// Throughput exceeds the current capacity for one or more global secondary indexes.
    /// DynamoDB is automatically scaling your index so please try again shortly.
    /// </para>
    ///  <note> 
    /// <para>
    /// This message is returned when when writes get throttled on an On-Demand GSI as DynamoDB
    /// is automatically scaling the GSI.
    /// </para>
    ///  </note> </li> </ul> </li> </ul> </li> <li> 
    /// <para>
    /// Validation Error:
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// Code: <code>ValidationError</code> 
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Messages: 
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// One or more parameter values were invalid.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// The update expression attempted to update the secondary index key beyond allowed size
    /// limits.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// The update expression attempted to update the secondary index key to unsupported type.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// An operand in the update expression has an incorrect data type.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Item size to update has exceeded the maximum allowed size.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Number overflow. Attempting to store a number with magnitude larger than supported
    /// range.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Type mismatch for attribute to update.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Nesting Levels have exceeded supported limits.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// The document path provided in the update expression is invalid for update.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// The provided expression refers to an attribute that does not exist in the item.
    /// </para>
    ///  </li> </ul> </li> </ul> </li> </ul>
    /// </summary>
    #if !NETSTANDARD
    [Serializable]
    #endif
    public partial class TransactionCanceledException : AmazonDynamoDBException
    {
        private List<CancellationReason> _cancellationReasons = new List<CancellationReason>();

        /// <summary>
        /// Constructs a new TransactionCanceledException with the specified error
        /// message.
        /// </summary>
        /// <param name="message">
        /// Describes the error encountered.
        /// </param>
        public TransactionCanceledException(string message) 
            : base(message) {}

        /// <summary>
        /// Construct instance of TransactionCanceledException
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public TransactionCanceledException(string message, Exception innerException) 
            : base(message, innerException) {}

        /// <summary>
        /// Construct instance of TransactionCanceledException
        /// </summary>
        /// <param name="innerException"></param>
        public TransactionCanceledException(Exception innerException) 
            : base(innerException) {}

        /// <summary>
        /// Construct instance of TransactionCanceledException
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="errorType"></param>
        /// <param name="errorCode"></param>
        /// <param name="requestId"></param>
        /// <param name="statusCode"></param>
        public TransactionCanceledException(string message, Exception innerException, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode) 
            : base(message, innerException, errorType, errorCode, requestId, statusCode) {}

        /// <summary>
        /// Construct instance of TransactionCanceledException
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorType"></param>
        /// <param name="errorCode"></param>
        /// <param name="requestId"></param>
        /// <param name="statusCode"></param>
        public TransactionCanceledException(string message, ErrorType errorType, string errorCode, string requestId, HttpStatusCode statusCode) 
            : base(message, errorType, errorCode, requestId, statusCode) {}


#if !NETSTANDARD
        /// <summary>
        /// Constructs a new instance of the TransactionCanceledException class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected TransactionCanceledException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            this.CancellationReasons = (List<CancellationReason>)info.GetValue("CancellationReasons", typeof(List<CancellationReason>));
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
            info.AddValue("CancellationReasons", this.CancellationReasons);
        }
#endif

        /// <summary>
        /// Gets and sets the property CancellationReasons. 
        /// <para>
        /// A list of cancellation reasons.
        /// </para>
        /// </summary>
        [AWSProperty(Min=1, Max=25)]
        public List<CancellationReason> CancellationReasons
        {
            get { return this._cancellationReasons; }
            set { this._cancellationReasons = value; }
        }

        // Check to see if CancellationReasons property is set
        internal bool IsSetCancellationReasons()
        {
            return this._cancellationReasons != null && this._cancellationReasons.Count > 0; 
        }

    }
}