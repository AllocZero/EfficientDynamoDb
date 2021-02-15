using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.GetItem;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.GetItem;

namespace EfficientDynamoDb.Context
{
    public partial class DynamoDbContext
    {
        public IGetItemRequestBuilder<TEntity> GetItem<TEntity>() where TEntity : class => new GetItemRequestBuilder<TEntity>(this);
        
        public Task<TEntity?> GetItemAsync<TEntity, TPartitionKey>(TPartitionKey partitionKey, CancellationToken cancellationToken = default)
            where TEntity : class => GetItemAsync<TEntity>(Config.Metadata.GetOrAddClassInfo(typeof(TEntity)), new PartitionKeyNode<TPartitionKey>(partitionKey, null), cancellationToken);

        public Task<TEntity?> GetItemAsync<TEntity, TPartitionKey, TSortKey>(TPartitionKey partitionKey, TSortKey sortKey,
            CancellationToken cancellationToken = default) where TEntity : class =>
            GetItemAsync<TEntity>(Config.Metadata.GetOrAddClassInfo(typeof(TEntity)), new PartitionAndSortKeyNode<TPartitionKey, TSortKey>(partitionKey, sortKey, null), cancellationToken);
        
        internal async Task<TEntity?> GetItemAsync<TEntity>(DdbClassInfo classInfo, BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new GetItemHighLevelHttpContent(classInfo, Config.TableNamePrefix, Config.Metadata, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<GetItemEntityProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return result.Item;
        }
        
        internal async Task<GetItemEntityResponse<TEntity>> GetItemResponseAsync<TEntity>(DdbClassInfo classInfo, BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new GetItemHighLevelHttpContent(classInfo, Config.TableNamePrefix, Config.Metadata, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            return await ReadAsync<GetItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }
    }
}