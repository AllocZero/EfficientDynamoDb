using System.Reflection;
using BenchmarkDotNet.Running;
using Benchmarks.AwsDdbSdk.Benchmarks;
using Benchmarks.AwsDdbSdk.Benchmarks.Deserialization;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var benchmark = new EfficientQueryBenchmark();
            
            benchmark.SetupMediumBenchmark();

            benchmark.MediumBenchmarkAsync().Wait();
            BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run();
        }
    }
}