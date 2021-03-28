using System;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace Benchmarks.Benchmarks
{
    public class ArithmeticBenchmark
    {
        [Params(100000)]
        public int Iterations;

        private int[] _input;

        [GlobalSetup]
        public void Setup()
        {
            var random = new Random();
            _input = Enumerable.Range(0, 100000).Select(_ => random.Next(0, 100)).ToArray();
        }
        
        // [Benchmark]
        public bool RawBenchmark()
        {
            var sum = false;

            for (var i = 0; i < Iterations; i++)
            {
                var objectLevel = _input[i];
                sum |= (objectLevel != 0 && objectLevel % 2 == 0);
            }

            return sum;
        }
        
        [Benchmark]
        public bool BitwiseEvenRawBenchmark()
        {
            var sum = false;

            for (var i = 0; i < Iterations; i++)
            {
                var objectLevel = _input[i];
                sum |= (objectLevel != 0 && (objectLevel & 1) == 0);
            }

            return sum;
        }
        
        [Benchmark]
        public bool BitwiseAllBenchmark()
        {
            var sum = false;

            for (var i = 0; i < Iterations; i++)
            {
                var objectLevel = _input[i];
                sum |= (-objectLevel >> 31) + (objectLevel & 1) == -2;
            }

            return sum;
        }
    }
}