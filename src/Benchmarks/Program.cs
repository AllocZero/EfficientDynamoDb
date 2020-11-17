using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using BenchmarkDotNet.Running;
using Benchmarks.AwsDdbSdk.Benchmarks;

namespace Benchmarks
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // var bench = new ContextQueryBenchmark();
            //
            // bench.SetupMixedBenchmarkAsync().Wait();
            
            var bench = new DeserializationBenchmark();
            await bench.SetupUnmarshaller();
            
            // for (var i = 0; i < 1000; i++)
            {
                await bench.EfficientReaderBenchmark().ConfigureAwait(false);
            }

            BenchmarkSwitcher.FromTypes(new[] {typeof(DeserializationBenchmark)}).RunAll();
        }
    }
}