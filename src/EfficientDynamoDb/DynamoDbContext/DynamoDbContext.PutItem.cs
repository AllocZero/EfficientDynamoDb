using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Operations.PutItem;
using EfficientDynamoDb.Operations;
using EfficientDynamoDb.Operations.PutItem;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb
{
    public partial class DynamoDbContext
    {
        /// <summary>
        /// Creates a builder for PutItem operation.
        /// </summary>
        /// <returns>PutItem operation builder.</returns>
        public IPutItemRequestBuilder PutItem() => new PutItemRequestBuilder(this);

        /// <summary>
        /// Executes PutItem operation asynchronously.
        /// </summary>
        /// <param name="item">Item to save.</param>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <returns>Task that represents asynchronous operation.</returns>
        public Task PutItemAsync<TEntity>(TEntity item, CancellationToken cancellationToken = default) where TEntity : class
        {
            return PutItem().WithItem(item).ExecuteAsync(cancellationToken);
        }
        
        internal async Task<OpResult<PutItemEntityResponse<TEntity>>> PutItemResponseAsync<TEntity>(BuilderNode? node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new PutItemHighLevelHttpContent(this, node);

            var apiResult = await Api.SendSafeAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            if (apiResult.Exception is not null)
                return new(apiResult.Exception);

            using var response = apiResult.Response!;
            var result = await ReadAsync<PutItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            return new(result);
        }
        
        internal async Task<OpResult<TEntity?>> PutItemAsync<TEntity>(BuilderNode? node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new PutItemHighLevelHttpContent(this, node);

            var apiResult = await Api.SendSafeAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            if (apiResult.Exception is not null)
                return new(apiResult.Exception);

            using var response = apiResult.Response!;
            var result = await ReadAsync<PutItemEntityProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            return new(result.Item);
        }
    }
}