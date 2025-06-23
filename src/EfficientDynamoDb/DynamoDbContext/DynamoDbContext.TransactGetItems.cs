using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal.Operations.TransactGetItems;
using EfficientDynamoDb.Operations;
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
        
        internal async Task<OpResult<List<TEntity?>>> TransactGetItemsAsync<TEntity>(BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new TransactGetItemsHighLevelHttpContent(this, node);

            var apiResult = await Api.SendSafeAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            if (apiResult.Exception is not null)
                return new(apiResult.Exception);
            
            using var response = apiResult.Response!;
            var result = await ReadAsync<TransactGetItemsEntityProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            var entities = new List<TEntity?>(result.Responses.Count);
            foreach (var item in result.Responses)
                entities.Add(item.Item);

            return new(entities);
        }
        
        internal async Task<OpResult<TransactGetItemsEntityResponse<TEntity>>> TransactGetItemsResponseAsync<TEntity>(BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new TransactGetItemsHighLevelHttpContent(this, node);

            var apiResult = await Api.SendSafeAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            if (apiResult.Exception is not null)
                return new(apiResult.Exception);
            
            using var response = apiResult.Response!;
            var result = await ReadAsync<TransactGetItemsEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            return new(result);
        }
    }
}