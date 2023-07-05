using System;
using System.Runtime.Serialization;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// You specified a condition that evaluated to false. For example, you might have tried to perform a conditional update on an item, but the actual value of the attribute did not match the expected value in the condition.<br/>
    /// OK to retry? No
    /// </summary>
    public class ConditionalCheckFailedException : DdbException
    {
        public Document? Item { get; }
        
        public ConditionalCheckFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public ConditionalCheckFailedException(Document? item, string message) : base(message)
        {
            Item = item;
        }
        
        public ConditionalCheckFailedException(Document? item, string message, Exception innerException) : base(message, innerException)
        {
            Item = item;
        }
        
        [Obsolete("This constructor is obsolete and will be removed in next major version. Use constructor with `Document? item` parameter instead.")]
        public ConditionalCheckFailedException(string message) : base(message)
        {
        }

        [Obsolete("This constructor is obsolete and will be removed in next major version. Use constructor with `Document? item` parameter instead.")]
        public ConditionalCheckFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}