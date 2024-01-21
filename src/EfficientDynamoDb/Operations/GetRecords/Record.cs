using System.Text.Json.Serialization;

namespace EfficientDynamoDb.Operations
{
    /// <summary>
    /// A description of a unique event within a stream.
    /// </summary>
    public class Record
    {
        /// <summary>
        /// The region in which the GetRecords request was received.
        /// </summary>
        [JsonPropertyName("awsRegion")]
        public string AwsRegion { get; set; } = "";
        
        /// <summary>
        /// The main body of the stream record, containing all of the DynamoDB-specific fields.
        /// </summary>
        [JsonPropertyName("dynamodb")]        
        public StreamRecord DynamoDb { get; set; } = null!;
        
        /// <summary>
        /// A globally unique identifier for the event that was recorded in this stream record.
        /// </summary>
        [JsonPropertyName("eventID")]
        public string EventId { get; set; } = "";
        
        /// <summary>
        /// The type of data modification that was performed on the DynamoDB table.
        /// </summary>
        [JsonPropertyName("eventName")]
        public EventName EventName { get; set; }
        
        /// <summary>
        /// The AWS service from which the stream record originated. For DynamoDB Streams, this is "aws:dynamodb".
        /// </summary>
        [JsonPropertyName("eventSource")]
        public string EventSource { get; set; } = "";
        
        /// <summary>
        /// <para>
        /// The version number of the stream record format. This number is updated whenever the structure of <see cref="Record"/> is modified.
        /// </para>
        /// <para>
        /// Client applications must not assume that eventVersion will remain at a particular value, as this number is subject to change at any time.
        /// In general, eventVersion will only increase as the low-level DynamoDB Streams API evolves.
        /// </para>
        /// </summary>
        [JsonPropertyName("eventVersion")]
        public string EventVersion { get; set; } = "";
        
        /// <summary>
        /// Items that are deleted by the Time to Live process after expiration have the following fields:
        /// <list type="bullet">
        /// <item>
        /// <description><see cref="Operations.UserIdentity.Type"/> set to "Service"</description>
        /// </item>
        /// <item>
        /// <description><see cref="Operations.UserIdentity.PrincipalId"/> set to "dynamodb.amazonaws.com"</description>
        /// </item>
        /// </list>
        /// The <see cref="UserIdentity"/> field is null for all other records.
        /// </summary>
        [JsonPropertyName("userIdentity")]
        public UserIdentity? UserIdentity { get; set; }
    }
}