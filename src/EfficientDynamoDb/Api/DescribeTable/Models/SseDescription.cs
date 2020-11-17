using System;
using EfficientDynamoDb.Api.DescribeTable.Models.Enums;

namespace EfficientDynamoDb.Api.DescribeTable.Models
{
    public class SseDescription
    {
        public DateTime InaccessibleEncryptionDateTime { get; }
        
        public string KmsMasterKeyArn { get; }
        
        public SseType SSEType { get; }
        
        public SseStatus Status { get; }

        public SseDescription(DateTime inaccessibleEncryptionDateTime, string kmsMasterKeyArn, SseType sseType, SseStatus status)
        {
            InaccessibleEncryptionDateTime = inaccessibleEncryptionDateTime;
            KmsMasterKeyArn = kmsMasterKeyArn;
            SSEType = sseType;
            Status = status;
        }
    }
}