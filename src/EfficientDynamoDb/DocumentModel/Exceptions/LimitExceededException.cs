using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.DocumentModel.Exceptions
{
    /// <summary>
    /// There are too many concurrent control plane operations. The cumulative number of tables and indexes in the CREATING, DELETING, or UPDATING state cannot exceed 50.<br/>
    /// OK to retry? Yes
    /// </summary>
    public class LimitExceededException : DdbException
    {
        public LimitExceededException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public LimitExceededException(string message) : base(message)
        {
        }

        public LimitExceededException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}