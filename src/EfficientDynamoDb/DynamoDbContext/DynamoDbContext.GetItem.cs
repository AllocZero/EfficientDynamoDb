using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.GetItem;
using EfficientDynamoDb.Operations.GetItem;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb
{
    public partial class DynamoDbContext
    {
        /// <summary>
        /// Creates a builder for GetItem operation.
        /// </summary>
        /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
        /// <returns>GetItem operation builder.</returns>
        public IGetItemEntityRequestBuilder<TEntity> GetItem<TEntity>() where TEntity : class => new GetItemEntityRequestBuilder<TEntity>(this);

        /// <summary>
        /// Executes GetItem operation asynchronously.
        /// </summary>
        /// <param name="partitionKey">Partition key of the item to get.</param>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <typeparam name="TEntity">Type of the entity.</typeparam>
        /// <returns>Task that represents asynchronous operation.</returns>
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// If the table has both partition and sort keys, use <see cref="GetItemAsync{TEntity}(object,object,System.Threading.CancellationToken)"/> instead.
        /// </remarks>
        public async Task<TEntity?> GetItemAsync<TEntity>(object partitionKey, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            using var httpContent = new GetItemByPkObjectHttpContent<TEntity>(this, partitionKey);

            using var response = await Api.SendAsync(httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<GetItemEntityProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return result.Item;
        }
        
        /// <summary>
        /// Executes GetItem operation asynchronously.
        /// </summary>
        /// <param name="partitionKey">Partition key of the item to get.</param>
        /// <param name="sortKey">Sort key of the item to get.</param>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <typeparam name="TEntity">Type of the entity.</typeparam>
        /// <returns>Task that represents asynchronous operation.</returns>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// If the table has only a partition key, use <see cref="GetItemAsync{TEntity}(object,System.Threading.CancellationToken)"/> instead.
        /// </remarks>
        public async Task<TEntity?> GetItemAsync<TEntity>(object partitionKey, object sortKey, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            using var httpContent = new GetItemByPkAndSkObjectHttpContent<TEntity>(this, partitionKey, sortKey);

            using var response = await Api.SendAsync(httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<GetItemEntityProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return result.Item;
        }

        /// <summary>
        /// Executes GetItem operation asynchronously.
        /// </summary>
        /// <param name="partitionKey">Partition key of the item to get.</param>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <typeparam name="TEntity">Type of the entity.</typeparam>
        /// <typeparam name="TPartitionKey">Type of the partition key.</typeparam>
        /// <returns>Task that represents asynchronous operation.</returns>
        /// <remarks>
        /// This method should be used only if the table has only partition key.
        /// If the table has both partition and sort keys, use <see cref="GetItemAsync{TEntity,TPartitionKey,TSortKey}(TPartitionKey,TSortKey,System.Threading.CancellationToken)"/> instead.
        /// </remarks>
        public Task<TEntity?> GetItemAsync<TEntity, TPartitionKey>(TPartitionKey partitionKey, CancellationToken cancellationToken = default)
            where TEntity : class => GetItemAsync<TEntity>(Config.Metadata.GetOrAddClassInfo(typeof(TEntity)), new PartitionKeyNode<TPartitionKey>(partitionKey, null), cancellationToken);

        /// <summary>
        /// Executes GetItem operation asynchronously.
        /// </summary>
        /// <param name="partitionKey">Partition key of the item to get.</param>
        /// <param name="sortKey">Sort key of the item to get.</param>
        /// <param name="cancellationToken">Token that can be used to cancel the task.</param>
        /// <typeparam name="TEntity">Type of the entity.</typeparam>
        /// <typeparam name="TPartitionKey">Type of the partition key.</typeparam>
        /// <typeparam name="TSortKey">Type of the sort key.</typeparam>
        /// <returns>Task that represents asynchronous operation.</returns>
        /// <remarks>
        /// This method should be used only if the table has both partition and sort keys.
        /// If the table has only a partition key, use <see cref="GetItemAsync{TEntity,TPartitionKey}(TPartitionKey,System.Threading.CancellationToken)"/> instead.
        /// </remarks>
        public Task<TEntity?> GetItemAsync<TEntity, TPartitionKey, TSortKey>(TPartitionKey partitionKey, TSortKey sortKey,
            CancellationToken cancellationToken = default) where TEntity : class =>
            GetItemAsync<TEntity>(Config.Metadata.GetOrAddClassInfo(typeof(TEntity)), new PartitionAndSortKeyNode<TPartitionKey, TSortKey>(partitionKey, sortKey, null), cancellationToken);
        
        internal async Task<TEntity?> GetItemAsync<TEntity>(DdbClassInfo classInfo, BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new GetItemHighLevelHttpContent(this, classInfo, node);

            using var response = await Api.SendAsync(httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<GetItemEntityProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return result.Item;
        }
        
        internal async Task<GetItemEntityResponse<TEntity>> GetItemResponseAsync<TEntity>(DdbClassInfo classInfo, BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new GetItemHighLevelHttpContent(this, classInfo, node);

            using var response = await Api.SendAsync(httpContent, cancellationToken).ConfigureAwait(false);
            return await ReadAsync<GetItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }
    }
}