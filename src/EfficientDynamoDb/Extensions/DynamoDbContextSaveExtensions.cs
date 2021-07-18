using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Operations.DeleteItem;
using EfficientDynamoDb.Internal.Operations.UpdateItem;

namespace EfficientDynamoDb.Extensions
{
    public static class DynamoDbContextSaveExtensions
    {
        /// <summary>
        /// Compatibility method. Deletes an entity in a way that AWS .NET SDK does.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <remarks>
        /// Check DeleteAsync compatibility section for details:
        /// https://alloczero.github.io/EfficientDynamoDb/docs/dev-guide/sdk-compatibility#operations
        /// </remarks>
        public static async Task DeleteAsync<TEntity>(this IDynamoDbContext context, TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new DeleteEntityHighLevelHttpContent<TEntity>(context.Config, entity);

            await context.ExecuteAsync<object>(httpContent, cancellationToken).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Compatibility method. Saves an entity in a way that AWS .NET SDK does.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <remarks>
        /// Check SaveAsync compatibility section for details:
        /// https://alloczero.github.io/EfficientDynamoDb/docs/dev-guide/sdk-compatibility#operations
        /// </remarks>
        public static async Task SaveAsync<TEntity>(this IDynamoDbContext context, TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new UpdateItemSaveHttpContent<TEntity>(context.Config, entity);

            await context.ExecuteAsync<object>(httpContent, cancellationToken).ConfigureAwait(false);
        }
    }
}