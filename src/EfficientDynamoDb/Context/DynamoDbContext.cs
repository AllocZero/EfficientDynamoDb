using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
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
        private readonly HttpApi _api = new HttpApi();
        private static readonly ConcurrentDictionary<string, Task<(string Pk, string? Sk)>> KeysCache = new ConcurrentDictionary<string, Task<(string Pk, string? Sk)>>();

        public DynamoDbContext(DynamoDbContextConfig config)
        {
            _config = config;
        }

        public async Task<DescribeTableResponse> DescribeTableAsync(string tableName)
        {
            var httpContent = new DescribeTableRequestHttpContent(GetTableNameWithPrefix(tableName));

            var response = await _api.SendAsync<DescribeTableResponse>(_config.RegionEndpoint.SystemName, _config.Credentials, httpContent).ConfigureAwait(false);

            return response;
        }

        public async Task<GetItemResponse> GetItemAsync(GetItemRequest request)
        {
            using var httpContent = await BuildHttpContentAsync(request).ConfigureAwait(false);
            return await GetItemInternalAsync(httpContent).ConfigureAwait(false);
        }

        public Task<GetItemResponse> GetItemAsync(IGetItemRequestBuilder builder) => GetItemAsync(builder.Build());

        public async Task<BatchGetItemResponse> BatchGetItemAsync(BatchGetItemRequest request)
        {
            using var httpContent = new BatchGetItemHttpContent(request, _config.TableNamePrefix);
            
            using var response = await _api.SendAsync(_config.RegionEndpoint.SystemName, _config.Credentials, httpContent).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, BatchGetItemParsingOptions.Instance).ConfigureAwait(false);

            return BatchGetItemResponseParser.Parse(result!);
        }
        
        public async Task<BatchWriteItemResponse> BatchWriteItemAsync(BatchWriteItemRequest request)
        {
            using var httpContent = new BatchWriteItemHttpContent(request, _config.TableNamePrefix);
            
            using var response = await _api.SendAsync(_config.RegionEndpoint.SystemName, _config.Credentials, httpContent).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, BatchWriteItemParsingOptions.Instance).ConfigureAwait(false);

            return BatchWriteItemResponseParser.Parse(result!);
        }

        public async Task<QueryResponse> QueryAsync(QueryRequest request)
        {
            using var httpContent = new QueryHttpContent(GetTableNameWithPrefix(request.TableName!), request);
            
            using var response = await _api.SendAsync(_config.RegionEndpoint.SystemName, _config.Credentials, httpContent).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, QueryParsingOptions.Instance).ConfigureAwait(false);

            return QueryResponseParser.Parse(result!);
        }

        public async Task<ScanResponse> ScanAsync(ScanRequest request)
        {
            using var httpContent = new ScanHttpContent(GetTableNameWithPrefix(request.TableName!), request);

            using var response = await _api.SendAsync(_config.RegionEndpoint.SystemName, _config.Credentials, httpContent).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, QueryParsingOptions.Instance).ConfigureAwait(false);

            return ScanResponseParser.Parse(result!);
        }
        
        public async Task<TransactGetItemsResponse> TransactGetItemsAsync(TransactGetItemsRequest request)
        {
            using var httpContent = new TransactGetItemsHttpContent(request, _config.TableNamePrefix);

            using var response = await _api.SendAsync(_config.RegionEndpoint.SystemName, _config.Credentials, httpContent).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, TransactGetItemsParsingOptions.Instance).ConfigureAwait(false);

            return TransactGetItemsResponseParser.Parse(result!);
        }

        public async Task<PutItemResponse> PutItemAsync(PutItemRequest request)
        {
            using var httpContent = new PutItemHttpContent(request, GetTableNameWithPrefix(request.TableName));
            
            using var response = await _api.SendAsync(_config.RegionEndpoint.SystemName, _config.Credentials, httpContent).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, PutItemParsingOptions.Instance).ConfigureAwait(false);

            return PutItemResponseParser.Parse(result);
        }
        
        public async Task<UpdateItemResponse> UpdateItemAsync(UpdateItemRequest request)
        {
            using var httpContent = await BuildHttpContentAsync(request).ConfigureAwait(false);
            
            using var response = await _api.SendAsync(_config.RegionEndpoint.SystemName, _config.Credentials, httpContent).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, UpdateItemParsingOptions.Instance).ConfigureAwait(false);

            return UpdateItemResponseParser.Parse(result);
        }

        public async Task<DeleteItemResponse> DeleteItemAsync(DeleteItemRequest request)
        {
            var (pkName, skName) = request.Key!.HasKeyNames
                ? (request.Key.PartitionKeyName!, request.Key.SortKeyName)
                : await GetKeyNamesAsync(request.TableName).ConfigureAwait(false);

            using var httpContent = new DeleteItemHttpContent(request, pkName, skName, _config.TableNamePrefix);
            
            using var response = await _api.SendAsync(_config.RegionEndpoint.SystemName, _config.Credentials, httpContent).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, PutItemParsingOptions.Instance).ConfigureAwait(false);

            return DeleteItemResponseParser.Parse(result);
        }
        
        public async Task<TransactWriteItemsResponse> TransactWriteItemsAsync(TransactWriteItemsRequest request)
        {
            using var httpContent = new TransactWriteItemsHttpContent(request, _config.TableNamePrefix);
            
            using var response = await _api.SendAsync(_config.RegionEndpoint.SystemName, _config.Credentials, httpContent).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, TransactWriteItemsParsingOptions.Instance).ConfigureAwait(false);

            return TransactWriteItemsResponseParser.Parse(result);
        }
        
        private async ValueTask<GetItemResponse> GetItemInternalAsync(HttpContent httpContent)
        {
            using var response = await _api.SendAsync(_config.RegionEndpoint.SystemName, _config.Credentials, httpContent).ConfigureAwait(false);
            var result = await ReadDocumentAsync(response, GetItemParsingOptions.Instance).ConfigureAwait(false);

            // TODO: Consider removing root dictionary
            return GetItemResponseParser.Parse(result!);
        }

        private async ValueTask<HttpContent> BuildHttpContentAsync(GetItemRequest request)
        {
            var prefixedTableName = GetTableNameWithPrefix(request.TableName);

            if (request.Key!.HasKeyNames)
                return new GetItemHttpContent(request, prefixedTableName, request.Key.PartitionKeyName!, request.Key.SortKeyName!);

            var (remotePkName, remoteSkName) = await GetKeyNamesAsync(request.TableName);
            return new GetItemHttpContent(request, prefixedTableName, remotePkName, remoteSkName!);
        }
        
        private async ValueTask<HttpContent> BuildHttpContentAsync(UpdateItemRequest request)
        {
            var prefixedTableName = GetTableNameWithPrefix(request.TableName);

            if (request.Key!.HasKeyNames)
                return new UpdateItemHttpContent(request, prefixedTableName, request.Key.PartitionKeyName!, request.Key.SortKeyName!);

            var (remotePkName, remoteSkName) = await GetKeyNamesAsync(request.TableName);
            return new UpdateItemHttpContent(request, prefixedTableName, remotePkName, remoteSkName!);
        }
        
        private async ValueTask<(string Pk, string? Sk)> GetKeyNamesAsync(string tableName) =>
            await KeysCache.GetOrAdd(tableName, async table =>
            {
                var response = await DescribeTableAsync(table).ConfigureAwait(false);

                var keySchema = response.Table.KeySchema;
                return (keySchema.First(x => x.KeyType == KeyType.HASH).AttributeName,
                    keySchema.FirstOrDefault(x => x.KeyType == KeyType.RANGE)?.AttributeName);
            }).ConfigureAwait(false);

        private async ValueTask<Document?> ReadDocumentAsync(HttpResponseMessage response, IParsingOptions options)
        {
            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            var expectedCrc = GetExpectedCrc(response);
            var result = await DdbJsonReader.ReadAsync(responseStream, options, expectedCrc.HasValue).ConfigureAwait(false);
            
            // TODO: Throw meaningful exception
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetTableNameWithPrefix(string tableName) => $"{_config.TableNamePrefix}{tableName}";
    }
}