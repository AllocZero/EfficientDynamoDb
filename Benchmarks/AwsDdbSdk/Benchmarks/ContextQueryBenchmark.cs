using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [MemoryDiagnoser]
    public class ContextQueryBenchmark : QueryBenchmarkBase
    {
        protected override async Task<int> QueryAsync<T>(string pk)
        {
            var entities = await DbContext.QueryAsync<T>(pk).GetRemainingAsync().ConfigureAwait(false);

            return entities.Count;
        }
    }
}