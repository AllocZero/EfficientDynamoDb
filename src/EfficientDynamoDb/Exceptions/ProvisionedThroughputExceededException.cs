using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// Maximum allowed provisioned throughput for a table or for one or more global secondary indexes was exceeded.<br/>
    /// OK to retry? Yes
    /// </summary>
    public class ProvisionedThroughputExceededException : RetryableException
    {
        public ProvisionedThroughputExceededException(string message) : base(message)
        {
        }

        public ProvisionedThroughputExceededException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}