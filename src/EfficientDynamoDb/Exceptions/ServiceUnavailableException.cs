using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// DynamoDB is currently unavailable. (This should be a temporary state.)<br/>
    /// OK to retry? Yes
    /// </summary>
    public class ServiceUnavailableException : RetryableException
    {
        public ServiceUnavailableException(string message) : base(message)
        {
        }

        public ServiceUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}