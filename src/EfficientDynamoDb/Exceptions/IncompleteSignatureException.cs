using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// The request signature did not include all of the required components.<br/>
    /// OK to retry? No
    /// </summary>
    public class IncompleteSignatureException : DdbException
    {

        public IncompleteSignatureException(string message) : base(message)
        {
        }

        public IncompleteSignatureException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}