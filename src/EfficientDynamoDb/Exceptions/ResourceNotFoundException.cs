using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// Requested resource not found.<br/>
    /// OK to retry? No
    /// <example>The table that is being requested does not exist, or is too early in the CREATING state.</example>
    /// </summary>
    public class ResourceNotFoundException : DdbException
    {

        public ResourceNotFoundException(string message) : base(message)
        {
        }

        public ResourceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}