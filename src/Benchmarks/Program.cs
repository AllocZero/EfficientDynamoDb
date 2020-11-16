using Amazon.DynamoDBv2.DocumentModel;
using BenchmarkDotNet.Running;
using Benchmarks.AwsDdbSdk.Benchmarks;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var bench = new DeserializationBenchmark();
            bench.SetupUnmarshaller();
            
            bench.EfficientReaderBenchmark().Wait();

            BenchmarkSwitcher.FromTypes(new[] {typeof(DeserializationBenchmark)}).RunAll();
        }
    }
}