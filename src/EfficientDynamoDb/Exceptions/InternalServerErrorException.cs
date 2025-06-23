using System;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// An HTTP 5xx status code indicates a problem that must be resolved by AWS. This might be a transient error, in which case you can retry your request until it succeeds.<br/>
    /// OK to retry? Yes
    /// </summary>
    public class InternalServerErrorException : RetryableException
    {
        internal override OpErrorType OpErrorType => ErrorType;
        
        internal static OpErrorType ErrorType => OpErrorType.InternalServerError;
        
        public InternalServerErrorException(string message) : base(message)
        {
        }

        public InternalServerErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}