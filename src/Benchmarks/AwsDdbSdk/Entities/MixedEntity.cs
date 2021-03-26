using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace Benchmarks.AwsDdbSdk.Entities
{
    public class MapObject
    {
        [EfficientDynamoDb.Attributes.DynamoDbProperty("p1")]
        [DynamoDBProperty("p1")] 
        public string P1 { get; set; }
    }
    
    public class MixedEntity : KeysOnlyEntity
    {
        [EfficientDynamoDb.Attributes.DynamoDbProperty("m")]
        [DynamoDBProperty("m")] 
        public MapObject M { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDbProperty("l1")]
        [DynamoDBProperty("l1")] 
        public List<MapObject> L1 { get; set; }
        
        [EfficientDynamoDb.Attributes.DynamoDbProperty("l2")]
        [DynamoDBProperty("l2")] 
        public List<MapObject> L2 { get; set; }
        
        [EfficientDynamoDb.Attributes.DynamoDbProperty("l3")]
        [DynamoDBProperty("l3")] 
        public List<MapObject> L3 { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDbProperty("ss")]
        [DynamoDBProperty("ss")] 
        public HashSet<string> Ss { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDbProperty("ns")]
        [DynamoDBProperty("ns")] 
        public HashSet<int> Ns { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDbProperty("s")]
        [DynamoDBProperty("s")]
        public string S { get; set; }
        
        [EfficientDynamoDb.Attributes.DynamoDbProperty("n")]
        [DynamoDBProperty("n")]
        public int N { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDbProperty("b")]
        [DynamoDBProperty("b")] 
        public bool B { get; set; }
    }
}