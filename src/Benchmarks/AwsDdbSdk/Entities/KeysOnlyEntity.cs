using Amazon.DynamoDBv2.DataModel;
using Benchmarks.AwsDdbSdk.Constants;
using EfficientDynamoDb.Attributes;
using EfficientDynamoDb.Internal.Converters.Primitives;

namespace Benchmarks.AwsDdbSdk.Entities
{
    [EfficientDynamoDb.Attributes.DynamoDBTable(Tables.TestTable)]
    [Amazon.DynamoDBv2.DataModel.DynamoDBTable(Tables.TestTable)]
    public class KeysOnlyEntity
    {
        [EfficientDynamoDb.Attributes.DynamoDBProperty("pk", typeof(StringDdbConverter), DynamoDbAttributeType.PartitionKey)]
        [DynamoDBHashKey("pk")]
        public string Pk { get; set; }
        
        [EfficientDynamoDb.Attributes.DynamoDBProperty("sk", typeof(StringDdbConverter), DynamoDbAttributeType.SortKey)]
        [DynamoDBRangeKey("sk")]
        public string Sk { get; set; }
    }
}