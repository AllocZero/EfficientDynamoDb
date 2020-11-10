using Amazon.DynamoDBv2.DataModel;
using Benchmarks.AwsDdbSdk.Constants;

namespace Benchmarks.AwsDdbSdk.Entities
{
    [DynamoDBTable(Tables.TestTable)]
    public class MediumComplexFieldsEntity : KeysOnlyEntity
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
        public TestNestedObject F5 { get; set; } = new TestNestedObject();
    }

    public class TestNestedObject
    {
        [DynamoDBProperty("f1")]
        public string StringField1 { get; set; } = "nested_field_1";
        
        [DynamoDBProperty("f2")]
        public string StringField2 { get; set; } = "nested_field_1";
        
        [DynamoDBProperty("f3")]
        public string StringField3 { get; set; } = "nested_field_1";
        
        [DynamoDBProperty("f4")]
        public string StringField4 { get; set; } = "nested_field_1";

        [DynamoDBProperty("f5")]
        public int IntField1 { get; set; } = 1;
        
        [DynamoDBProperty("f6")]
        public int IntField2 { get; set; } = int.MaxValue;
        
        [DynamoDBProperty("f7")]
        public int IntField3 { get; set; } = 1000;
        
        [DynamoDBProperty("f8")]
        public int IntField4 { get; set; } = 1000000;
    }
}