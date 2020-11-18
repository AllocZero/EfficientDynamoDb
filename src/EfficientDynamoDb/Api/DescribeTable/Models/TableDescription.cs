using System;
using System.Collections.Generic;
using EfficientDynamoDb.Api.DescribeTable.Models.Enums;
using EfficientDynamoDb.Api.DescribeTable.Models.Indexes;

namespace EfficientDynamoDb.Api.DescribeTable.Models
{
    public class TableDescription
    {
        public ArchivalSummary? ArchivalSummary { get; set; }

        public IReadOnlyList<AttributeDefinition> AttributeDefinitions { get; set; } = Array.Empty<AttributeDefinition>();
        
        public BillingModeSummary? BillingModeSummary { get; set; }
        
        public DateTime CreationDateTime { get; set; }

        public IReadOnlyList<GlobalSecondaryIndexDescription> GlobalSecondaryIndexes { get; set; } = Array.Empty<GlobalSecondaryIndexDescription>();
        
        public string? GlobalTableVersion { get; set; }
        
        public long ItemCount { get; set; }

        public IReadOnlyList<KeySchemaElement> KeySchema { get; set; } = Array.Empty<KeySchemaElement>();
        
        public string? LatestStreamArn { get; set; }
        
        public string? LatestStreamLabel { get; set; }

        public IReadOnlyList<LocalSecondaryIndexDescription> LocalSecondaryIndexes { get; set; } = Array.Empty<LocalSecondaryIndexDescription>();
        
        public ProvisionedThroughputDescription? ProvisionedThroughput { get; set; }

        public IReadOnlyList<ReplicaDescription> Replicas { get; set; } = Array.Empty<ReplicaDescription>();
        
        public RestoreSummary? RestoreSummary { get; set; }
        
        public SseDescription? SSEDescription { get; set; }
        
        public StreamSpecification? StreamSpecification { get; set; }

        public string TableArn { get; set; } = string.Empty;

        public string TableId { get; set; } = string.Empty;

        public string TableName { get; set; } = string.Empty;
        
        public long TableSizeBytes { get; set; }
        
        public TableStatus TableStatus { get; set; }
    }
}