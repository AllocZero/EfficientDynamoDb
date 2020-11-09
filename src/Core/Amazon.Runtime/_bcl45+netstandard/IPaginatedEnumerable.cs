#if !NETSTANDARD13
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Amazon.Runtime
{
    /// <summary>
    /// Interface for enumerables consumed by the customer
    /// to read responses or result keys
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IPaginatedEnumerable<TResult>
#if BCL
         : IEnumerable<TResult>
#elif AWS_ASYNC_ENUMERABLES_API
         : IAsyncEnumerable<TResult>
#endif

    {

    }
}
#endif
