using System;
using System.Collections.Generic;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems
{
    public class TransactWriteItemsRequest
    {
        /// <summary>
        /// An ordered array of up to 25 <see cref="TransactWriteItem"/> objects, each of which contains a <see cref="ConditionCheck"/>, <see cref="TransactPutItem"/>, <see cref="TransactUpdateItem"/>, or <see cref="TransactDeleteItem"/> object. These can operate on items in different tables, but the tables must reside in the same AWS account and Region, and no two of them can operate on the same item.
        /// </summary>
        public IReadOnlyCollection<TransactWriteItem> TransactItems { get; set; } = Array.Empty<TransactWriteItem>();
        
        /// <summary>
        /// Providing a <see cref="ClientRequestToken"/> makes the call to <c>TransactWriteItems</c> idempotent, meaning that multiple identical calls have the same effect as one single call.<br/><br/>
        /// Although multiple identical calls using the same client request token produce the same result on the server (no side effects), the responses to the calls might not be the same. If the <see cref="ReturnConsumedCapacity"/> parameter is set, then the initial <c>TransactWriteItems</c> call returns the amount of write capacity units consumed in making the changes. Subsequent <c>TransactWriteItems</c> calls with the same client token return the number of read capacity units consumed in reading the item.<br/><br/>
        /// A client request token is valid for 10 minutes after the first request that uses it is completed. After 10 minutes, any request with the same client token is treated as a new request. Do not resubmit the same request with the same client token for more than 10 minutes, or the result might not be idempotent.<br/><br/>
        /// If you submit a request with the same client token but a change in other parameters within the 10-minute idempotency window, DynamoDB returns an <see cref="IdempotentParameterMismatchException"/> exception.
        /// 
        /// </summary>
        public string? ClientRequestToken { get; set; }
        
        /// <summary>
        /// Determines the level of detail about provisioned throughput consumption that is returned in the response.
        /// </summary>
        public ReturnConsumedCapacity ReturnConsumedCapacity { get; set; }
        
        /// <summary>
        /// Determines whether item collection metrics are returned.
        /// </summary>
        public ReturnItemCollectionMetrics ReturnItemCollectionMetrics { get; set; }
    }
}