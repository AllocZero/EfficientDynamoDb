using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Extensions;

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
        public int HackStructList()
        {
            var list = new List<AttributeValue>();

            for (var i = 0; i < 10000; i++)
            {
                list.Add(new AttributeValue());
            }

            return list.Count;
        }

        
        [Benchmark]
        public int BytesBuffer()
        {
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);
           
            writer.WriteStartObject();
            for (var i = 0; i < 10000; i++)
            {
                writer.WriteString($"test_{i}", i);
            }
            writer.WriteEndObject();

            return writer.BytesPending;
        }
    }
}