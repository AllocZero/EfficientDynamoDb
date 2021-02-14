using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.BatchGetItem;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Operations.BatchGetItem;

namespace EfficientDynamoDb.Context
{
    public partial class DynamoDbContext
    {
        public IBatchGetRequestBuilder BatchGetItem() => new BatchGetRequestBuilder(this);
        
        internal async Task<List<TEntity>> BatchGetItemAsync<TEntity>(BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new BatchGetItemHighLevelHttpContent(node, this.Config.TableNamePrefix, Config.Metadata);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<BatchGetItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            List<TEntity>? items = null;
            if (result.Responses?.Count > 0)
            {
                foreach (var values in result.Responses.Values)
                {
                    if (items == null)
                        items = values;
                    else
                        items.AddRange(values);
                }
            }

            var attempt = 0;
            while (result.UnprocessedKeys?.Count > 0)
            {
                if (!Config.RetryStrategies.ProvisionedThroughputExceededStrategy.TryGetRetryDelay(attempt++, out var delay))
                    throw new DdbException($"Maximum number of {attempt} attempts exceeded while executing batch read item request.");

                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                using var unprocessedHttpContent = new BatchGetItemHttpContent(new BatchGetItemRequest {RequestItems = result.UnprocessedKeys}, Config.TableNamePrefix);

                using var unprocessedResponse = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
                result = await ReadAsync<BatchGetItemEntityResponse<TEntity>>(unprocessedResponse, cancellationToken).ConfigureAwait(false);

                if (result.Responses?.Count > 0)
                {
                    foreach (var values in result.Responses.Values)
                    {
                        if (items == null)
                            items = values;
                        else
                            items.AddRange(values);
                    }
                }
            }

            return items ?? new List<TEntity>();
        }
    }
}