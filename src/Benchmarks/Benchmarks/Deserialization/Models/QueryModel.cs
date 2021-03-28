using System.Collections.Generic;

namespace Benchmarks.Deserialization.Models
{
    public class QueryModel<TEntity>
    {
        public int Count { get; set; }
        
        public List<TEntity> Items { get; set; }
        
        public int ScannedCount { get; set; }
    }
}