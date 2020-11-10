﻿using System.Reflection;
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

            BenchmarkRunner.Run(Assembly.GetExecutingAssembly());
            BenchmarkSwitcher.FromTypes(new[] {typeof(ContextQueryBenchmark), typeof(LowLevelQueryBenchmark)}).RunAll();
        }
    }
}