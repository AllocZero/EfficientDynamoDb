using System.Collections.Generic;

namespace EfficientDynamoDb.Api.DescribeTable.Models.Indexes
{
    public class LocalSecondaryIndexDescription : IndexDescriptionBase
    {
        public LocalSecondaryIndexDescription(string indexArn, string indexName, long indexSizeBytes, long itemCount,
            IReadOnlyList<KeySchemaElement> keySchema, Projection projection) : base(indexArn, indexName, indexSizeBytes, itemCount, keySchema,
            projection)
        {
        }
    }
}