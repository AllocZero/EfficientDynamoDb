using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace Benchmarks.AwsDdbSdk.Entities
{
    public class MapObject
    {
        [EfficientDynamoDb.Attributes.DynamoDBProperty("p1")]
        [DynamoDBProperty("p1")] 
        public string P1 { get; set; }
    }
    
    public class MixedEntity : KeysOnlyEntity
    {
        [EfficientDynamoDb.Attributes.DynamoDBProperty("m")]
        [DynamoDBProperty("m")] 
        public MapObject M { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDBProperty("l1")]
        [DynamoDBProperty("l1")] 
        public List<MapObject> L1 { get; set; }
        
        [EfficientDynamoDb.Attributes.DynamoDBProperty("l2")]
        [DynamoDBProperty("l2")] 
        public List<MapObject> L2 { get; set; }
        
        [EfficientDynamoDb.Attributes.DynamoDBProperty("l3")]
        [DynamoDBProperty("l3")] 
        public List<MapObject> L3 { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDBProperty("ss")]
        [DynamoDBProperty("ss")] 
        public HashSet<string> Ss { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDBProperty("ns")]
        [DynamoDBProperty("ns")] 
        public HashSet<int> Ns { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDBProperty("s")]
        [DynamoDBProperty("s")]
        public string S { get; set; }
        
        [EfficientDynamoDb.Attributes.DynamoDBProperty("n")]
        [DynamoDBProperty("n")]
        public int N { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDBProperty("b")]
        [DynamoDBProperty("b")] 
        public bool B { get; set; }
    }
}