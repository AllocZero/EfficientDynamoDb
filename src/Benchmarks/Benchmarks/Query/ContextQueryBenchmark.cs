using System.Collections.Generic;
using System.Threading.Tasks;

namespace Benchmarks.Query
{
    public class ContextQueryBenchmark : QueryBenchmarkBase
    {
        protected override async Task<IReadOnlyCollection<object>> QueryAsync<T>(string pk)
        {
            return await DbContext.QueryAsync<T>(pk).GetRemainingAsync().ConfigureAwait(false);
        }
    }
}