using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.DocumentModel.Exceptions
{
    /// <summary>
    /// The resource which you are attempting to change is in use.<br/>
    /// OK to retry? No
    /// <example>You tried to re-create an existing table, or delete a table currently in the CREATING state.</example>
    /// </summary>
    public class ResourceInUseException : DdbException
    {
        public ResourceInUseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ResourceInUseException(string message) : base(message)
        {
        }

        public ResourceInUseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}