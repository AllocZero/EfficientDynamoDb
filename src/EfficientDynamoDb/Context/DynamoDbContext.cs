using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EfficientDynamoDb.Api.DescribeTable;
using EfficientDynamoDb.Api.DescribeTable.Models.Enums;
using EfficientDynamoDb.Context.RequestBuilders;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Builder;
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

        public async Task<Document> GetItemAsync<TPk>(string tableName, TPk pk)
            where TPk : IAttributeValue
        {
            var (pkName, _) = await GetKeyNamesAsync(tableName).ConfigureAwait(false);
            
            using var httpContent = new GetItemHttpContent<TPk>(GetTableNameWithPrefix(tableName), pkName, pk);
            return await GetItemInternalAsync(httpContent).ConfigureAwait(false);
        }
        
        public async Task<Document> GetItemAsync<TPk, TSk>(string tableName, TPk pk, TSk sk)
            where TPk : IAttributeValue
            where TSk : IAttributeValue
        {
            var (pkName, skName) = await GetKeyNamesAsync(tableName).ConfigureAwait(false);
            
            using var httpContent = new GetItemHttpContent<TPk, TSk>(GetTableNameWithPrefix(tableName), pkName, pk, skName!, sk);
            return await GetItemInternalAsync(httpContent).ConfigureAwait(false);
        }
        
        public async Task<Document> GetItemAsync<TPk>(IGetItemRequestBuilder<TPk> builder) where TPk : IAttributeValue
        {
            using var httpContent = await BuildHttpContentAsync(builder).ConfigureAwait(false);
            return await GetItemInternalAsync(httpContent).ConfigureAwait(false);
        }

        public async Task<Document> GetItemAsync<TPk, TSk>(IGetItemRequestBuilder<TPk, TSk> builder)
            where TPk : IAttributeValue
            where TSk : IAttributeValue
        {
            using var httpContent = await BuildHttpContentAsync(builder).ConfigureAwait(false);
            return await GetItemInternalAsync(httpContent).ConfigureAwait(false);
        }
        
        private async ValueTask<Document> GetItemInternalAsync(HttpContent httpContent)
        {
            using var response = await _api.SendAsync(_config.RegionEndpoint.SystemName, _config.Credentials, httpContent).ConfigureAwait(false);

            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await DdbJsonReader.ReadAsync(responseStream, GetParsingOptions.Instance).ConfigureAwait(false);

            // TODO: Consider removing root dictionary
            return result["Item"].AsDocument();
        }

        private async ValueTask<HttpContent> BuildHttpContentAsync<TPk>(IGetItemRequestBuilder<TPk> builder) where TPk : IAttributeValue
        {
            var data = builder.Build();
            var prefixedTableName = GetTableNameWithPrefix(data.TableName);
            if (data.PkName != null)
                return new GetItemHttpContent<TPk>(prefixedTableName, data.PkName, data.PkValue);

            var (remotePkName, _) = await GetKeyNamesAsync(data.TableName);
            return new GetItemHttpContent<TPk>(prefixedTableName, remotePkName, data.PkValue);
        }

        private async ValueTask<HttpContent> BuildHttpContentAsync<TPk, TSk>(IGetItemRequestBuilder<TPk, TSk> builder)
            where TPk : IAttributeValue
            where TSk : IAttributeValue
        {
            var data = builder.Build();
            var prefixedTableName = GetTableNameWithPrefix(data.TableName);
            if (data.PkName != null && data.SkName != null)
                return new GetItemHttpContent<TPk, TSk>(prefixedTableName, data.PkName, data.PkValue, data.SkName, data.SkValue);

            var (remotePkName, remoteSkName) = await GetKeyNamesAsync(data.TableName);
            return new GetItemHttpContent<TPk, TSk>(prefixedTableName, remotePkName, data.PkValue, remoteSkName!, data.SkValue);
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