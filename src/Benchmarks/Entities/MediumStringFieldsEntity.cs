using System;
using Amazon.DynamoDBv2.DataModel;
using Benchmarks.Constants;
using Benchmarks.Converters;

namespace Benchmarks.Entities
{
    [DynamoDBTable(Tables.TestTable)]
    public class MediumStringFieldsEntity : KeysOnlyEntity
    {
        [EfficientDynamoDb.Attributes.DynamoDbProperty("f1")]
        [DynamoDBProperty("f1")] 
        public string F1 { get; set; } = "test_property_1";
        
        [EfficientDynamoDb.Attributes.DynamoDbProperty("f2")]
        [DynamoDBProperty("f2")]
        public string F2 { get; set; } = "test_property_2";

        [EfficientDynamoDb.Attributes.DynamoDbProperty("f3")]
        [DynamoDBProperty("f3", typeof(DateTimeUtcConverter))] 
        public DateTime F3 { get; set; } = new DateTime(2020, 10, 9, 01, 0, 0, DateTimeKind.Utc);
        
        [EfficientDynamoDb.Attributes.DynamoDbProperty("f4")]
        [DynamoDBProperty("f4", typeof(DateTimeUtcConverter))]
        public DateTime F4 { get; set; } =new DateTime(2020, 10, 10, 01, 0, 0, DateTimeKind.Utc);
        
        [EfficientDynamoDb.Attributes.DynamoDbProperty("f5")]
        [DynamoDBProperty("f5")]
        public string F5 { get; set; } = "test_property_5";
        
        [EfficientDynamoDb.Attributes.DynamoDbProperty("f6")]
        [DynamoDBProperty("f6")]
        public string F6 { get; set; } = "test_property_6";
        
        [EfficientDynamoDb.Attributes.DynamoDbProperty("f7")]
        [DynamoDBProperty("f7", typeof(DateTimeUtcConverter))]
        public DateTime F7 { get; set; } = new DateTime(2020, 10, 11, 01, 0, 0, DateTimeKind.Utc);
        
        [EfficientDynamoDb.Attributes.DynamoDbProperty("f8")]
        [DynamoDBProperty("f8", typeof(DateTimeUtcConverter))]
        public DateTime F8 { get; set; } = new DateTime(2020, 10, 12, 01, 0, 0, DateTimeKind.Utc);
        
        [EfficientDynamoDb.Attributes.DynamoDbProperty("f9")]
        [DynamoDBProperty("f9")]
        public int F9 { get; set; } = 1;
        
        [EfficientDynamoDb.Attributes.DynamoDbProperty("f10")]
        [DynamoDBProperty("f10")]
        public int F10 { get; set; } = int.MaxValue;
        
        [EfficientDynamoDb.Attributes.DynamoDbProperty("f11")]
        [DynamoDBProperty("f11")]
        public int F11 { get; set; } = 1000;
        
        [EfficientDynamoDb.Attributes.DynamoDbProperty("f12")]
        [DynamoDBProperty("f12")]
        public int F12 { get; set; } = 1000000;
    }
}