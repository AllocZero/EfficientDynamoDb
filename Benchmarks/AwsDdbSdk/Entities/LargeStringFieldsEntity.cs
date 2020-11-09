using Amazon.DynamoDBv2.DataModel;
using Benchmarks.AwsDdbSdk.Constants;

namespace Benchmarks.AwsDdbSdk.Entities
{
    [DynamoDBTable(Tables.TestTable)]
    public class LargeStringFieldsEntity : KeysOnlyEntity
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
        
        [DynamoDBProperty("f13")]
        public string F13 { get; set; }
        
        [DynamoDBProperty("f14")]
        public string F14 { get; set; }
        
        [DynamoDBProperty("f15")]
        public string F15 { get; set; }
        
        [DynamoDBProperty("f16")]
        public string F16 { get; set; }
        
        [DynamoDBProperty("f17")]
        public string F17 { get; set; }
        
        [DynamoDBProperty("f18")]
        public string F18 { get; set; }
        
        [DynamoDBProperty("f19")]
        public string F19 { get; set; }
        
        [DynamoDBProperty("f20")]
        public string F20 { get; set; }
        
        [DynamoDBProperty("f21")]
        public string F21 { get; set; }
        
        [DynamoDBProperty("f22")]
        public string F22 { get; set; }
        
        [DynamoDBProperty("f23")]
        public string F23 { get; set; }
        
        [DynamoDBProperty("f24")]
        public string F24 { get; set; }
        
        [DynamoDBProperty("25")]
        public string F25 { get; set; }
        
        [DynamoDBProperty("26")]
        public string F26 { get; set; }
        
        [DynamoDBProperty("27")]
        public string F27 { get; set; }
        
        [DynamoDBProperty("28")]
        public string F28 { get; set; }
        
        [DynamoDBProperty("f29")]
        public string F29 { get; set; }
        
        [DynamoDBProperty("f30")]
        public string F30 { get; set; }
        
        [DynamoDBProperty("f31")]
        public string F31 { get; set; }
        
        [DynamoDBProperty("f32")]
        public string F32 { get; set; }
        
        [DynamoDBProperty("f33")]
        public string F33 { get; set; }
        
        [DynamoDBProperty("f34")]
        public string F34 { get; set; }
        
        [DynamoDBProperty("f35")]
        public string F35 { get; set; }
        
        [DynamoDBProperty("f36")]
        public string F36 { get; set; }
        
        [DynamoDBProperty("f37")]
        public string F37 { get; set; }
        
        [DynamoDBProperty("f38")]
        public string F38 { get; set; }
        
        [DynamoDBProperty("f39")]
        public string F39 { get; set; }
        
        [DynamoDBProperty("f40")]
        public string F40 { get; set; }
    }
}