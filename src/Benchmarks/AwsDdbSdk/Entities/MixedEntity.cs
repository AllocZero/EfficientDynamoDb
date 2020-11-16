using System.Collections.Generic;
using Amazon.DynamoDBv2.DataModel;

namespace Benchmarks.AwsDdbSdk.Entities
{
    public class MapObject
    {
        [DynamoDBProperty("p1")] 
        public string P1 { get; set; } = "test_property_1";
    }
    
    public class MixedEntity : KeysOnlyEntity
    {
        [DynamoDBProperty("m")] 
        public MapObject M { get; set; } = new MapObject();

        [DynamoDBProperty("l1")] 
        public List<MapObject> L1 { get; set; } = new List<MapObject> {new MapObject()};
        
        [DynamoDBProperty("l2")] 
        public List<MapObject> L2 { get; set; } = new List<MapObject> {new MapObject()};
        
        [DynamoDBProperty("l3")] 
        public List<MapObject> L3 { get; set; } = new List<MapObject> {new MapObject()};

        [DynamoDBProperty("ss")] 
        public HashSet<string> Ss { get; set; } = new HashSet<string> {"string_1"};

        [DynamoDBProperty("ns")] 
        public HashSet<int> Ns { get; set; } = new HashSet<int> {50};

        [DynamoDBProperty("s")]
        public string S { get; set; } = "test_property2";
        
        [DynamoDBProperty("n")]
        public int N { get; set; } = 100;

        [DynamoDBProperty("b")] 
        public bool B { get; set; } = true;
    }
}