using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.Internal.Operations.Query;

namespace EfficientDynamoDb.Context
{
    public partial class DynamoDbContext
    {
        public IQueryRequestBuilder<TEntity> Query<TEntity>() where TEntity : class => new QueryRequestBuilder<TEntity>(this);

        internal async Task<IReadOnlyList<TEntity>> QueryListAsync<TEntity>(string tableName, BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            QueryEntityResponseProjection<TEntity>? result = null;
            List<TEntity>? items = null;

            // Does not reuse QueryAsyncEnumerable because of potential allocations
            var isFirst = true;
            do
            {
                var contentNode = isFirst ? node : new PaginationTokenNode(result?.PaginationToken, node);
                using var httpContent = new QueryHighLevelHttpContent(tableName, Config.TableNamePrefix, Config.Metadata, contentNode);

                using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
                result = await ReadAsync<QueryEntityResponseProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

                if (items == null)
                    items = result.Items;
                else
                    items.AddRange(result.Items);

                isFirst = false;
            } while (result.PaginationToken != null);

            return items;
        }

        internal async Task<PagedResult<TEntity>> QueryPageAsync<TEntity>(string tableName, BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new QueryHighLevelHttpContent(tableName, Config.TableNamePrefix, Config.Metadata, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<QueryEntityResponseProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return new PagedResult<TEntity>(result.Items, result.PaginationToken);
        }

        internal async IAsyncEnumerable<IReadOnlyList<TEntity>> QueryAsyncEnumerable<TEntity>(string tableName, BuilderNode node,
            [EnumeratorCancellation] CancellationToken cancellationToken = default) where TEntity : class
        {
            QueryEntityResponseProjection<TEntity>? result = null;

            var isFirst = true;
            do
            {
                var contentNode = isFirst ? node : new PaginationTokenNode(result?.PaginationToken, node);
                using var httpContent = new QueryHighLevelHttpContent(tableName, Config.TableNamePrefix, Config.Metadata, contentNode);

                using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
                result = await ReadAsync<QueryEntityResponseProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

                yield return result.Items;

                isFirst = false;
            } while (result.PaginationToken != null);
        }

        internal async Task<QueryEntityResponse<TEntity>> QueryAsync<TEntity>(string tableName, BuilderNode node, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            using var httpContent = new QueryHighLevelHttpContent(tableName, Config.TableNamePrefix, Config.Metadata, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            return await ReadAsync<QueryEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }
    }
}