using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EfficientDynamoDb.Api.DescribeTable;
using EfficientDynamoDb.Api.DescribeTable.Models.Enums;
using EfficientDynamoDb.Context.RequestBuilders;
using EfficientDynamoDb.Context.Requests;
using EfficientDynamoDb.Context.Requests.GetItem;
using EfficientDynamoDb.Context.Requests.Query;
using EfficientDynamoDb.Context.Responses.Query;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Builder;
using EfficientDynamoDb.Internal.Builder.GetItemHttpContents;
using EfficientDynamoDb.Internal.Parsers;
using EfficientDynamoDb.Internal.Reader;
using EfficientDynamoDb.Internal.Reader.ParsingOptions;

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

        public async Task<Document> GetItemAsync(GetItemRequest request)
        {
            using var httpContent = await BuildHttpContentAsync(request).ConfigureAwait(false);
            return await GetItemInternalAsync(httpContent).ConfigureAwait(false);
        }

        public Task<Document> GetItemAsync(IGetItemRequestBuilder builder) => GetItemAsync(builder.Build());

        public async Task<QueryResponse> QueryAsync(QueryRequest request)
        {
            using var httpContent = new QueryHttpContent(GetTableNameWithPrefix(request.TableName!), request);
            
            using var response = await _api.SendAsync(_config.RegionEndpoint.SystemName, _config.Credentials, httpContent).ConfigureAwait(false);
            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await DdbJsonReader.ReadAsync(responseStream, QueryParsingOptions.Instance).ConfigureAwait(false);

            return QueryResponseParser.Parse(result);
        }
        
        private async ValueTask<Document> GetItemInternalAsync(HttpContent httpContent)
        {
            using var response = await _api.SendAsync(_config.RegionEndpoint.SystemName, _config.Credentials, httpContent).ConfigureAwait(false);

            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await DdbJsonReader.ReadAsync(responseStream, GetParsingOptions.Instance).ConfigureAwait(false);

            // TODO: Consider removing root dictionary
            return result["Item"].AsDocument();
        }

        private async ValueTask<HttpContent> BuildHttpContentAsync(GetItemRequest request)
        {
            var prefixedTableName = GetTableNameWithPrefix(request.TableName);

            if (request.Key!.HasKeyNames)
                return new GetItemHttpContent(request, prefixedTableName, request.Key.PartitionKeyName!, request.Key.SortKeyName!);

            var (remotePkName, remoteSkName) = await GetKeyNamesAsync(request.TableName);
            return new GetItemHttpContent(request, prefixedTableName, remotePkName, remoteSkName!);
        }
        
        private async ValueTask<(string Pk, string? Sk)> GetKeyNamesAsync(string tableName) =>
            await KeysCache.GetOrAdd(tableName, async table =>
            {
                var response = await DescribeTableAsync(table).ConfigureAwait(false);

                var keySchema = response.Table.KeySchema;
                return (keySchema.First(x => x.KeyType == KeyType.HASH).AttributeName,
                    keySchema.FirstOrDefault(x => x.KeyType == KeyType.RANGE)?.AttributeName);
            }).ConfigureAwait(false);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetTableNameWithPrefix(string tableName) => $"{_config.TableNamePrefix}{tableName}";
    }
}