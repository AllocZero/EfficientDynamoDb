namespace EfficientDynamoDb.Operations
{
    public class StreamInfo
    {
        /// <summary>
        /// The Amazon Resource Name (ARN) for the stream.
        /// </summary>
        public string StreamArn { get; set; } = "";

        /// <summary>
        /// A timestamp, in ISO 8601 format, for this stream.
        /// </summary>
        public string StreamLabel { get; set; } = "";

        /// <summary>
        /// The DynamoDB table with which the stream is associated.
        /// </summary>
        public string TableName { get; set; } = "";
    }
}