using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.UpdateItem;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.UpdateItem;

namespace EfficientDynamoDb.Context
{
    public partial class DynamoDbContext
    {
        public IUpdateEntityRequestBuilder<TEntity> Update<TEntity>() where TEntity : class => new UpdateEntityRequestBuilder<TEntity>(this);
        
        internal async Task<UpdateItemEntityResponse<TEntity>> UpdateItemResponseAsync<TEntity>(DdbClassInfo classInfo, BuilderNode? node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new UpdateItemHighLevelHttpContent(this, classInfo, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);

            return await ReadAsync<UpdateItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }
        
        internal async Task<TEntity?> UpdateItemAsync<TEntity>(DdbClassInfo classInfo, BuilderNode? node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new UpdateItemHighLevelHttpContent(this, classInfo, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);

            var result =  await ReadAsync<UpdateItemEntityProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            return result.Item;
        }
    }
}