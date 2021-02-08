using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.BatchWriteItem;
using EfficientDynamoDb.Context.Operations.GetItem;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.UpdateItem;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.BatchWriteItem;
using EfficientDynamoDb.Internal.Operations.GetItem;
using EfficientDynamoDb.Internal.Operations.PutItem;
using EfficientDynamoDb.Internal.Operations.Query;
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

        public async Task<TEntity?> GetItemAsync<TEntity, TPartitionKey>(TPartitionKey partitionKey, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var classInfo = Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            var request = new GetItemHighLevelRequest<TPartitionKey>(partitionKey) {TableName = classInfo.TableName!};
            using var httpContent = new GetItemHighLevelHttpContent<TPartitionKey>(request, Config.TableNamePrefix,
                (DdbPropertyInfo<TPartitionKey>) classInfo.PartitionKey!);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<GetItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return result.Item;
        }

        public async Task<TEntity?> GetItemAsync<TEntity, TPartitionKey, TSortKey>(TPartitionKey partitionKey, TSortKey sortKey,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            var classInfo = Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            var request = new GetItemHighLevelRequest<TPartitionKey, TSortKey>(partitionKey, sortKey) {TableName = classInfo.TableName!};
            using var httpContent = new GetItemHighLevelHttpContent<TPartitionKey, TSortKey>(request, Config.TableNamePrefix,
                (DdbPropertyInfo<TPartitionKey>) classInfo.PartitionKey!, (DdbPropertyInfo<TSortKey>) classInfo.SortKey!);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<GetItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return result.Item;
        }

        public IQueryRequestBuilder Query() => new QueryRequestBuilder(this);

        public IUpdateRequestBuilder<TEntity> Update<TEntity>() where TEntity : class => new UpdateRequestBuilder<TEntity>(this);
        
        public IBatchWriteItemRequestBuilder BatchWriteItem() => new BatchWriteItemRequestBuilder(this);

        internal async Task<IReadOnlyList<TEntity>> QueryListAsync<TEntity>(string tableName, BuilderNode? node, CancellationToken cancellationToken = default) where TEntity : class
        {
            QueryEntityResponseProjection<TEntity>? result = null;
            List<TEntity>? items = null;

            do
            {
                using var httpContent = new QueryHighLevelHttpContent(tableName, Config.TableNamePrefix, Config.Metadata, new PaginationTokenNode(result?.PaginationToken, node));

                using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
                result = await ReadAsync<QueryEntityResponseProjection<TEntity>>(response, cancellationToken).ConfigureAwait(false);

                if (items == null)
                    items = result.Items;
                else
                    items.AddRange(result.Items);
            } while (result.PaginationToken != null);

            return items;
        }

        internal async Task<QueryEntityResponse<TEntity>> QueryAsync<TEntity>(string tableName, BuilderNode? node, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new QueryHighLevelHttpContent(tableName, Config.TableNamePrefix, Config.Metadata, node);
            
            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            return await ReadAsync<QueryEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
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