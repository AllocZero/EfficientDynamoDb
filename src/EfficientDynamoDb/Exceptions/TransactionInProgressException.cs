using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// The transaction with the given request token is already in progress.
    /// </summary>
    public class TransactionInProgressException : DdbException
    {

        public TransactionInProgressException(string message) : base(message)
        {
        }

        public TransactionInProgressException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}