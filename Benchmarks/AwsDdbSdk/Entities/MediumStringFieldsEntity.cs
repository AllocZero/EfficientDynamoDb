using Amazon.DynamoDBv2.DataModel;
using Benchmarks.AwsDdbSdk.Constants;

namespace Benchmarks.AwsDdbSdk.Entities
{
    [DynamoDBTable(Tables.TestTable)]
    public class MediumStringFieldsEntity : KeysOnlyEntity
    {
        [DynamoDBProperty("f1")] 
        public string F1 { get; set; } = "test_property_1";
        
        [DynamoDBProperty("f2")]
        public string F2 { get; set; } = "test_property_2";
        
        [DynamoDBProperty("f3")]
        public string F3 { get; set; } = "test_property_3";
        
        [DynamoDBProperty("f4")]
        public string F4 { get; set; } = "test_property_4";
        
        [DynamoDBProperty("f5")]
        public string F5 { get; set; } = "test_property_5";
        
        [DynamoDBProperty("f6")]
        public string F6 { get; set; } = "test_property_6";
        
        [DynamoDBProperty("f7")]
        public string F7 { get; set; } = "test_property_7";
        
        [DynamoDBProperty("f8")]
        public string F8 { get; set; } = "test_property_8";
        
        [DynamoDBProperty("f9")]
        public int F9 { get; set; } = 1;
        
        [DynamoDBProperty("f10")]
        public int F10 { get; set; } = int.MaxValue;
        
        [DynamoDBProperty("f11")]
        public int F11 { get; set; } = 1000;
        
        [DynamoDBProperty("f12")]
        public int F12 { get; set; } = 1000000;
    }
}