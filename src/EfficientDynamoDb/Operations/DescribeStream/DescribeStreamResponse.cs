namespace EfficientDynamoDb.Operations
{
    public class DescribeStreamResponse
    {
        /// <summary>
        /// A complete description of the stream, including its creation date and time, the DynamoDB table associated with the stream,
        /// the shard IDs within the stream, and the beginning and ending sequence numbers of stream records within the shards.
        /// </summary>
        public StreamDescription StreamDescription { get; }
        
        public DescribeStreamResponse(StreamDescription streamDescription)
        {
            StreamDescription = streamDescription;
        }
    }
}