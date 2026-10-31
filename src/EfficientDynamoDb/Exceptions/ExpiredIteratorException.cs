using System;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// The shard iterator has expired. An iterator is valid for 15 minutes after it was returned by GetShardIterator.<br/>
    /// OK to retry? No. Request a new shard iterator instead.
    /// </summary>
    public class ExpiredIteratorException : DdbException
    {
        internal override OpErrorType OpErrorType => ErrorType;
        
        internal static OpErrorType ErrorType => OpErrorType.ExpiredIterator;
        
        public ExpiredIteratorException(string message) : base(message)
        {
        }

        public ExpiredIteratorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}