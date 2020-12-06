using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.DocumentModel.Exceptions
{
    public class RetryableException : DdbException
    {
        public RetryableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public RetryableException(string message) : base(message)
        {
        }

        public RetryableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}