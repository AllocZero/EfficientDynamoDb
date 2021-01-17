using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace Benchmarks.AwsDdbSdk.Entities
{
    public class MapObject
    {
        [EfficientDynamoDb.DocumentModel.Attributes.DynamoDBProperty("p1")]
        [DynamoDBProperty("p1")] 
        public string P1 { get; set; }
    }
    
    public class MixedEntity : KeysOnlyEntity
    {
        [EfficientDynamoDb.DocumentModel.Attributes.DynamoDBProperty("m")]
        [DynamoDBProperty("m")] 
        public MapObject M { get; set; }

        [EfficientDynamoDb.DocumentModel.Attributes.DynamoDBProperty("l1")]
        [DynamoDBProperty("l1")] 
        public List<MapObject> L1 { get; set; }
        
        [EfficientDynamoDb.DocumentModel.Attributes.DynamoDBProperty("l2")]
        [DynamoDBProperty("l2")] 
        public List<MapObject> L2 { get; set; }
        
        [EfficientDynamoDb.DocumentModel.Attributes.DynamoDBProperty("l3")]
        [DynamoDBProperty("l3")] 
        public List<MapObject> L3 { get; set; }

        [EfficientDynamoDb.DocumentModel.Attributes.DynamoDBProperty("ss")]
        [DynamoDBProperty("ss")] 
        public HashSet<string> Ss { get; set; }

        [EfficientDynamoDb.DocumentModel.Attributes.DynamoDBProperty("ns")]
        [DynamoDBProperty("ns")] 
        public HashSet<int> Ns { get; set; }

        [EfficientDynamoDb.DocumentModel.Attributes.DynamoDBProperty("s")]
        [DynamoDBProperty("s")]
        public string S { get; set; }
        
        [EfficientDynamoDb.DocumentModel.Attributes.DynamoDBProperty("n")]
        [DynamoDBProperty("n")]
        public int N { get; set; }

        [EfficientDynamoDb.DocumentModel.Attributes.DynamoDBProperty("b")]
        [DynamoDBProperty("b")] 
        public bool B { get; set; }
    }
}