namespace EfficientDynamoDb.Operations
{
    public class GetShardIteratorRequest
    {
        /// <summary>
        /// <para>
        /// The identifier of the shard.
        /// The iterator will be returned for this shard ID.
        /// </para>
        /// <para>
        /// Required
        /// </para>
        /// </summary>
        public string ShardId { get; set; } = null!;

        /// <summary>
        /// The sequence number of a stream record in the shard from which to start reading.
        /// </summary>
        public string? SequenceNumber { get; set; }

        /// <summary>
        /// The Amazon Resource Name (ARN) for the stream.
        /// <para>
        /// Required
        /// </para>
        /// </summary>
        public string StreamArn { get; set; } = null!;
        
        /// <summary>
        /// Determines how the shard iterator is used to start reading stream records from the shard.
        /// <para>
        /// Required
        /// </para>
        /// </summary>
        public ShardIteratorType ShardIteratorType { get; set; }
    }
}