using System;
using System.Collections.Generic;
using EfficientDynamoDb.Operations.DescribeTable.Models;
using EfficientDynamoDb.Operations.DescribeTable.Models.Enums;

namespace EfficientDynamoDb.Operations
{
    public class StreamDescription
    {
        /// <summary>
        /// The date and time when the request to create this stream was issued.
        /// </summary>
        public DateTime CreationRequestDateTime { get; set; }

        /// <summary>
        /// The key attribute(s) of the stream's DynamoDB table.
        /// </summary>
        public IReadOnlyList<KeySchemaElement> KeySchema { get; set; } = Array.Empty<KeySchemaElement>();

        /// <summary>
        /// <para>
        /// The shard ID of the item where the operation stopped, inclusive of the previous result set.
        /// Use this value to start a new operation, excluding this value in the new request.
        /// </para>
        /// <para>
        /// If <see cref="LastEvaluatedShardId"/> is empty, then the "last page" of results has been processed and there is currently no more data to be retrieved.
        /// </para>
        /// <para>
        /// If <see cref="LastEvaluatedShardId"/> is not empty, it does not necessarily mean that there is more data in the result set.
        /// The only way to know when you have reached the end of the result set is when LastEvaluatedShardId is empty.
        /// </para>
        /// </summary>
        public string? LastEvaluatedShardId { get; set; }

        /// <summary>
        /// The shards that comprise the stream.
        /// </summary>
        public IReadOnlyList<Shard> Shards { get; set; } = Array.Empty<Shard>();
        
        /// <summary>
        /// The Amazon Resource Name (ARN) for the stream.
        /// </summary>
        public string StreamArn { get; set; } = null!;
        
        /// <summary>
        /// A timestamp, in ISO 8601 format, for this stream.
        /// </summary>
        public string StreamLabel { get; set; } = null!;
        
        /// <summary>
        /// Indicates the current status of the stream.
        /// </summary>
        public StreamStatus StreamStatus { get; set; }
        
        /// <summary>
        /// Indicates the format of the records within this stream.
        /// </summary>
        public StreamViewType StreamViewType { get; set; }
        
        /// <summary>
        /// The DynamoDB table with which the stream is associated.
        /// </summary>
        public string TableName { get; set; } = null!;
    }
}