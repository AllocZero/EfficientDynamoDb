using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.DeleteItem;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.DeleteItem;

namespace EfficientDynamoDb.Context
{
    public partial class DynamoDbContext
    {
        public IDeleteItemRequestBuilder<TEntity> DeleteItem<TEntity>() where TEntity : class => new DeleteItemRequestBuilder<TEntity>(this);
        
        internal async Task<DeleteItemEntityResponse<TEntity>> DeleteItemResponseAsync<TEntity>(DdbClassInfo classInfo, BuilderNode node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new DeleteItemHighLevelHttpContent(this, classInfo, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);

            return await ReadAsync<DeleteItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }
        
        internal async Task<TEntity?> DeleteItemAsync<TEntity>(DdbClassInfo classInfo, BuilderNode node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new DeleteItemHighLevelHttpContent(this, classInfo, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);

            var result = await ReadAsync<DeleteItemEntityProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            return result.Attributes;
        }
    }
}