using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// The request did not include the required authorization header, or it was malformed.<br/>
    /// OK to retry? No
    /// </summary>
    public class MissingAuthenticationTokenException : DdbException
    {

        public MissingAuthenticationTokenException(string message) : base(message)
        {
        }

        public MissingAuthenticationTokenException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}