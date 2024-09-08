using System;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// Throughput exceeds the current throughput limit for the account.<br/>
    /// OK to retry? Yes
    /// <example>Rate of on-demand requests exceeds the allowed account throughput.</example>
    /// </summary>
    public class RequestLimitExceededException : RetryableException
    {
        internal override OpErrorType OpErrorType => ErrorType;
        
        internal static OpErrorType ErrorType => OpErrorType.RequestLimitExceeded;
        
        public RequestLimitExceededException(string message) : base(message)
        {
        }

        public RequestLimitExceededException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}