using System;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// The request signature is incorrect. The most likely cause is an invalid AWS access key ID or secret key.<br/>
    /// OK to retry? Yes
    /// </summary>
    public class UnrecognizedClientException : DdbException
    {
        internal override OpErrorType OpErrorType => ErrorType;
        
        internal static OpErrorType ErrorType => OpErrorType.UnrecognizedClient;
        
        public UnrecognizedClientException(string message) : base(message)
        {
        }

        public UnrecognizedClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}