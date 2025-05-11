using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Operations.BatchWriteItem;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.BatchWriteItem;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;
using EfficientDynamoDb.Operations.Shared.Capacity;

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
            using var httpContent = new BatchWriteItemHighLevelHttpContent(this, node, Config.TableNameFormatter);

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
                using var unprocessedHttpContent = new BatchWriteItemHttpContent(new BatchWriteItemRequest{RequestItems = unprocessedItems}, null);
            
                using var unprocessedResponse = await Api.SendAsync(Config, unprocessedHttpContent, cancellationToken).ConfigureAwait(false);
                documentResult = await DynamoDbLowLevelContext.ReadDocumentAsync(unprocessedResponse, BatchWriteItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);
            }
        }
        
        internal async Task<BatchWriteItemResponse> BatchWriteItemResponseAsync(BuilderNode node, CancellationToken cancellationToken = default)
        {
            using var httpContent = new BatchWriteItemHighLevelHttpContent(this, node, Config.TableNameFormatter);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var documentResult = await DynamoDbLowLevelContext.ReadDocumentAsync(response, BatchWriteItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);
            if (documentResult == null)
                return new BatchWriteItemResponse(null, null, null);

            var unprocessedItems = BatchWriteItemResponseParser.ParseFailedItems(documentResult);
            var consumedCapacity = CapacityParser.ParseFullConsumedCapacities(documentResult);
            var itemCollectionMetrics = ItemCollectionMetricsParser.ParseMultipleItemCollectionMetrics(documentResult);

            var attempt = 0;
            while (unprocessedItems != null && unprocessedItems.Count > 0)
            {
                if (!Config.RetryStrategies.ProvisionedThroughputExceededStrategy.TryGetRetryDelay(attempt++, out var delay))
                    throw new DdbException($"Maximum number of {attempt} attempts exceeded while executing batch write item request.");

                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                using var unprocessedHttpContent = new BatchWriteItemHttpContent(new BatchWriteItemRequest{RequestItems = unprocessedItems}, null);
            
                using var unprocessedResponse = await Api.SendAsync(Config, unprocessedHttpContent, cancellationToken).ConfigureAwait(false);
                documentResult = await DynamoDbLowLevelContext.ReadDocumentAsync(unprocessedResponse, BatchWriteItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);
                if (documentResult == null)
                    break;
                
                unprocessedItems = BatchWriteItemResponseParser.ParseFailedItems(documentResult);
                MergeFullCapacities(ref consumedCapacity, CapacityParser.ParseFullConsumedCapacities(documentResult));
                MergeItemCollectionMetrics(ref itemCollectionMetrics, ItemCollectionMetricsParser.ParseMultipleItemCollectionMetrics(documentResult));
            }

            return new BatchWriteItemResponse(consumedCapacity, itemCollectionMetrics, unprocessedItems);
        }

        private static void MergeFullCapacities(ref List<FullConsumedCapacity>? total, List<FullConsumedCapacity>? current)
        {
            if (current == null)
                return;

            if (total == null)
            {
                total = current;
                return;
            }

            foreach (var capacity in current)
            {
                var idx = total.FindIndex(x => x.TableName == capacity.TableName);
                if (idx == -1)
                    total.Add(capacity);
                else
                {
                    var totalCapacity = total[idx];
                    totalCapacity.CapacityUnits += capacity.CapacityUnits;
                    totalCapacity.GlobalSecondaryIndexes = MergeIndexConsumedCapacities(totalCapacity.GlobalSecondaryIndexes, capacity.GlobalSecondaryIndexes);
                    totalCapacity.LocalSecondaryIndexes = MergeIndexConsumedCapacities(totalCapacity.LocalSecondaryIndexes, capacity.LocalSecondaryIndexes);
                }
            }
        }

        private static IReadOnlyDictionary<string, ConsumedCapacity>? MergeIndexConsumedCapacities(IReadOnlyDictionary<string, ConsumedCapacity>? total,
            IReadOnlyDictionary<string, ConsumedCapacity>? current)
        {
            if (current == null)
                return total;
            if (total == null)
                return current;
            
            var tempDict = total.ToDictionary(x => x.Key, x => x.Value);
            foreach (var capacity in current)
            {
                if (tempDict.TryGetValue(capacity.Key, out var totalCapacity))
                    totalCapacity.CapacityUnits += capacity.Value.CapacityUnits;
                else
                {
                    tempDict[capacity.Key] = capacity.Value;
                }
            }

            return tempDict;
        }
        
        private static void MergeItemCollectionMetrics(ref Dictionary<string, ItemCollectionMetrics>? total, Dictionary<string, ItemCollectionMetrics>? current)
        {
            if (current == null)
                return;
            if (total == null)
            {
                total = current;
                return;
            }
            
            foreach (var metrics in total)
            {
                total[metrics.Key] = metrics.Value;
            }
        }
    }
}