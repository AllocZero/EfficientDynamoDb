using System;

namespace EfficientDynamoDb.Context.Operations.DescribeTable.Models
{
    public class RestoreSummary
    {
        public DateTime RestoreDateTime { get; }
        
        public bool RestoreInProgress { get; }
        
        public string SourceBackupArn { get; }
        
        public string SourceTableArn { get; }

        public RestoreSummary(DateTime restoreDateTime, bool restoreInProgress, string sourceBackupArn, string sourceTableArn)
        {
            RestoreDateTime = restoreDateTime;
            RestoreInProgress = restoreInProgress;
            SourceBackupArn = sourceBackupArn;
            SourceTableArn = sourceTableArn;
        }
    }
}