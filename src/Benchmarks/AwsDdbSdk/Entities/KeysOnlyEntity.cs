using Amazon.DynamoDBv2.DataModel;
using Benchmarks.AwsDdbSdk.Constants;
using EfficientDynamoDb.Internal.Mapping.Converters;

namespace Benchmarks.AwsDdbSdk.Entities
{
    [DynamoDBTable(Tables.TestTable)]
    public class KeysOnlyEntity
    {
        [EfficientDynamoDb.Internal.Mapping.Attributes.DynamoDBProperty("pk", typeof(StringDdbConverter))]
        [DynamoDBHashKey("pk")]
        public string Pk { get; set; }
        
        [EfficientDynamoDb.Internal.Mapping.Attributes.DynamoDBProperty("sk", typeof(StringDdbConverter))]
        [DynamoDBRangeKey("sk")]
        public string Sk { get; set; }
    }
}