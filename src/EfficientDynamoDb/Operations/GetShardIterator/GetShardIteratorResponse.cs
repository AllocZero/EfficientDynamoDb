namespace EfficientDynamoDb.Operations
{
    public class GetShardIteratorResponse
    {
        /// <summary>
        /// The position in the shard from which to start reading stream records sequentially.
        /// A shard iterator specifies this position using the sequence number of a stream record in a shard.
        /// </summary>
        public string ShardIterator { get; set; } = null!;
    }
}