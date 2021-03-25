using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.BatchWriteItem;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Operations.BatchWriteItem;

namespace EfficientDynamoDb.Context
{
    public partial class DynamoDbContext
    {
        public IBatchWriteItemRequestBuilder BatchWrite() => new BatchWriteItemRequestBuilder(this);
        
        internal async Task BatchWriteItemAsync(BuilderNode node, CancellationToken cancellationToken = default)
        {
            using var httpContent = new BatchWriteItemHighLevelHttpContent(this, node, Config.TableNamePrefix);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
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
                using var unprocessedHttpContent = new BatchWriteItemHttpContent(new BatchWriteItemRequest{RequestItems = unprocessedItems}, Config.TableNamePrefix);
            
                using var unprocessedResponse = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
                documentResult = await DynamoDbLowLevelContext.ReadDocumentAsync(response, BatchWriteItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);
            }
        }
    }
}