using System;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// The client did not correctly sign the request.<br/>
    /// OK to retry? No
    /// </summary>
    public class AccessDeniedException : DdbException
    {
        internal override OpErrorType OpErrorType => ErrorType;
        
        internal static OpErrorType ErrorType => OpErrorType.AccessDenied;

        public AccessDeniedException(string message) : base(message)
        {
        }

        public AccessDeniedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}