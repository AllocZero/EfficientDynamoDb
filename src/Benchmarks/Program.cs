using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using BenchmarkDotNet.Running;
using Benchmarks.AwsDdbSdk.Benchmarks;
using Benchmarks.AwsDdbSdk.Benchmarks.Deserialization;
using Benchmarks.AwsDdbSdk.Benchmarks.Deserialization.Models;
using Benchmarks.AwsDdbSdk.Entities;
using EfficientDynamoDb.DocumentModel.Extensions;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            // var json = JsonSerializer.Serialize(new QueryModel<KeysOnlyEntity>()
            // {
            //     Items = Enumerable.Range(0, 1000).Select(x=> new KeysOnlyEntity()).ToList()
            // });
            //
            // var result = JsonSerializer.DeserializeAsync<QueryModel<KeysOnlyEntity>>(new MemoryStream(Encoding.UTF8.GetBytes(json))).Result;
            //
            var benchmark = new EfficientEntityQueryBenchmark();
            benchmark.EntitiesCount = 1000;   
            benchmark.SetupMediumBenchmark();

            benchmark.MediumBenchmarkAsync().Wait();

            BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run();
        }
    }
}