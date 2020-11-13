using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class ContextQueryBenchmark : QueryBenchmarkBase
    {
        protected override async Task<IReadOnlyCollection<object>> QueryAsync<T>(string pk)
        {
            return await DbContext.QueryAsync<T>(pk).GetRemainingAsync().ConfigureAwait(false);
        }
    }
}