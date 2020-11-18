using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using EfficientDynamoDb.Api.DescribeTable;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Builder;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Context
{
    public class DynamoDbContext
    {
        private readonly DynamoDbContextConfig _config;
        private readonly HttpApi _api = new HttpApi();

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

        public async Task<Document> GetItemAsync<TPk, TSk>(string tableName, TPk pk, TSk sk)
            where TPk : IAttributeValue
            where TSk : IAttributeValue
        {
            var pkName = "pk";
            var skName = "sk";
            
            using var httpContent = new GetItemHttpContent<TPk, TSk>(GetTableNameWithPrefix(tableName), pkName, pk, skName, sk);
            using var response = await _api.SendAsync(_config.RegionEndpoint.SystemName, _config.Credentials, httpContent).ConfigureAwait(false);

            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var result = await DdbJsonReader.ReadAsync(responseStream).ConfigureAwait(false);

            return result["Item"].AsDocument();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetTableNameWithPrefix(string tableName) => $"{_config.TableNamePrefix}{tableName}";
    }
}