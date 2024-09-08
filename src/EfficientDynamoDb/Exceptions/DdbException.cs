using System;
using System.Runtime.Serialization;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// A base class for all DynamoDb exceptions.
    /// </summary>
    public class DdbException : Exception
    {
        public DdbException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DdbException(string message) : base(message)
        {
        }

        public DdbException(string message, Exception innerException) : base(message, innerException)
        {
        }

        internal virtual OpErrorType OpErrorType => OpErrorType.Unknown;
    }
}