using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.BatchGetItem;
using EfficientDynamoDb.Context.Operations.BatchWriteItem;
using EfficientDynamoDb.Context.Operations.GetItem;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.Scan;
using EfficientDynamoDb.Context.Operations.Shared;
using EfficientDynamoDb.Context.Operations.UpdateItem;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.BatchGetItem;
using EfficientDynamoDb.Internal.Operations.BatchWriteItem;
using EfficientDynamoDb.Internal.Operations.GetItem;
using EfficientDynamoDb.Internal.Operations.PutItem;
using EfficientDynamoDb.Internal.Operations.Query;
using EfficientDynamoDb.Internal.Operations.Scan;
using EfficientDynamoDb.Internal.Operations.UpdateItem;
using EfficientDynamoDb.Internal.Reader;
using static EfficientDynamoDb.Context.DynamoDbLowLevelContext;

namespace EfficientDynamoDb.Context
{
    public class DynamoDbContext
    {
        internal DynamoDbContextConfig Config => LowContext.Config;
        private HttpApi Api => LowContext.Api;
        
        public DynamoDbLowLevelContext LowContext { get; }

        public DynamoDbContext(DynamoDbContextConfig config)
        {
            LowContext = new DynamoDbLowLevelContext(config, new HttpApi(config.HttpClientFactory));
        }

        public IPutItemRequestBuilder PutItem() => new PutItemRequestBuilder(this);
        
        public async Task PutItemAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            await PutItemAsync<T>(new ItemNode(entity, Config.Metadata.GetOrAddClassInfo(typeof(T)), null), cancellationToken).ConfigureAwait(false);
        }

        public Task<TEntity?> GetItemAsync<TEntity, TPartitionKey>(TPartitionKey partitionKey, CancellationToken cancellationToken = default)
            where TEntity : class => GetItemAsync<TEntity>(Config.Metadata.GetOrAddClassInfo(typeof(TEntity)), new PartitionKeyNode<TPartitionKey>(partitionKey, null), cancellationToken);

        public Task<TEntity?> GetItemAsync<TEntity, TPartitionKey, TSortKey>(TPartitionKey partitionKey, TSortKey sortKey,
            CancellationToken cancellationToken = default) where TEntity : class =>
            GetItemAsync<TEntity>(Config.Metadata.GetOrAddClassInfo(typeof(TEntity)), new PartitionAndSortKeyNode<TPartitionKey, TSortKey>(partitionKey, sortKey, null), cancellationToken);

        public IQueryRequestBuilder<TEntity> Query<TEntity>() where TEntity : class => new QueryRequestBuilder<TEntity>(this);
        
        public IScanRequestBuilder<TEntity> Scan<TEntity>() where TEntity : class => new ScanRequestBuilder<TEntity>(this);
        
        public IGetItemRequestBuilder<TEntity> GetItem<TEntity>() where TEntity : class => new GetItemRequestBuilder<TEntity>(this);

        public IUpdateRequestBuilder<TEntity> Update<TEntity>() where TEntity : class => new UpdateRequestBuilder<TEntity>(this);
        
        public IBatchGetRequestBuilder BatchGetItem() => new BatchGetRequestBuilder(this);
        
        public IBatchWriteItemRequestBuilder BatchWriteItem() => new BatchWriteItemRequestBuilder(this);
        
        public T ToObject<T>(Document document) where T : class => document.ToObject<T>(Config.Metadata);

        public Document ToDocument<T>(T entity) where T : class => entity.ToDocument(Config.Metadata);
        
