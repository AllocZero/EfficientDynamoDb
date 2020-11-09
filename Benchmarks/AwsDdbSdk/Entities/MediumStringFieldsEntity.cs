using Amazon.DynamoDBv2.DataModel;
using Benchmarks.AwsDdbSdk.Constants;

namespace Benchmarks.AwsDdbSdk.Entities
{
    [DynamoDBTable(Tables.TestTable)]
    public class MediumStringFieldsEntity : KeysOnlyEntity
    {
        [DynamoDBProperty("f1")]
        public string F1 { get; set; }
        
        [DynamoDBProperty("f2")]
        public string F2 { get; set; }
        
        [DynamoDBProperty("f3")]
        public string F3 { get; set; }
        
        [DynamoDBProperty("f4")]
        public string F4 { get; set; }
        
        [DynamoDBProperty("f5")]
        public string F5 { get; set; }
        
        [DynamoDBProperty("f6")]
        public string F6 { get; set; }
        
        [DynamoDBProperty("f7")]
        public string F7 { get; set; }
        
        [DynamoDBProperty("f8")]
        public string F8 { get; set; }
        
        [DynamoDBProperty("f9")]
        public string F9 { get; set; }
        
        [DynamoDBProperty("f10")]
        public string F10 { get; set; }
        
        [DynamoDBProperty("f11")]
        public string F11 { get; set; }
        
        [DynamoDBProperty("f12")]
        public string F12 { get; set; }
    }
}