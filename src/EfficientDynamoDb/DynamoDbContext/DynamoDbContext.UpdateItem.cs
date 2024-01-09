using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.UpdateItem;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.UpdateItem;

namespace EfficientDynamoDb
{
    public partial class DynamoDbContext
    {
        /// <summary>
        /// Creates a builder for UpdateItem operation.
        /// </summary>
        /// <typeparam name="TEntity">Type of the DB entity.</typeparam>
        /// <returns>Update operation builder.</returns>
        public IUpdateEntityRequestBuilder<TEntity> UpdateItem<TEntity>() where TEntity : class => new UpdateEntityRequestBuilder<TEntity>(this);
        
        internal async Task<UpdateItemEntityResponse<TEntity>> UpdateItemResponseAsync<TEntity>(DdbClassInfo classInfo, BuilderNode? node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new UpdateItemHighLevelHttpContent(this, classInfo, node);

            using var response = await Api.SendAsync(httpContent, cancellationToken).ConfigureAwait(false);

            return await ReadAsync<UpdateItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }
        
        internal async Task<TEntity?> UpdateItemAsync<TEntity>(DdbClassInfo classInfo, BuilderNode? node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new UpdateItemHighLevelHttpContent(this, classInfo, node);

            using var response = await Api.SendAsync(httpContent, cancellationToken).ConfigureAwait(false);

            var result =  await ReadAsync<UpdateItemEntityProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            return result.Item;
        }
    }
}