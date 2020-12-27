using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
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
            var json = JsonSerializer.Serialize(new QueryModel<KeysOnlyEntity>()
            {
                Items = new List<KeysOnlyEntity>{new KeysOnlyEntity()}
            });
            
            var result = JsonSerializer.Deserialize<QueryModel<KeysOnlyEntity>>(json);
            //
            var benchmark = new EfficientEntityQueryBenchmark();
            benchmark.EntitiesCount = 1000;   
            benchmark.SetupMediumBenchmark();

            benchmark.MediumBenchmarkAsync().Wait();

            BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run();
        }
    }
}