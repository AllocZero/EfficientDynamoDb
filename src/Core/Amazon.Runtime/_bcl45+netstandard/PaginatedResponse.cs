#if !NETSTANDARD13
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Amazon.Runtime
{
    /// <summary>
    /// An enumerable containing all of the responses for a paginated
    /// operation
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public class PaginatedResponse<TResponse> : IPaginatedEnumerable<TResponse>
    {
        private readonly IPaginator<TResponse> _paginator;

        /// <summary>
        /// Create a PaginatedResponse object by providing
        /// any operation paginator
        /// </summary>
        /// <param name="paginator"></param>
        public PaginatedResponse(IPaginator<TResponse> paginator)
        {
            this._paginator = paginator;
        }
#if AWS_ASYNC_ENUMERABLES_API
        /// <summary>
        /// Get responses asynchronously
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
         public async IAsyncEnumerator<TResponse> GetAsyncEnumerator(CancellationToken cancellationToken = default)
         {
             await foreach (var response in _paginator.PaginateAsync().WithCancellation(cancellationToken).ConfigureAwait(false))
             {
                 cancellationToken.ThrowIfCancellationRequested();
                 yield return response;
             }
         }
#endif
#if BCL
        /// <summary>
        /// Get responses synchronously
        /// </summary>
        /// <returns></returns>        
        public IEnumerator<TResponse> GetEnumerator()
         {
             return _paginator.Paginate().GetEnumerator();
         }
 
         IEnumerator IEnumerable.GetEnumerator()
         {
             return GetEnumerator();
         }
#endif
    }
}
#endif
