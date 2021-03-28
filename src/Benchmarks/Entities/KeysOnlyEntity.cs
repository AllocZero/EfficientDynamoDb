using Amazon.DynamoDBv2.DataModel;
using Benchmarks.Constants;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Internal.Converters.Primitives;

namespace Benchmarks.Entities
{
    [EfficientDynamoDb.Attributes.DynamoDbTable(Tables.TestTable)]
    [Amazon.DynamoDBv2.DataModel.DynamoDBTable(Tables.TestTable)]
    public class KeysOnlyEntity
    {
        [EfficientDynamoDb.Attributes.DynamoDbProperty("pk", typeof(StringDdbConverter), DynamoDbAttributeType.PartitionKey)]
        [DynamoDBHashKey("pk")]
        public string Pk { get; set; }
        
        [EfficientDynamoDb.Attributes.DynamoDbProperty("sk", typeof(StringDdbConverter), DynamoDbAttributeType.SortKey)]
        [DynamoDBRangeKey("sk")]
        public string Sk { get; set; }
    }
}