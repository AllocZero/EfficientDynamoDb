using System;

namespace EfficientDynamoDb.Context.Operations.DescribeTable.Models
{
    public class ArchivalSummary
    {
        public string ArchivalBackupArn { get; }
        
        public DateTime ArchivalDateTime { get; }
        
        public string ArchivalReason { get; }

        public ArchivalSummary(string archivalBackupArn, DateTime archivalDateTime, string archivalReason)
        {
            ArchivalBackupArn = archivalBackupArn;
            ArchivalDateTime = archivalDateTime;
            ArchivalReason = archivalReason;
        }
    }
}