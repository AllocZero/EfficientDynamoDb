using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.UpdateItem;
using EfficientDynamoDb.Operations;
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
        
        internal async Task<OpResult<UpdateItemEntityResponse<TEntity>>> UpdateItemResponseAsync<TEntity>(DdbClassInfo classInfo, BuilderNode? node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new UpdateItemHighLevelHttpContent(this, classInfo, node);
            
            var apiResult = await Api.SendSafeAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            if (apiResult.Exception is not null)
                return new(apiResult.Exception);

            using var response = apiResult.Response!;
            var result = await ReadAsync<UpdateItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            return new(result);
        }
        
        internal async Task<OpResult<TEntity?>> UpdateItemAsync<TEntity>(DdbClassInfo classInfo, BuilderNode? node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new UpdateItemHighLevelHttpContent(this, classInfo, node);

            var apiResult = await Api.SendSafeAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            if (apiResult.Exception is not null)
                return new(apiResult.Exception);
            
            using var response = apiResult.Response!;
            var result =  await ReadAsync<UpdateItemEntityProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            return new(result.Item);
        }
    }
}