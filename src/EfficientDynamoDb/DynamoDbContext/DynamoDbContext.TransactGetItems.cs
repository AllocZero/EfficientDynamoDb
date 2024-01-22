using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Operations.TransactGetItems;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.TransactGetItems;

namespace EfficientDynamoDb
{
    public partial class DynamoDbContext
    {
        /// <summary>
        /// Creates a builder for TransactGet operation.
        /// </summary>
        /// <returns>TransactGet operation builder.</returns>
        public ITransactGetItemsEntityRequestBuilder TransactGet() => new TransactGetItemsEntityRequestBuilder(this);
        
        internal async Task<List<TEntity?>> TransactGetItemsAsync<TEntity>(BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new TransactGetItemsHighLevelHttpContent(this, node);

            using var response = await Api.SendAsync(httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<TransactGetItemsEntityProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            var entities = new List<TEntity?>(result.Responses.Count);
            foreach (var item in result.Responses)
                entities.Add(item.Item);

            return entities;
        }
        
        internal async Task<TransactGetItemsEntityResponse<TEntity>> TransactGetItemsResponseAsync<TEntity>(BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new TransactGetItemsHighLevelHttpContent(this, node);

            using var response = await Api.SendAsync(httpContent, cancellationToken).ConfigureAwait(false);
            return await ReadAsync<TransactGetItemsEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }
    }
}