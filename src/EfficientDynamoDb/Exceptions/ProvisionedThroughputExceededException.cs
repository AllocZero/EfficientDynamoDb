using System;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// Maximum allowed provisioned throughput for a table or for one or more global secondary indexes was exceeded.<br/>
    /// OK to retry? Yes
    /// </summary>
    public class ProvisionedThroughputExceededException : RetryableException
    {
        internal override OpErrorType OpErrorType => ErrorType;
        
        internal static OpErrorType ErrorType => OpErrorType.ProvisionedThroughputExceeded;
        
        public ProvisionedThroughputExceededException(string message) : base(message)
        {
        }

        public ProvisionedThroughputExceededException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}