        internal async Task<TEntity?> GetItemAsync<TEntity>(DdbClassInfo classInfo, BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new GetItemHighLevelHttpContent(classInfo, Config.TableNamePrefix, Config.Metadata, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<GetItemEntityProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return result.Item;
        }
        
        internal async Task<GetItemEntityResponse<TEntity>> GetItemResponseAsync<TEntity>(DdbClassInfo classInfo, BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new GetItemHighLevelHttpContent(classInfo, Config.TableNamePrefix, Config.Metadata, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            return await ReadAsync<GetItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }

        internal async Task<IReadOnlyList<TEntity>> QueryListAsync<TEntity>(string tableName, BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            QueryEntityResponseProjection<TEntity>? result = null;
            List<TEntity>? items = null;

            // Does not reuse QueryAsyncEnumerable because of potential allocations
            var isFirst = true;
            do
            {
                var contentNode = isFirst ? node : new PaginationTokenNode(result?.PaginationToken, node);
                using var httpContent = new QueryHighLevelHttpContent(tableName, Config.TableNamePrefix, Config.Metadata, contentNode);

                using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
                result = await ReadAsync<QueryEntityResponseProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

                if (items == null)
                    items = result.Items;
                else
                    items.AddRange(result.Items);

                isFirst = false;
            } while (result.PaginationToken != null);

            return items;
        }
        
        internal async Task<PagedResult<TEntity>> QueryPageAsync<TEntity>(string tableName, BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new QueryHighLevelHttpContent(tableName, Config.TableNamePrefix, Config.Metadata, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<QueryEntityResponseProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return new PagedResult<TEntity>(result.Items, result.PaginationToken);
        }
        
        internal async Task<PagedResult<TEntity>> ScanPageAsync<TEntity>(string tableName, BuilderNode? node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new ScanHighLevelHttpContent(tableName, Config.TableNamePrefix, Config.Metadata, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<ScanEntityResponseProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return new PagedResult<TEntity>(result.Items, result.PaginationToken);
        }
        
        internal async IAsyncEnumerable<IReadOnlyList<TEntity>> QueryAsyncEnumerable<TEntity>(string tableName, BuilderNode node, [EnumeratorCancellation] CancellationToken cancellationToken = default) where TEntity : class
        {
            QueryEntityResponseProjection<TEntity>? result = null;

            var isFirst = true;
            do
            {
                var contentNode = isFirst ? node : new PaginationTokenNode(result?.PaginationToken, node);
                using var httpContent = new QueryHighLevelHttpContent(tableName, Config.TableNamePrefix, Config.Metadata, contentNode);

                using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
                result = await ReadAsync<QueryEntityResponseProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

                yield return result.Items;

                isFirst = false;
            } while (result.PaginationToken != null);
        }
        
        internal async IAsyncEnumerable<IReadOnlyList<TEntity>> ScanAsyncEnumerable<TEntity>(string tableName, BuilderNode? node, [EnumeratorCancellation] CancellationToken cancellationToken = default) where TEntity : class
        {
            ScanEntityResponseProjection<TEntity>? result = null;

            var isFirst = true;
            do
            {
                var contentNode = isFirst ? node : new PaginationTokenNode(result?.PaginationToken, node);
                using var httpContent = new ScanHighLevelHttpContent(tableName, Config.TableNamePrefix, Config.Metadata, contentNode);

                using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
                result = await ReadAsync<ScanEntityResponseProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

                yield return result.Items;

                isFirst = false;
            } while (result.PaginationToken != null);
        }

        internal async IAsyncEnumerable<IReadOnlyList<TEntity>> ParallelScanAsyncEnumerable<TEntity>(string tableName, BuilderNode? node, int totalSegments,
            [EnumeratorCancellation] CancellationToken cancellationToken = default) where TEntity : class
        {
            var totalSegmentsNode = new TotalSegmentsNode(totalSegments, node);

            var tasks = new List<Task<(int Segment, PagedResult<TEntity> Page)>>(totalSegments);
            for (var segment = 0; segment < totalSegments; segment++)
                tasks.Add(ScanSegmentAsync(tableName, totalSegmentsNode, segment, cancellationToken));

            try
            {
                while (tasks.Count != 0)
                {
                    var finishedSegmentTask = await Task.WhenAny(tasks).ConfigureAwait(false);
                    var finishedSegment = await finishedSegmentTask.ConfigureAwait(false);
                    if (finishedSegment.Page.PaginationToken == null)
                    {
                        tasks.Remove(finishedSegmentTask);
                    }
                    else
                    {
                        var finishedIndex = tasks.IndexOf(finishedSegmentTask);
                        tasks[finishedIndex] = ScanSegmentAsync(tableName, new PaginationTokenNode(finishedSegment.Page.PaginationToken, totalSegmentsNode),
                            finishedSegment.Segment,
                            cancellationToken);
                    }

                    yield return finishedSegment.Page.Items;
                }
            }
            finally
            {
                // Make sure all left tasks are awaited in case of disposal/cancellation
                await Task.WhenAll(tasks).ConfigureAwait(false);
            }

            async Task<(int Segment, PagedResult<TEntity> Page)> ScanSegmentAsync(string tableName, BuilderNode node, int segment,
                CancellationToken cancellationToken = default)
            {
                var result = await ScanPageAsync<TEntity>(tableName, new SegmentNode(segment, node), cancellationToken).ConfigureAwait(false);
                return (segment, result);
            }
        }

        internal async Task<QueryEntityResponse<TEntity>> QueryAsync<TEntity>(string tableName, BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new QueryHighLevelHttpContent(tableName, Config.TableNamePrefix, Config.Metadata, node);
            
            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            return await ReadAsync<QueryEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }
        
        internal async Task<ScanEntityResponse<TEntity>> ScanAsync<TEntity>(string tableName, BuilderNode? node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new ScanHighLevelHttpContent(tableName, Config.TableNamePrefix, Config.Metadata, node);
            
            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            return await ReadAsync<ScanEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }

        internal async Task<PutItemEntityResponse<TEntity>> PutItemAsync<TEntity>(BuilderNode? node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new PutItemHighLevelHttpContent(Config.TableNamePrefix, Config.Metadata, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);

            return await ReadAsync<PutItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }
        
        internal async Task<UpdateItemEntityResponse<TEntity>> UpdateItemAsync<TEntity>(BuilderNode? node,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new UpdateItemHighLevelHttpContent<TEntity>(Config.TableNamePrefix, Config.Metadata, node);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);

            return await ReadAsync<UpdateItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }
        
        internal async Task<List<TEntity>> BatchGetItemAsync<TEntity>(BuilderNode node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new BatchGetItemHighLevelHttpContent(node, Config.TableNamePrefix, Config.Metadata);

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
                using var unprocessedHttpContent = new BatchGetItemHttpContent(new BatchGetItemRequest{RequestItems = result.UnprocessedKeys}, Config.TableNamePrefix);
            
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
        
        internal async Task BatchWriteItemAsync(BuilderNode node, CancellationToken cancellationToken = default)
        {
            using var httpContent = new BatchWriteItemHighLevelHttpContent(node, Config.TableNamePrefix);

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
                documentResult = await ReadDocumentAsync(response, BatchWriteItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);
            }
        }

        private async ValueTask<TResult> ReadAsync<TResult>(HttpResponseMessage response, CancellationToken cancellationToken = default) where TResult : class
        {
            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            var expectedCrc = GetExpectedCrc(response);
            var result = await EntityDdbJsonReader.ReadAsync<TResult>(responseStream, Config.Metadata, expectedCrc.HasValue, cancellationToken).ConfigureAwait(false);
            
            if (expectedCrc.HasValue && expectedCrc.Value != result.Crc)
                throw new ChecksumMismatchException();

            return result.Value!;
        }
    }
}