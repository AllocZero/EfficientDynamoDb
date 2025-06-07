using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Operations.BatchGetItem;
using EfficientDynamoDb.Operations;
using EfficientDynamoDb.Operations.BatchGetItem;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared.Capacity;

namespace EfficientDynamoDb
{
    public partial class DynamoDbContext
    {
        /// <summary>
        /// Creates a builder for BatchGet operation.
        /// </summary>
        /// <returns>BatchGet operation builder.</returns>
        public IBatchGetEntityRequestBuilder BatchGet() => new BatchGetEntityRequestBuilder(this);
        
        internal async Task<OpResult<List<TEntity>>> BatchGetItemListAsync<TEntity>(BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new BatchGetItemHighLevelHttpContent(this, node);

            var apiResult = await Api.SendSafeAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            if (apiResult.Exception is not null)
                return new(apiResult.Exception);
            
            using var response = apiResult.Response!;
            var result = await ReadAsync<BatchGetItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            
            List<TEntity>? items = null;
            ExtractItems(ref items, result.Responses);

            var attempt = 0;
            while (result.UnprocessedKeys?.Count > 0)
            {
                if (!Config.RetryStrategies.ProvisionedThroughputExceededStrategy.TryGetRetryDelay(attempt++, out var delay))
                    return new(new DdbException($"Maximum number of {attempt} attempts exceeded while executing batch read item request."));

                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                using var unprocessedHttpContent = new BatchGetItemHttpContent(new BatchGetItemRequest {RequestItems = result.UnprocessedKeys}, null);

                var unprocessedApiResult = await Api.SendSafeAsync(Config, unprocessedHttpContent, cancellationToken).ConfigureAwait(false);
                if (unprocessedApiResult.Exception is not null)
                    return new(unprocessedApiResult.Exception);
                
                using var unprocessedResponse = unprocessedApiResult.Response!;
                result = await ReadAsync<BatchGetItemEntityResponse<TEntity>>(unprocessedResponse, cancellationToken).ConfigureAwait(false);

                ExtractItems(ref items, result.Responses);
            }

            return new(items ?? new List<TEntity>());
        }
        
        internal async Task<OpResult<BatchGetItemResponse<TEntity>>> BatchGetItemResponseAsync<TEntity>(BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new BatchGetItemHighLevelHttpContent(this, node);
            var apiResult = await Api.SendSafeAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            if (apiResult.Exception is not null)
                return new(apiResult.Exception);

            using var response = apiResult.Response!;
            var result = await ReadAsync<BatchGetItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
            
            List<TEntity>? items = null;
            ExtractItems(ref items, result.Responses);
            var totalConsumedCapacity = result.ConsumedCapacity;
            
            var attempt = 0;
            while (result.UnprocessedKeys?.Count > 0)
            {
                if (!Config.RetryStrategies.ProvisionedThroughputExceededStrategy.TryGetRetryDelay(attempt++, out var delay))
                    break;

                await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                using var unprocessedHttpContent = new BatchGetItemHttpContent(new BatchGetItemRequest {RequestItems = result.UnprocessedKeys}, null);
                
                var unprocessedApiResult = await Api.SendSafeAsync(Config, unprocessedHttpContent, cancellationToken).ConfigureAwait(false);
                if (unprocessedApiResult.Exception is not null)
                    return new(unprocessedApiResult.Exception);
                
                using var unprocessedResponse = unprocessedApiResult.Response!;
                result = await ReadAsync<BatchGetItemEntityResponse<TEntity>>(unprocessedResponse, cancellationToken).ConfigureAwait(false);
                
                ExtractItems(ref items, result.Responses);
                MergeTableCapacities(ref totalConsumedCapacity, result.ConsumedCapacity);
            }

            return new(new BatchGetItemResponse<TEntity>(
                totalConsumedCapacity,
                items ?? (IReadOnlyList<TEntity>)Array.Empty<TEntity>(),
                result.UnprocessedKeys)
            );
        }

        private static void ExtractItems<TEntity>(ref List<TEntity>? items, IReadOnlyDictionary<string, List<TEntity>>? responses)
        {
            if (responses == null || responses.Count == 0)
                return;

            foreach (var values in responses.Values)
            {
                if (items == null)
                    items = values;
                else
                    items.AddRange(values);
            }
        }

        private static void MergeTableCapacities(ref List<TableConsumedCapacity>? total, List<TableConsumedCapacity>? current)
        {
            if (current == null)
                return;
            if (total == null || total.Count == 0)
            {
                total = current;
                return;
            }
            
            // It's ok to use O(n^2) here because:
            // - this method is called only when there are unprocessed keys
            // - transforming total to dictionary and back is slower until there are more than ~20 tables in the request (assuming single retry).
            foreach (var currentCapacity in current)
            {
                var idx = total.FindIndex(x => x.TableName == currentCapacity.TableName);
                if (idx >= 0)
                    total[idx].CapacityUnits += currentCapacity.CapacityUnits;
                else
                    total.Add(currentCapacity);
            }
        }
    }
}