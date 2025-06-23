using System;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// The transaction with the given request token is already in progress.
    /// </summary>
    public class TransactionInProgressException : DdbException
    {
        internal override OpErrorType OpErrorType => ErrorType;
        
        internal static OpErrorType ErrorType => OpErrorType.TransactionInProgress;
        
        public TransactionInProgressException(string message) : base(message)
        {
        }

        public TransactionInProgressException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}