using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;
using Benchmarks.AwsDdbSdk.Constants;

namespace Benchmarks.AwsDdbSdk.Entities
{
    [DynamoDBTable(Tables.TestTable)]
    public class MediumComplexCollectionFieldsEntity : KeysOnlyEntity
    {
        [DynamoDBProperty("f1")] 
        public string F1 { get; set; } = "test_property_1";
        
        // [DynamoDBProperty("f2")]
        // public string F2 { get; set; } = "test_property_2";
        //
        // [DynamoDBProperty("f3")]
        // public string F3 { get; set; } = "test_property_3";
        //
        // [DynamoDBProperty("f4")]
        // public string F4 { get; set; } = "test_property_4";
        
        [DynamoDBProperty("f5")]
        public TestNestedObject F5 { get; set; } = new TestNestedObject();
        
        [DynamoDBProperty("f6")]
        public List<string> F6 { get; set; } = new List<string>(3) {"test_property_2", "test_property_3", "test_property_4"}; 
    }
}