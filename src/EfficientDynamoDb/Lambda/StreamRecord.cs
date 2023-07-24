using System;
using System.Collections.Generic;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Lambda
{
    public class StreamRecord 
    {
        public DateTime? ApproximateCreationDateTime { get; set; }

        [DynamoDbProperty("SequenceNumber")]
        public string SequenceNumber { get; set; } = null!;
        
        public long SizeBytes { get; set; }

        [DynamoDbProperty("StreamViewType")]
        public string StreamViewType { get; set; } = null!;
        
        [DynamoDbProperty("NewImage")]
        public Document? NewImage { get; set; }
        
        [DynamoDbProperty("OldImage")]
        public Document? OldImage { get; set; }

        [DynamoDbProperty("Keys")]
        public Document Keys { get; set; } = null!;
    }
}