using System;
using Amazon.DynamoDBv2.DataModel;
using Benchmarks.AwsDdbSdk.Converters;

namespace Benchmarks.AwsDdbSdk.Entities
{
    public class MediumConverterFieldsEntity: KeysOnlyEntity
    {
        [DynamoDBProperty("f1")] 
        public string F1 { get; set; } = "test_property_1";
        
        [DynamoDBProperty("f2")]
        public string F2 { get; set; } = "test_property_2";

        [DynamoDBProperty("f3", typeof(DateTimeUtcConverter))] 
        public DateTime F3 { get; set; } = new DateTime(2020, 10, 9, 01, 0, 0, DateTimeKind.Utc);
        
        [DynamoDBProperty("f4", typeof(DateTimeUtcConverter))]
        public DateTime F4 { get; set; } =new DateTime(2020, 10, 10, 01, 0, 0, DateTimeKind.Utc);
        
        [DynamoDBProperty("f5")]
        public string F5 { get; set; } = "test_property_5";
        
        [DynamoDBProperty("f6")]
        public string F6 { get; set; } = "test_property_6";
        
        [DynamoDBProperty("f7", typeof(DateTimeUtcConverter))]
        public DateTime F7 { get; set; } = new DateTime(2020, 10, 11, 01, 0, 0, DateTimeKind.Utc);
        
        [DynamoDBProperty("f8", typeof(DateTimeUtcConverter))]
        public DateTime F8 { get; set; } = new DateTime(2020, 10, 12, 01, 0, 0, DateTimeKind.Utc);
        
        [DynamoDBProperty("f9", typeof(SparseIntConverter))]
        public int F9 { get; set; } = 1;
        
        [DynamoDBProperty("f10", typeof(SparseIntConverter))]
        public int F10 { get; set; } = int.MaxValue;
        
        [DynamoDBProperty("f11", typeof(SparseIntConverter))]
        public int F11 { get; set; } = 1000;
        
        [DynamoDBProperty("f12", typeof(SparseIntConverter))]
        public int F12 { get; set; } = 1000000;
    }
}