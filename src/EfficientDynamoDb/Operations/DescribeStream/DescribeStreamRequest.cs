namespace EfficientDynamoDb.Operations
{
    public class DescribeStreamRequest
    {
        /// <summary>
        /// The Amazon Resource Name (ARN) for the stream.
        /// <para>
        /// Required
        /// </para>
        /// </summary>
        public string StreamArn { get; set; } = null!;
        
        /// <summary>
        /// The shard ID of the first item that this operation will evaluate.
        /// Use the value that was returned for <see cref="StreamDescription.LastEvaluatedShardId"/> in the previous operation.
        /// </summary>
        public string? ExclusiveStartShardId { get; set; }
        
        /// <summary>
        /// The maximum number of shard objects to return.
        /// The upper limit is 100.
        /// </summary>
        public int Limit { get; set; }
    }
}