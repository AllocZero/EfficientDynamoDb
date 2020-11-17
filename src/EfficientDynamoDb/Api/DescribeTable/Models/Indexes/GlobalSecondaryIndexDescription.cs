using System.Collections.Generic;
using EfficientDynamoDb.Api.DescribeTable.Models.Enums;

namespace EfficientDynamoDb.Api.DescribeTable.Models.Indexes
{
    public class GlobalSecondaryIndexDescription : IndexDescriptionBase
    {
        public bool Backfilling { get; }

        public IndexStatus IndexStatus { get; }

        public ProvisionedThroughputDescription ProvisionedThroughput { get; }

        public GlobalSecondaryIndexDescription(string indexArn, string indexName, long indexSizeBytes, long itemCount,
            IReadOnlyCollection<KeySchemaElement> keySchema, Projection projection, bool backfilling, IndexStatus indexStatus,
            ProvisionedThroughputDescription provisionedThroughput) : base(indexArn, indexName, indexSizeBytes, itemCount, keySchema, projection)
        {
            Backfilling = backfilling;
            IndexStatus = indexStatus;
            ProvisionedThroughput = provisionedThroughput;
        }
    }
}