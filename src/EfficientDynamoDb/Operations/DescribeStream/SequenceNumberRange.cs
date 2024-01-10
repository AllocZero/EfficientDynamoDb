namespace EfficientDynamoDb.Operations
{
    public class SequenceNumberRange
    {
        /// <summary>
        /// The first sequence number for the stream records contained within a shard.
        /// String contains numeric characters only.
        /// </summary>
        public string StartingSequenceNumber { get; }
        
        /// <summary>
        /// The last sequence number for the stream records contained within a shard.
        /// String contains numeric characters only.
        /// </summary>
        public string EndingSequenceNumber { get; }
        
        public SequenceNumberRange(string startingSequenceNumber, string endingSequenceNumber)
        {
            StartingSequenceNumber = startingSequenceNumber;
            EndingSequenceNumber = endingSequenceNumber;
        }
    }
}