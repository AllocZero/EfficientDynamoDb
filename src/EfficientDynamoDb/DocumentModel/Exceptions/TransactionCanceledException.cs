using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using EfficientDynamoDb.Context.Operations.Shared;

namespace EfficientDynamoDb.DocumentModel.Exceptions
{
    // TODO: Review all cases https://docs.aws.amazon.com/amazondynamodb/latest/APIReference/API_TransactWriteItems.html
    /// <summary>
    /// The entire transaction request was canceled.<br/><br/>
    /// <list type="bullet">
    /// <listheader>
    /// DynamoDB cancels a TransactWriteItems request under the following circumstances:
    /// </listheader>
    ///     <item>
    ///         <term>A condition in one of the condition expressions is not met.</term>
    ///     </item>
    ///     <item>
    ///         <term>A table in the <c>TransactWriteItems</c> request is in a different account or region.</term>
    ///     </item>
    ///     <item>
    ///         <term>More than one action in the <c>TransactWriteItems</c> operation targets the same item.</term>
    ///     </item>
    ///     <item>
    ///         <term>There is insufficient provisioned capacity for the transaction to be completed.</term>
    ///     </item>
    ///     <item>
    ///         <term>An item size becomes too large (larger than 400 KB), or a local secondary index (LSI) becomes too large, or a similar validation error occurs because of changes made by the transaction.</term>
    ///     </item>
    ///     <item>
    ///         <term>There is a user error, such as an invalid data format.</term>
    ///     </item>
    /// </list>
    ///
    /// <list type="bullet">
    /// <listheader>
    /// DynamoDB cancels a <c>TransactGetItems</c> request under the following circumstances:
    /// </listheader>
    ///     <item>
    ///         <term>There is an ongoing <c>TransactGetItems</c> operation that conflicts with a concurrent <c>PutItem</c>, <c>UpdateItem</c>, <c>DeleteItem</c> or <c>TransactWriteItems</c> request. In this case the <c>TransactGetItems</c> operation fails with a <see cref="TransactionCanceledException"/>.</term>
    ///     </item>
    ///     <item>
    ///         <term>A table in the <c>TransactGetItems</c> request is in a different account or region.</term>
    ///     </item>
    ///     <item>
    ///         <term>There is insufficient provisioned capacity for the transaction to be completed.</term>
    ///     </item>
    ///     <item>
    ///         <term>There is a user error, such as an invalid data format.</term>
    ///     </item>
    /// </list>
    /// </summary>
    public class TransactionCanceledException : DdbException
    {
        public IReadOnlyList<TransactionCancellationReason> CancellationReasons { get; }

        public TransactionCanceledException(IReadOnlyList<TransactionCancellationReason> cancellationReasons, string message) : base(message)
        {
            CancellationReasons = cancellationReasons;
        }

        public TransactionCanceledException(IReadOnlyList<TransactionCancellationReason> cancellationReasons, string message, Exception innerException) : base(message, innerException)
        {
            CancellationReasons = cancellationReasons;
        }
    }
}