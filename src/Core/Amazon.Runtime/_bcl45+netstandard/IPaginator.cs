#if !NETSTANDARD13
using System.Collections.Generic;
using System.Threading;

namespace Amazon.Runtime
{
    /// <summary>
    /// Interface for operation paginators
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public interface IPaginator<TResponse>
    {
#if BCL
         IEnumerable<TResponse> Paginate();
#endif
#if AWS_ASYNC_ENUMERABLES_API
         IAsyncEnumerable<TResponse> PaginateAsync(CancellationToken cancellationToken = default);
#endif
    }
}
#endif
