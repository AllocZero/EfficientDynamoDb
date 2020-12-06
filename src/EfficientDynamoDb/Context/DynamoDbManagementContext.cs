using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.DescribeTable;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Operations.DescribeTable;

namespace EfficientDynamoDb.Context
{
    public class DynamoDbManagementContext
    {
        private readonly DynamoDbContextConfig _config;
        private readonly HttpApi _api = new HttpApi();

        public DynamoDbManagementContext(DynamoDbContextConfig config)
        {
            _config = config;
        }

        public async Task<DescribeTableResponse> DescribeTableAsync(string tableName, CancellationToken cancellationToken = default)
        {
            var httpContent = new DescribeTableRequestHttpContent(_config.TableNamePrefix + tableName);

            var response = await _api.SendAsync<DescribeTableResponse>(_config, httpContent, cancellationToken).ConfigureAwait(false);

            return response;
        }
    }
}