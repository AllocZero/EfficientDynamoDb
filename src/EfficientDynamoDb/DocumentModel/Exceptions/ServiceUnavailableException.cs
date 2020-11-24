using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.DocumentModel.Exceptions
{
    /// <summary>
    /// DynamoDB is currently unavailable. (This should be a temporary state.)<br/>
    /// OK to retry? Yes
    /// </summary>
    public class ServiceUnavailableException : DdbException
    {
        public ServiceUnavailableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ServiceUnavailableException(string message) : base(message)
        {
        }

        public ServiceUnavailableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}