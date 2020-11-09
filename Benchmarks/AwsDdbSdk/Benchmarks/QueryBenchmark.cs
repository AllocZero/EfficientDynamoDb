using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using BenchmarkDotNet.Attributes;
using Benchmarks.AwsDdbSdk.Entities;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    public class QueryBenchmark
    {
        private DynamoDBContext _context;
        
        [GlobalSetup]
        public async Task Setup()
        {
            _context = new DynamoDBContext();
        }
        public async Task<List<KeysOnlyEntity>> KeysOnlyBenchmarkAsync()
        {
            
        }
    }
}