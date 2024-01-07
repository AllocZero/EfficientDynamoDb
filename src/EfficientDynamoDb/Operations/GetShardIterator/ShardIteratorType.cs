namespace EfficientDynamoDb.Operations
{
    /// <summary>
    /// Determines how the shard iterator is used to start reading stream records from the shard.
    /// </summary>
    public enum ShardIteratorType
    {
        Undefined = 0,
        /// <summary>
        /// Start reading exactly from the position denoted by a specific sequence number
        /// </summary>
        AtSequenceNumber = 10,
        /// <summary>
        /// Start reading right after the position denoted by a specific sequence number
        /// </summary>
        AfterSequenceNumber = 20,
        /// <summary>
        /// Start reading at the last (untrimmed) stream record, which is the oldest record in the shard.
        /// In DynamoDB Streams, there is a 24 hour limit on data retention.
        /// Stream records whose age exceeds this limit are subject to removal (trimming) from the stream.
        /// </summary>
        TrimHorizon = 30,
        /// <summary>
        /// Start reading just after the most recent stream record in the shard, so that you always read the most recent data in the shard.
        /// </summary>
        Latest = 40
    }
}