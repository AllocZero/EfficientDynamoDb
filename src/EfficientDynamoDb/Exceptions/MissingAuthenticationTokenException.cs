using System;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// The request did not include the required authorization header, or it was malformed.<br/>
    /// OK to retry? No
    /// </summary>
    public class MissingAuthenticationTokenException : DdbException
    {
        internal override OpErrorType OpErrorType => ErrorType;
        
        internal static OpErrorType ErrorType => OpErrorType.MissingAuthenticationToken;
        
        public MissingAuthenticationTokenException(string message) : base(message)
        {
        }

        public MissingAuthenticationTokenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}