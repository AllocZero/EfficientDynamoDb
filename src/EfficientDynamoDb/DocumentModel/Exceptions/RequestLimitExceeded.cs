using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.DocumentModel.Exceptions
{
    /// <summary>
    /// Throughput exceeds the current throughput limit for the account.<br/>
    /// OK to retry? Yes
    /// <example>Rate of on-demand requests exceeds the allowed account throughput.</example>
    /// </summary>
    public class RequestLimitExceeded : RetryableException
    {
        public RequestLimitExceeded(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public RequestLimitExceeded(string message) : base(message)
        {
        }

        public RequestLimitExceeded(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}