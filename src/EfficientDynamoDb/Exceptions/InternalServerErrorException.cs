using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// An HTTP 5xx status code indicates a problem that must be resolved by AWS. This might be a transient error, in which case you can retry your request until it succeeds.<br/>
    /// OK to retry? Yes
    /// </summary>
    public class InternalServerErrorException : RetryableException
    {
        public InternalServerErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InternalServerErrorException(string message) : base(message)
        {
        }

        public InternalServerErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}