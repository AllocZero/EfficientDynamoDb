using BenchmarkDotNet.Running;
using Benchmarks.AwsDdbSdk.Benchmarks;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            // var bench = new QueryBenchmark();
            // bench.SetupAsync().Wait();
            
            var summary = BenchmarkRunner.Run<LowLevelQueryBenchmark>();
        }
    }
}