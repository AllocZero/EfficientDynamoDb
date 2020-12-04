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
            BenchmarkSwitcher.FromAssembly(Assembly.GetExecutingAssembly()).Run();
        }
    }
}