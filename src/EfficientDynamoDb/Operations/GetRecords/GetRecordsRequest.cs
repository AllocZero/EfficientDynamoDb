namespace EfficientDynamoDb.Operations
{
    public class GetRecordsRequest
    {
        /// <summary>
        /// The maximum number of records to return from the shard. The upper limit is 1000.
        /// </summary>
        public int Limit { get; set; } = -1;

        /// <summary>
        /// <para>
        /// A shard iterator that was retrieved from previous <see cref="GetShardIteratorResponse"/> or <see cref="GetRecordsResponse"/>.
        /// This iterator can be used to access the stream records in this shard.
        /// </para>
        /// <para>
        /// Required
        /// </para>
        /// </summary>
        public string ShardIterator { get; set; } = null!;
    }
}