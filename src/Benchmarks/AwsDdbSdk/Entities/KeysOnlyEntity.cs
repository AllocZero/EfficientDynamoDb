using Amazon.DynamoDBv2.DataModel;
using Benchmarks.AwsDdbSdk.Constants;
using EfficientDynamoDb.Internal.Converters.Primitives;

namespace Benchmarks.AwsDdbSdk.Entities
{
    [EfficientDynamoDb.DocumentModel.Attributes.DynamoDBTable(Tables.TestTable)]
    [DynamoDBTable(Tables.TestTable)]
    public class KeysOnlyEntity
    {
        [EfficientDynamoDb.DocumentModel.Attributes.DynamoDBProperty("pk", typeof(StringDdbConverter))]
        [DynamoDBHashKey("pk")]
        public string Pk { get; set; }
        
        [EfficientDynamoDb.DocumentModel.Attributes.DynamoDBProperty("sk", typeof(StringDdbConverter))]
        [DynamoDBRangeKey("sk")]
        public string Sk { get; set; }
    }
}