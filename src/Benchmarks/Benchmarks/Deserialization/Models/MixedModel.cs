using Benchmarks.Benchmarks.Deserialization.Models.Wrappers;

namespace Benchmarks.Benchmarks.Deserialization.Models
{
    public class MixedModel
    {
        public MapWrapper<MapModel> m { get; set; }

        public ListWrapper<MapWrapper<MapModel>> l1 { get; set; }

        public ListWrapper<MapWrapper<MapModel>> l2 { get; set; } 
        
        public ListWrapper<MapWrapper<MapModel>> l3 { get; set; }
        
        public StringSetWrapper ss { get; set; }
        
        public NumberSetWrapper ns { get; set; }
        
        public StringWrapper s { get; set; }
        
        public NumberWrapper n { get; set; }

        public BoolWrapper b { get; set; }
    }
    
    public class MapModel
    {
        public StringWrapper p1 { get; set; }
    }
}