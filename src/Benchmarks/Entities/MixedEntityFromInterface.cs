using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace Benchmarks.Entities
{
    public interface IMixedEntityFromInterface
    {
        [EfficientDynamoDb.Attributes.DynamoDbProperty("m")]
        MapObject M { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDbProperty("l1")]
        List<MapObject> L1 { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDbProperty("l2")]
        List<MapObject> L2 { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDbProperty("l3")]
        List<MapObject> L3 { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDbProperty("ss")]
        HashSet<string> Ss { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDbProperty("ns")]
        HashSet<int> Ns { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDbProperty("s")]
        string S { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDbProperty("n")]
        int N { get; set; }

        [EfficientDynamoDb.Attributes.DynamoDbProperty("b")]
        bool B { get; set; }

    }

    public class MixedEntityFromInterface : KeysOnlyEntity, IMixedEntityFromInterface
    {
        [DynamoDBProperty("m")] 
        public MapObject M { get; set; }

        [DynamoDBProperty("l1")] 
        public List<MapObject> L1 { get; set; }
        
        [DynamoDBProperty("l2")] 
        public List<MapObject> L2 { get; set; }
        
        [DynamoDBProperty("l3")] 
        public List<MapObject> L3 { get; set; }

        [DynamoDBProperty("ss")] 
        public HashSet<string> Ss { get; set; }

        [DynamoDBProperty("ns")] 
        public HashSet<int> Ns { get; set; }

        [DynamoDBProperty("s")]
        public string S { get; set; }
        
        [DynamoDBProperty("n")]
        public int N { get; set; }

        [DynamoDBProperty("b")] 
        public bool B { get; set; }
    }
}