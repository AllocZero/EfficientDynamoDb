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
        public string F9 { get; set; } = "test_property_9";
        
        [DynamoDBProperty("f10")]
        public string F10 { get; set; } = "test_property_10";
        
        [DynamoDBProperty("f11")]
        public string F11 { get; set; } = "test_property_11";
        
        [DynamoDBProperty("f12")]
        public string F12 { get; set; } = "test_property_12";
    }
}