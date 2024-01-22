using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.DeleteItem;
using EfficientDynamoDb.Operations.DeleteItem;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb
{
    public partial class DynamoDbContext
    {
        /// <summary>
        /// Creates a builder for DeleteItem operation.
        /// </summary>
        /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
        /// <returns>DeleteItem operation builder.</returns>
        public IDeleteItemEntityRequestBuilder<TEntity> DeleteItem<TEntity>() where TEntity : class => new DeleteItemEntityRequestBuilder<TEntity>(this);

        /// <summary>
        /// Executes DeleteItem operation asynchronously.
        /// </summary>
        /// <param name="partitionKey">Partition key of the item to delete.</param>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <typeparam name="TEntity">Type of the entity.</typeparam>
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// Otherwise, use <see cref="DeleteItemAsync{TEntity}(object,object,System.Threading.CancellationToken)"/> instead.
        /// </remarks>
        public async Task DeleteItemAsync<TEntity>(object partitionKey, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new DeleteItemByPkObjectHttpContent<TEntity>(this, partitionKey);

            using var response = await Api.SendAsync(httpContent, cancellationToken).ConfigureAwait(false);

            await ReadAsync<object>(response, cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Executes DeleteItem operation asynchronously.
        /// </summary>
        /// <param name="partitionKey">Partition key of the item to delete.</param>
        /// <param name="sortKey">Sort key of the item to delete.</param>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <typeparam name="TEntity">Type of the entity.</typeparam>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// Otherwise, use <see cref="DeleteItemAsync{TEntity}(object,System.Threading.CancellationToken)"/> instead.
        /// </remarks>
        public async Task DeleteItemAsync<TEntity>(object partitionKey, object sortKey, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new DeleteItemByPkAndSkObjectHttpContent<TEntity>(this, partitionKey, sortKey);

            using var response = await Api.SendAsync(httpContent, cancellationToken).ConfigureAwait(false);

            await ReadAsync<object>(response, cancellationToken).ConfigureAwait(false);
        }
        
        internal async Task<DeleteItemEntityResponse<TEntity>> DeleteItemResponseAsync<TEntity>(DdbClassInfo classInfo, BuilderNode node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new DeleteItemHighLevelHttpContent(this, classInfo, node);

            using var response = await Api.SendAsync(httpContent, cancellationToken).ConfigureAwait(false);

            return await ReadAsync<DeleteItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }
        
        internal async Task<TEntity?> DeleteItemAsync<TEntity>(DdbClassInfo classInfo, BuilderNode node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new DeleteItemHighLevelHttpContent(this, classInfo, node);

            using var response = await Api.SendAsync(httpContent, cancellationToken).ConfigureAwait(false);

            var result = await ReadAsync<DeleteItemEntityProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            return result.Attributes;
        }
        
        internal async Task DeleteItemAsync(DdbClassInfo classInfo, BuilderNode node, CancellationToken cancellationToken = default)
        {
            using var httpContent = new DeleteItemHighLevelHttpContent(this, classInfo, node);

            using var response = await Api.SendAsync(httpContent, cancellationToken).ConfigureAwait(false);

            await ReadAsync<object>(response, cancellationToken).ConfigureAwait(false);
        }
    }
}