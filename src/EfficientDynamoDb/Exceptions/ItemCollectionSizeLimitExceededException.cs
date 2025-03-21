using System;
using System.Runtime.Serialization;

namespace EfficientDynamoDb.Exceptions
{
    /// <summary>
    /// For a table with a local secondary index, a group of items with the same partition key value has exceeded the maximum size limit of 10 GB. For more information on item collections, see Item Collections.<br/>
    /// OK to retry? Yes
    /// </summary>
    public class ItemCollectionSizeLimitExceededException : DdbException
    {
        public ItemCollectionSizeLimitExceededException(string message) : base(message)
        {
        }

        public ItemCollectionSizeLimitExceededException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}