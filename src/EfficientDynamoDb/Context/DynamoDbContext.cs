using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.BatchGetItem;
using EfficientDynamoDb.Context.Operations.BatchWriteItem;
using EfficientDynamoDb.Context.Operations.DeleteItem;
using EfficientDynamoDb.Context.Operations.DescribeTable;
using EfficientDynamoDb.Context.Operations.DescribeTable.Models.Enums;
using EfficientDynamoDb.Context.Operations.GetItem;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Context.Operations.Scan;
using EfficientDynamoDb.Context.Operations.TransactGetItems;
using EfficientDynamoDb.Context.Operations.TransactWriteItems;
using EfficientDynamoDb.Context.Operations.UpdateItem;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Operations.BatchGetItem;
using EfficientDynamoDb.Internal.Operations.BatchWriteItem;
using EfficientDynamoDb.Internal.Operations.DeleteItem;
using EfficientDynamoDb.Internal.Operations.DescribeTable;
using EfficientDynamoDb.Internal.Operations.GetItem;
using EfficientDynamoDb.Internal.Operations.PutItem;
using EfficientDynamoDb.Internal.Operations.Query;
using EfficientDynamoDb.Internal.Operations.Scan;
using EfficientDynamoDb.Internal.Operations.TransactGetItems;
using EfficientDynamoDb.Internal.Operations.TransactWriteItems;
using EfficientDynamoDb.Internal.Operations.UpdateItem;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Context
{
    public class DynamoDbContext
    {
        private readonly DynamoDbContextConfig _config;
        private readonly HttpApi _api;
        private static readonly ConcurrentDictionary<string, Task<(string Pk, string? Sk)>> KeysCache = new ConcurrentDictionary<string, Task<(string Pk, string? Sk)>>();

        public DynamoDbContext(DynamoDbContextConfig config)
        {
            _api = new HttpApi(config.HttpClientFactory);
            _config = config;
        }

        public async Task<GetItemResponse> GetItemAsync(GetItemRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContent = await BuildHttpContentAsync(request).ConfigureAwait(false);
            return await GetItemInternalAsync(httpContent, cancellationToken).ConfigureAwait(false);
        }

        public Task<GetItemResponse> GetItemAsync(IGetItemRequestBuilder builder) => GetItemAsync(builder.Build());

        public async Task<BatchGetItemResponse> BatchGetItemAsync(BatchGetItemRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContent = new BatchGetItemHttpContent(request, _config.TableNamePrefix);
            
            using var response = await _api.SendAsync(_config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, BatchGetItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            return BatchGetItemResponseParser.Parse(result!);
        }
        
        public async Task<BatchWriteItemResponse> BatchWriteItemAsync(BatchWriteItemRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContent = new BatchWriteItemHttpContent(request, _config.TableNamePrefix);
            
            using var response = await _api.SendAsync(_config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, BatchWriteItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            return BatchWriteItemResponseParser.Parse(result!);
        }

        public async Task<QueryResponse> QueryAsync(QueryRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContent = new QueryHttpContent(request, _config.TableNamePrefix);
            
            using var response = await _api.SendAsync(_config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, QueryParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            return QueryResponseParser.Parse(result!);
        }

        public async Task<ScanResponse> ScanAsync(ScanRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContent = new ScanHttpContent(request, _config.TableNamePrefix);

            using var response = await _api.SendAsync(_config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, QueryParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            return ScanResponseParser.Parse(result!);
        }
        
        public async Task<TransactGetItemsResponse> TransactGetItemsAsync(TransactGetItemsRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContent = new TransactGetItemsHttpContent(request, _config.TableNamePrefix);

            using var response = await _api.SendAsync(_config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, TransactGetItemsParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            return TransactGetItemsResponseParser.Parse(result!);
        }

        public async Task<PutItemResponse> PutItemAsync(PutItemRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContent = new PutItemHttpContent(request, _config.TableNamePrefix);
            
            using var response = await _api.SendAsync(_config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, PutItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            return PutItemResponseParser.Parse(result);
        }
        
        public async Task<UpdateItemResponse> UpdateItemAsync(UpdateItemRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContent = await BuildHttpContentAsync(request).ConfigureAwait(false);
            
            using var response = await _api.SendAsync(_config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, UpdateItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            return UpdateItemResponseParser.Parse(result);
        }

        public async Task<DeleteItemResponse> DeleteItemAsync(DeleteItemRequest request, CancellationToken cancellationToken = default)
        {
            var (pkName, skName) = request.Key!.HasKeyNames
                ? (request.Key.PartitionKeyName!, request.Key.SortKeyName)
                : await GetKeyNamesAsync(request.TableName).ConfigureAwait(false);

            using var httpContent = new DeleteItemHttpContent(request, pkName, skName, _config.TableNamePrefix);
            
            using var response = await _api.SendAsync(_config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, PutItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            return DeleteItemResponseParser.Parse(result);
        }
        
        public async Task<TransactWriteItemsResponse> TransactWriteItemsAsync(TransactWriteItemsRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContent = new TransactWriteItemsHttpContent(request, _config.TableNamePrefix);
            
            using var response = await _api.SendAsync(_config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, TransactWriteItemsParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            return TransactWriteItemsResponseParser.Parse(result);
        }
        
        private async ValueTask<GetItemResponse> GetItemInternalAsync(HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            using var response = await _api.SendAsync(_config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, GetItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            // TODO: Consider removing root dictionary
            return GetItemResponseParser.Parse(result!);
        }

        private async ValueTask<HttpContent> BuildHttpContentAsync(GetItemRequest request)
        {
            if (request.Key!.HasKeyNames)
                return new GetItemHttpContent(request, _config.TableNamePrefix, request.Key.PartitionKeyName!, request.Key.SortKeyName!);

            var (remotePkName, remoteSkName) = await GetKeyNamesAsync(request.TableName);
            return new GetItemHttpContent(request, _config.TableNamePrefix, remotePkName, remoteSkName!);
        }
        
        private async ValueTask<HttpContent> BuildHttpContentAsync(UpdateItemRequest request)
        {
            if (request.Key!.HasKeyNames)
                return new UpdateItemHttpContent(request, _config.TableNamePrefix, request.Key.PartitionKeyName!, request.Key.SortKeyName!);

            var (remotePkName, remoteSkName) = await GetKeyNamesAsync(request.TableName);
            return new UpdateItemHttpContent(request, _config.TableNamePrefix, remotePkName, remoteSkName!);
        }
        
        private async ValueTask<(string Pk, string? Sk)> GetKeyNamesAsync(string tableName)
        {
            if (KeysCache.TryGetValue(tableName, out var task) && (task.IsCompletedSuccessfully || !task.IsCompleted))
                return await task.ConfigureAwait(false);

            return await KeysCache.AddOrUpdate(tableName, CreateKeyNamesTaskAsync,
                (table, t) => t.IsCompletedSuccessfully || !t.IsCompleted
                    ? task
                    : CreateKeyNamesTaskAsync(table)).ConfigureAwait(false);
            
            async Task<(string Pk, string? Sk)> CreateKeyNamesTaskAsync(string table)
            {
                var response = await _api.SendAsync<DescribeTableResponse>(_config, new DescribeTableRequestHttpContent(_config.TableNamePrefix, tableName))
                    .ConfigureAwait(false);

                var keySchema = response.Table.KeySchema;
                return (keySchema.First(x => x.KeyType == KeyType.HASH).AttributeName,
                    keySchema.FirstOrDefault(x => x.KeyType == KeyType.RANGE)?.AttributeName);
            }
        }

        private static async ValueTask<Document?> ReadDocumentAsync(HttpResponseMessage response, IParsingOptions options, CancellationToken cancellationToken = default)
        {
            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            var expectedCrc = GetExpectedCrc(response);
            var result = await DdbJsonReader.ReadAsync(responseStream, options, expectedCrc.HasValue, cancellationToken).ConfigureAwait(false);
            
            if (expectedCrc.HasValue && expectedCrc.Value != result.Crc)
                throw new ChecksumMismatchException();

            return result.Value;
        }

        private static uint? GetExpectedCrc(HttpResponseMessage response)
        {
            if (!response.Content.Headers.ContentLength.HasValue)
                return null;
            
            if (response.Headers.TryGetValues("x-amz-crc32", out var crcHeaderValues) && uint.TryParse(crcHeaderValues.FirstOrDefault(), out var expectedCrc))
                return expectedCrc;

            return null;
        }
    }
}