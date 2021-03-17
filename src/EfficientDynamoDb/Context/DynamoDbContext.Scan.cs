using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.Scan;
using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.Internal.Operations.Scan;

namespace EfficientDynamoDb.Context
{
    public partial class DynamoDbContext
    {
        public IScanEntityRequestBuilder<TEntity> Scan<TEntity>() where TEntity : class => new ScanEntityRequestBuilder<TEntity>(this);
        
        internal async Task<PagedResult<TEntity>> ScanPageAsync<TEntity>(string tableName, BuilderNode? node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new ScanHighLevelHttpContent(this, tableName, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<ScanEntityResponseProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return new PagedResult<TEntity>(result.Items, result.PaginationToken);
        }
        
        internal async IAsyncEnumerable<IReadOnlyList<TEntity>> ScanAsyncEnumerable<TEntity>(string tableName, BuilderNode? node, [EnumeratorCancellation] CancellationToken cancellationToken = default) where TEntity : class
        {
            ScanEntityResponseProjection<TEntity>? result = null;

            var isFirst = true;
            do
            {
                var contentNode = isFirst ? node : new PaginationTokenNode(result?.PaginationToken, node);
                using var httpContent = new ScanHighLevelHttpContent(this, tableName, contentNode);

                using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
                result = await ReadAsync<ScanEntityResponseProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

                yield return result.Items;

                isFirst = false;
            } while (result.PaginationToken != null);
        }

        internal IAsyncEnumerable<IReadOnlyList<TEntity>> ParallelScanAsyncEnumerable<TEntity>(string tableName, BuilderNode? node, int totalSegments) where TEntity : class =>
            new ParallelScanAsyncEnumerable<TEntity>(this, tableName, node, totalSegments);
        
        internal async Task<ScanEntityResponse<TEntity>> ScanAsync<TEntity>(string tableName, BuilderNode? node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new ScanHighLevelHttpContent(this, tableName, node);
            
            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            return await ReadAsync<ScanEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }
    }
}