using System;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// Requested resource not found.<br/>
    /// OK to retry? No
    /// <example>The table that is being requested does not exist, or is too early in the CREATING state.</example>
    /// </summary>
    public class ResourceNotFoundException : DdbException
    {
        internal override OpErrorType OpErrorType => ErrorType;
        
        internal static OpErrorType ErrorType => OpErrorType.ResourceNotFound;
        
        public ResourceNotFoundException(string message) : base(message)
        {
        }

        public ResourceNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}