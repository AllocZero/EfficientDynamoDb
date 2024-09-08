using System;
using System.Runtime.Serialization;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// The request signature did not include all of the required components.<br/>
    /// OK to retry? No
    /// </summary>
    public class IncompleteSignatureException : DdbException
    {
        internal override OpErrorType OpErrorType => ErrorType;
        
        internal static OpErrorType ErrorType => OpErrorType.IncompleteSignature;
        
        public IncompleteSignatureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public IncompleteSignatureException(string message) : base(message)
        {
        }

        public IncompleteSignatureException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}