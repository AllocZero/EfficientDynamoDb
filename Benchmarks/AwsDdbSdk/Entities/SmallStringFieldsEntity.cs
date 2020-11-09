using Amazon.DynamoDBv2.DataModel;
using Benchmarks.AwsDdbSdk.Constants;

namespace Benchmarks.AwsDdbSdk.Entities
{
    [DynamoDBTable(Tables.TestTable)]
    public class SmallStringFieldsEntity : KeysOnlyEntity
    {
        [DynamoDBProperty("f1")]
        public string F1 { get; set; }
        
        [DynamoDBProperty("f2")]
        public string F2 { get; set; }
        
        [DynamoDBProperty("f3")]
        public string F3 { get; set; }
        
        [DynamoDBProperty("f4")]
        public string F4 { get; set; }
    }
}