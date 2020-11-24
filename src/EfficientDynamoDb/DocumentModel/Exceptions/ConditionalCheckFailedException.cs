using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.DocumentModel.Exceptions
{
    /// <summary>
    /// You specified a condition that evaluated to false. For example, you might have tried to perform a conditional update on an item, but the actual value of the attribute did not match the expected value in the condition.<br/>
    /// OK to retry? No
    /// </summary>
    public class ConditionalCheckFailedException : DdbException
    {
        public ConditionalCheckFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ConditionalCheckFailedException(string message) : base(message)
        {
        }

        public ConditionalCheckFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}