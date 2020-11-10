using BenchmarkDotNet.Running;
using Benchmarks.AwsDdbSdk.Benchmarks;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            // var bench = new ContextQueryBenchmark();
            // bench.SetupLargeBenchmarkAsync().Wait();

            var summary = BenchmarkRunner.Run<ContextQueryBenchmark>();
        }
    }
}