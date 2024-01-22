using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Operations.BatchWriteItem;
using EfficientDynamoDb.Operations.BatchWriteItem;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb
{
    public partial class DynamoDbContext
    {
        /// <summary>
        /// Creates a builder for BatchWrite operation.
        /// </summary>
        /// <returns>BatchWrite operation builder.</returns>
        public IBatchWriteItemRequestBuilder BatchWrite() => new BatchWriteItemRequestBuilder(this);
        
        internal async Task BatchWriteItemAsync(BuilderNode node, CancellationToken cancellationToken = default)
        {
            using var httpContent = new BatchWriteItemHighLevelHttpContent(this, node, Config.TableNamePrefix);

            using var response = await Api.SendAsync(httpContent, cancellationToken).ConfigureAwait(false);
            var documentResult = await DynamoDbLowLevelContext.ReadDocumentAsync(response, BatchWriteItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            var attempt = 0;
            while (documentResult != null)
            {
                var unprocessedItems = BatchWriteItemResponseParser.ParseFailedItems(documentResult);
                if (unprocessedItems == null || unprocessedItems.Count == 0)
                    break;

                if (!Config.RetryStrategies.ProvisionedThroughputExceededStrategy.TryGetRetryDelay(attempt++, out var delay))
                    throw new DdbException($"Maximum number of {attempt} attempts exceeded while executing batch write item request.");

                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                using var unprocessedHttpContent = new BatchWriteItemHttpContent(new BatchWriteItemRequest{RequestItems = unprocessedItems}, null);
            
                using var unprocessedResponse = await Api.SendAsync(unprocessedHttpContent, cancellationToken).ConfigureAwait(false);
                documentResult = await DynamoDbLowLevelContext.ReadDocumentAsync(unprocessedResponse, BatchWriteItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}