using System.Collections.Generic;

namespace Benchmarks.AwsDdbSdk.Benchmarks.Deserialization.Models.Wrappers
{
    public class ListWrapper<TWrapper>
    {
        public List<TWrapper> L { get; set; }
    }
}