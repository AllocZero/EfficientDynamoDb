using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.Exceptions
{
    public class RetryableException : DdbException
    {

        public RetryableException(string message) : base(message)
        {
        }

        public RetryableException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}