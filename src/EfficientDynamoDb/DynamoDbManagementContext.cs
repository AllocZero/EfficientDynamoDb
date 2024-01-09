using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Operations.DescribeTable;
using EfficientDynamoDb.Operations.DescribeTable;

namespace EfficientDynamoDb
{
    public class DynamoDbManagementContext
    {
        private readonly DynamoDbContextConfig _config;
        private readonly HttpApi _api;

        public DynamoDbManagementContext(DynamoDbContextConfig config)
        {
            _api = new HttpApi(config, ServiceNames.DynamoDb);
            _config = config;
        }

        public async Task<DescribeTableResponse> DescribeTableAsync(string tableName, CancellationToken cancellationToken = default)
        {
            var httpContent = new DescribeTableRequestHttpContent(_config.TableNamePrefix, tableName);

            var response = await _api.SendAsync<DescribeTableResponse>(httpContent, cancellationToken).ConfigureAwait(false);

            return response;
        }
    }
}