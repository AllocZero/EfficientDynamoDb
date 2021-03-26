using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Operations.PutItem;
using EfficientDynamoDb.Operations.PutItem;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb
{
    public partial class DynamoDbContext
    {
        public IPutItemRequestBuilder PutItem() => new PutItemRequestBuilder(this);

        public Task PutItemAsync<TEntity>(TEntity item, CancellationToken cancellationToken = default) where TEntity : class
        {
            return PutItem().WithItem(item).ExecuteAsync(cancellationToken);
        }
        
        internal async Task<PutItemEntityResponse<TEntity>> PutItemResponseAsync<TEntity>(BuilderNode? node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new PutItemHighLevelHttpContent(this, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);

            return await ReadAsync<PutItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }
        
        internal async Task<TEntity?> PutItemAsync<TEntity>(BuilderNode? node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new PutItemHighLevelHttpContent(this, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);

            var result = await ReadAsync<PutItemEntityProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            return result.Item;
        }
    }
}