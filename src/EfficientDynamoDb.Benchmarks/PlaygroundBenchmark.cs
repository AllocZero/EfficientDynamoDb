using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Builder;

namespace EfficientDynamoDb.Benchmarks
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

        [GlobalSetup(Target = nameof(NotPooledGetHttpContent))]
        public void SetupNotPooledGetHttpContent()
        {
            GlobalDynamoDbConfig.UseMemoryStreamPooling = false;
            GlobalDynamoDbConfig.UsePooledBufferForJsonWrites = false;
        }
        
        // [Benchmark]
        public Task<long> NotPooledGetHttpContent() => GetHttpContent();
        
        [GlobalSetup(Target = nameof(PooledGetHttpContent))]
        public void SetupPooledGetHttpContent()
        {
            GlobalDynamoDbConfig.UseMemoryStreamPooling = true;
            GlobalDynamoDbConfig.UsePooledBufferForJsonWrites = true;
        }

        // [Benchmark]
        public Task<long> PooledGetHttpContent() => GetHttpContent();
        
        private async Task<long> GetHttpContent()
        {
            using var content = new GetItemHttpContent<StringAttributeValue, StringAttributeValue>("table", "pk", new StringAttributeValue(BigStringValue), "sk",
                new StringAttributeValue(BigStringValue));

            await using var stream = await content.ReadAsStreamAsync().ConfigureAwait(false);
            
            return stream.Length;
        }
    }
}