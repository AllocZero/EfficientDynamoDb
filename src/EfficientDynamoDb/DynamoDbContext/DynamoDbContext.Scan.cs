using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Operations.Scan;
using EfficientDynamoDb.Operations;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Scan;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb
{
    public partial class DynamoDbContext
    {
        /// <summary>
        /// Creates a builder for Scan operation.
        /// </summary>
        /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
        /// <returns>Scan operation builder.</returns>
        public IScanEntityRequestBuilder<TEntity> Scan<TEntity>() where TEntity : class => new ScanEntityRequestBuilder<TEntity>(this);
        
        internal async Task<OpResult<PagedResult<TEntity>>> ScanPageAsync<TEntity>(string? tableName, BuilderNode? node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new ScanHighLevelHttpContent(this, tableName, node);

            var apiResult = await Api.SendSafeAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            if (apiResult.Exception is not null)
                return new(apiResult.Exception);

            using var response = apiResult.Response!;
            var result = await ReadAsync<ScanEntityResponseProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return new(new PagedResult<TEntity>(result.Items, result.PaginationToken));
        }
        
        internal async IAsyncEnumerable<IReadOnlyList<TEntity>> ScanAsyncEnumerable<TEntity>(string? tableName, BuilderNode? node, [EnumeratorCancellation] CancellationToken cancellationToken = default) where TEntity : class
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

        internal IAsyncEnumerable<IReadOnlyList<TEntity>> ParallelScanAsyncEnumerable<TEntity>(string? tableName, BuilderNode? node, int totalSegments) where TEntity : class =>
            new ParallelScanAsyncEnumerable<TEntity>(this, tableName, node, totalSegments);
        
        internal async Task<OpResult<ScanEntityResponse<TEntity>>> ScanAsync<TEntity>(string? tableName, BuilderNode? node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new ScanHighLevelHttpContent(this, tableName, node);
            
            var apiResult = await Api.SendSafeAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            if (apiResult.Exception is not null)
                return new(apiResult.Exception);
            
            using var response = apiResult.Response!;
            var result = await ReadAsync<ScanEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return new(result);
        }
    }
}