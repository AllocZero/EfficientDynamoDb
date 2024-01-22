namespace EfficientDynamoDb.Operations
{
    public class Shard
    {
        /// <summary>
        /// The shard ID of the current shard's parent.
        /// </summary>
        public string ParentShardId { get; }
        
        /// <summary>
        /// The range of possible sequence numbers for the shard.
        /// </summary>
        public SequenceNumberRange SequenceNumberRange { get; }
        
        /// <summary>
        /// The system-generated identifier for this shard.
        /// </summary>
        public string ShardId { get; }
        
        public Shard(string parentShardId, SequenceNumberRange sequenceNumberRange, string shardId)
        {
            ParentShardId = parentShardId;
            SequenceNumberRange = sequenceNumberRange;
            ShardId = shardId;
        }
    }
}