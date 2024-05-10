using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// This exception is returned as an AmazonServiceException response with a THROTTLING_EXCEPTION status code. This exception might be returned if you perform control plane API operations too rapidly.<br/>
    /// For tables using on-demand mode, this exception might be returned for any data plane API operation if your request rate is too high.<br/>
    /// OK to retry? Yes
    /// </summary>
    public class ThrottlingException : RetryableException
    {

        public ThrottlingException(string message) : base(message)
        {
        }

        public ThrottlingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}