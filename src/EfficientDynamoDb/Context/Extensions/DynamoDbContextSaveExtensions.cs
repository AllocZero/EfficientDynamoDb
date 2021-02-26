using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.UpdateItem;
using EfficientDynamoDb.Internal.Operations.UpdateItem;

namespace EfficientDynamoDb.Context.Extensions
{
    public static class DynamoDbContextSaveExtensions
    {
        public static async Task SaveAsync<TEntity>(this DynamoDbContext context, TEntity entity, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new UpdateItemSaveHttpContent<TEntity>(context, entity);

            await context.ExecuteAsync<object>(httpContent, cancellationToken).ConfigureAwait(false);
        }
    }
}