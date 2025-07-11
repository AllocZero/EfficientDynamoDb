using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Operations.Query;
using EfficientDynamoDb.Operations;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb
{
    public partial class DynamoDbContext
    {
        /// <summary>
        /// Creates a builder for Query operation.
        /// </summary>
        /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
        /// <returns>Query operation builder.</returns>
        public IQueryEntityRequestBuilder<TEntity> Query<TEntity>() where TEntity : class => new QueryEntityRequestBuilder<TEntity>(this);

        internal async Task<OpResult<IReadOnlyList<TEntity>>> QueryListAsync<TEntity>(string? tableName, BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            QueryEntityResponseProjection<TEntity>? result = null;
            List<TEntity>? items = null;

            // Does not reuse QueryAsyncEnumerable because of potential allocations
            var isFirst = true;
            do
            {
                var contentNode = isFirst ? node : new PaginationTokenNode(result?.PaginationToken, node);
                using var httpContent = new QueryHighLevelHttpContent(this, tableName, contentNode);

                var apiResult = await Api.SendSafeAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
                if (apiResult.Exception is not null)
                    return new(apiResult.Exception);
                
                using var response = apiResult.Response!;
                result = await ReadAsync<QueryEntityResponseProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

                if (items == null)
                    items = result.Items;
                else
                    items.AddRange(result.Items);

                isFirst = false;
            } while (result.PaginationToken != null);

            return new(items);
        }

        internal async Task<OpResult<PagedResult<TEntity>>> QueryPageAsync<TEntity>(string? tableName, BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new QueryHighLevelHttpContent(this, tableName, node);

            var apiResult = await Api.SendSafeAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            if (apiResult.Exception is not null) 
                return new(apiResult.Exception);

            using var response = apiResult.Response!;
            var result = await ReadAsync<QueryEntityResponseProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return new(new PagedResult<TEntity>(result.Items, result.PaginationToken));
        }

        internal async IAsyncEnumerable<IReadOnlyList<TEntity>> QueryAsyncEnumerable<TEntity>(string? tableName, BuilderNode node,
            [EnumeratorCancellation] CancellationToken cancellationToken = default) where TEntity : class
        {
            QueryEntityResponseProjection<TEntity>? result = null;

            var isFirst = true;
            do
            {
                var contentNode = isFirst ? node : new PaginationTokenNode(result?.PaginationToken, node);
                using var httpContent = new QueryHighLevelHttpContent(this, tableName, contentNode);

                using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
                result = await ReadAsync<QueryEntityResponseProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

                yield return result.Items;

                isFirst = false;
            } while (result.PaginationToken != null);
        }

        internal async Task<OpResult<QueryEntityResponse<TEntity>>> QueryAsync<TEntity>(string? tableName, BuilderNode node, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            using var httpContent = new QueryHighLevelHttpContent(this, tableName, node);

            var apiResult = await Api.SendSafeAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            if (apiResult.Exception is not null)
                return new(apiResult.Exception);
            
            using var response = apiResult.Response!;
            var result = await ReadAsync<QueryEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            return new (result);
        }
    }
}