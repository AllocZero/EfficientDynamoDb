#if !NETSTANDARD13
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Amazon.Runtime
{
    /// <summary>
    /// An enumerable containing all of the Result Keys for a paginator
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TResultKey"></typeparam>
    public class PaginatedResultKeyResponse<TResponse, TResultKey> : IPaginatedEnumerable<TResultKey>
    {
        private readonly IPaginator<TResponse> _paginator;
        private readonly Func<TResponse, IEnumerable<TResultKey>> _resultKeySelector;

        /// <summary>
        /// Create a PaginatedResultKeyResponse by providing any operation paginator
        /// and a selector function for the result key
        /// </summary>
        /// <param name="paginator"></param>
        /// <param name="resultKeySelector"></param>
        public PaginatedResultKeyResponse(IPaginator<TResponse> paginator, Func<TResponse, IEnumerable<TResultKey>> resultKeySelector)
        {
            this._paginator = paginator;
            this._resultKeySelector = resultKeySelector;
        }
#if AWS_ASYNC_ENUMERABLES_API
        /// <summary>
        /// Get the result keys asynchronously
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
         public async IAsyncEnumerator<TResultKey> GetAsyncEnumerator(CancellationToken cancellationToken = default)
         {
             await foreach (var parent in _paginator.PaginateAsync().WithCancellation(cancellationToken).ConfigureAwait(false))
             {
                 foreach (var resultKey in _resultKeySelector(parent))
                 {
                     cancellationToken.ThrowIfCancellationRequested();
                     yield return resultKey;
                 }
             }
         }
#endif
#if BCL
        /// <summary>
        /// Get the result keys synchronously
        /// </summary>
        /// <returns></returns>
         public IEnumerator<TResultKey> GetEnumerator()
         {
             foreach (var parent in _paginator.Paginate())
             {
                 foreach (var i in _resultKeySelector(parent))
                 {
                     yield return i;
                 }
             }
         }
 
         IEnumerator IEnumerable.GetEnumerator()
         {
             return GetEnumerator();
         }
#endif
    }
}
#endif
