using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// The request signature is incorrect. The most likely cause is an invalid AWS access key ID or secret key.<br/>
    /// OK to retry? Yes
    /// </summary>
    public class UnrecognizedClientException : DdbException
    {

        public UnrecognizedClientException(string message) : base(message)
        {
        }

        public UnrecognizedClientException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}