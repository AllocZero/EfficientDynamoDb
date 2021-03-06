using System.Collections.Generic;

namespace EfficientDynamoDb.Operations.DescribeTable.Models.Indexes
{
    public abstract class IndexDescriptionBase
    {
        public string IndexArn { get; }

        public string IndexName { get; }

        public long IndexSizeBytes { get; }

        public long ItemCount { get; }

        public IReadOnlyList<KeySchemaElement> KeySchema { get; }

        public Projection Projection { get; }

        public IndexDescriptionBase(string indexArn, string indexName, long indexSizeBytes, long itemCount, IReadOnlyList<KeySchemaElement> keySchema,
            Projection projection)
        {
            IndexArn = indexArn;
            IndexName = indexName;
            IndexSizeBytes = indexSizeBytes;
            ItemCount = itemCount;
            KeySchema = keySchema;
            Projection = projection;
        }
    }
}