using Amazon.DynamoDBv2.DataModel;
using Benchmarks.AwsDdbSdk.Constants;

namespace Benchmarks.AwsDdbSdk.Entities
{
    [DynamoDBTable(Tables.TestTable)]
    public class KeysOnlyEntity
    {
        [DynamoDBHashKey("pk")]
        public string Pk { get; set; }
        
        [DynamoDBRangeKey("sk")]
        public string Sk { get; set; }
    }
}