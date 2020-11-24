using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class PlaygroundBenchmark
    {
        private static readonly string BigStringValue = new string(Enumerable.Range(0, 100000).Select(x => 'x').ToArray());
        
        // [Benchmark]
        public int StructList()
        {
            var list = new List<StringAttributeValue>();

            for (var i = 0; i < 10000; i++)
            {
                list.Add(new StringAttributeValue());
            }

            return list.Count;
        }
        
        // [Benchmark]
        public int InterfaceList()
        {
            var list = new List<IAttributeValue>();

            for (var i = 0; i < 10000; i++)
            {
                list.Add(new StringAttributeValue());
            }

            return list.Count;
        }
        
        [Benchmark]
        public int HackStructList()
        {
            var list = new List<AttributeValue>();

            for (var i = 0; i < 10000; i++)
            {
                list.Add(new AttributeValue());
            }

            return list.Count;
        }
    }
}