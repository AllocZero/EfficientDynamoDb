using System;
using System.Collections.Generic;
using EfficientDynamoDb.Operations.DescribeTable.Models.Enums;
using EfficientDynamoDb.Operations.DescribeTable.Models.Indexes;

namespace EfficientDynamoDb.Operations.DescribeTable.Models
{
    public class ReplicaDescription
    {
        public IReadOnlyList<ReplicaGlobalSecondaryIndexDescription> GlobalSecondaryIndexes { get; }
        
        public string KMSMasterKeyId { get; }
        
        public ProvisionedThroughputOverride ProvisionedThroughputOverride { get; }
        
        public string RegionName { get; }
        
        public DateTime ReplicaInaccessibleDateTime { get; }
        
        public ReplicaStatus ReplicaStatus { get; }
        
        public string ReplicaStatusDescription { get; }
        
        public string ReplicaStatusProgress { get; }

        public ReplicaDescription(IReadOnlyList<ReplicaGlobalSecondaryIndexDescription> globalSecondaryIndexes, string kmsMasterKeyId, ProvisionedThroughputOverride provisionedThroughputOverride, string regionName, DateTime replicaInaccessibleDateTime, ReplicaStatus replicaStatus, string replicaStatusDescription, string replicaStatusProgress)
        {
            GlobalSecondaryIndexes = globalSecondaryIndexes;
            KMSMasterKeyId = kmsMasterKeyId;
            ProvisionedThroughputOverride = provisionedThroughputOverride;
            RegionName = regionName;
            ReplicaInaccessibleDateTime = replicaInaccessibleDateTime;
            ReplicaStatus = replicaStatus;
            ReplicaStatusDescription = replicaStatusDescription;
            ReplicaStatusProgress = replicaStatusProgress;
        }
    }
}