using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Operations.DeleteItem;
using EfficientDynamoDb.Internal.Operations.UpdateItem;

namespace EfficientDynamoDb.Extensions
{
    public static class DynamoDbContextSaveExtensions
    {
        public static async Task DeleteAsync<TEntity>(this IDynamoDbContext context, TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new DeleteEntityHighLevelHttpContent<TEntity>(context.Config, entity);

            await context.ExecuteAsync<object>(httpContent, cancellationToken).ConfigureAwait(false);
        }
        
        public static async Task SaveAsync<TEntity>(this IDynamoDbContext context, TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new UpdateItemSaveHttpContent<TEntity>(context.Config, entity);

            await context.ExecuteAsync<object>(httpContent, cancellationToken).ConfigureAwait(false);
        }
    }
}