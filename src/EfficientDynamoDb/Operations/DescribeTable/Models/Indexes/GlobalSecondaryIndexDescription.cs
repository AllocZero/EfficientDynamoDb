using System.Collections.Generic;
using EfficientDynamoDb.Operations.DescribeTable.Models.Enums;

namespace EfficientDynamoDb.Operations.DescribeTable.Models.Indexes
{
    public class GlobalSecondaryIndexDescription : IndexDescriptionBase
    {
        public bool Backfilling { get; }

        public IndexStatus IndexStatus { get; }

        public ProvisionedThroughputDescription ProvisionedThroughput { get; }

        public GlobalSecondaryIndexDescription(string indexArn, string indexName, long indexSizeBytes, long itemCount,
            IReadOnlyList<KeySchemaElement> keySchema, Projection projection, bool backfilling, IndexStatus indexStatus,
            ProvisionedThroughputDescription provisionedThroughput) : base(indexArn, indexName, indexSizeBytes, itemCount, keySchema, projection)
        {
            Backfilling = backfilling;
            IndexStatus = indexStatus;
            ProvisionedThroughput = provisionedThroughput;
        }
    }
}