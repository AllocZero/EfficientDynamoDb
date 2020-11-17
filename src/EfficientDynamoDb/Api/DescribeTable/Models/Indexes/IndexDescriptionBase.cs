using System.Collections.Generic;

namespace EfficientDynamoDb.Api.DescribeTable.Models.Indexes
{
    public abstract class IndexDescriptionBase
    {
        public string IndexArn { get; }

        public string IndexName { get; }

        public long IndexSizeBytes { get; }

        public long ItemCount { get; }

        public IReadOnlyCollection<KeySchemaElement> KeySchema { get; }

        public Projection Projection { get; }

        public IndexDescriptionBase(string indexArn, string indexName, long indexSizeBytes, long itemCount, IReadOnlyCollection<KeySchemaElement> keySchema,
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