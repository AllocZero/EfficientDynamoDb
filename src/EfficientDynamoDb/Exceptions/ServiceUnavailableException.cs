using System;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// DynamoDB is currently unavailable. (This should be a temporary state.)<br/>
    /// OK to retry? Yes
    /// </summary>
    public class ServiceUnavailableException : RetryableException
    {
        internal override OpErrorType OpErrorType => ErrorType;
        
        internal static OpErrorType ErrorType => OpErrorType.ServiceUnavailable;
        
        public ServiceUnavailableException(string message) : base(message)
        {
        }

        public ServiceUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}