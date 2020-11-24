using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.DocumentModel.Exceptions
{
    /// <summary>
    /// The client did not correctly sign the request.<br/>
    /// OK to retry? No
    /// </summary>
    public class AccessDeniedException : DdbException
    {
        public AccessDeniedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public AccessDeniedException(string message) : base(message)
        {
        }

        public AccessDeniedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}