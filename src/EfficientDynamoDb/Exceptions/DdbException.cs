using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// A base class for all DynamoDb exceptions.
    /// </summary>
    public class DdbException : Exception
    {
        public DdbException(string message) : base(message)
        {
        }

        public DdbException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}