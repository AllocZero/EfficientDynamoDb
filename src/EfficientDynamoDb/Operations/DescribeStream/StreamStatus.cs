namespace EfficientDynamoDb.Operations
{
    /// <summary>
    /// Indicates the current status of the stream
    /// </summary>
    public enum StreamStatus
    {
        Undefined = 0,
        /// <summary>
        /// Streams is currently being enabled on the DynamoDB table.
        /// </summary>
        Enabling = 10,
        /// <summary>
        /// The stream is enabled.
        /// </summary>
        Enabled = 20,
        /// <summary>
        /// Streams is currently being disabled on the DynamoDB table.
        /// </summary>
        Disabling = 30,
        /// <summary>
        /// The stream is disabled.
        /// </summary>
        Disabled = 40,
    }
}