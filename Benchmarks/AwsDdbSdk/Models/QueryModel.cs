using System.Collections.Generic;

namespace Benchmarks.AwsDdbSdk.Models
{
    public class QueryModel
    {
        public int Count { get; set; }
        
        public List<MediumModel> Items { get; set; }
        
        public int ScannedCount { get; set; }
    }
}