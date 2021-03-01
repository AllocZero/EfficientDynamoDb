using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.TransactGetItems;
using EfficientDynamoDb.Internal.Operations.TransactGetItems;

namespace EfficientDynamoDb.Context
{
    public partial class DynamoDbContext
    {
        public ITransactGetItemsRequestBuilder TransactGetItems() => new TransactGetItemsRequestBuilder(this);
        
        internal async Task<List<TEntity?>> TransactGetItemsAsync<TEntity>(BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new TransactGetItemsHighLevelHttpContent(this, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<TransactGetItemsEntityProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            var entities = new List<TEntity?>(result.Responses.Count);
            foreach (var item in result.Responses)
                entities.Add(item.Item);

            return entities;
        }
        
        internal async Task<TransactGetItemsEntityResponse<TEntity>> TransactGetItemsResponseAsync<TEntity>(BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new TransactGetItemsHighLevelHttpContent(this, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            return await ReadAsync<TransactGetItemsEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }
    }
}