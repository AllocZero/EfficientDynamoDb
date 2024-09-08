using System;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// There are too many concurrent control plane operations. The cumulative number of tables and indexes in the CREATING, DELETING, or UPDATING state cannot exceed 50.<br/>
    /// OK to retry? Yes
    /// </summary>
    public class LimitExceededException : RetryableException
    {
        internal override OpErrorType OpErrorType => ErrorType;
        
        internal static OpErrorType ErrorType => OpErrorType.LimitExceeded;
        
        public LimitExceededException(string message) : base(message)
        {
        }

        public LimitExceededException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}