using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Operations.PutItem;
using static EfficientDynamoDb.Context.DynamoDbLowLevelContext;

namespace EfficientDynamoDb.Context
{
    public class DynamoDbContext
    {
        private DynamoDbContextConfig Config => LowContext.Config;
        private HttpApi Api => LowContext.Api;
        
        public DynamoDbLowLevelContext LowContext { get; }

        public DynamoDbContext(DynamoDbContextConfig config)
        {
            LowContext = new DynamoDbLowLevelContext(config, new HttpApi(config.HttpClientFactory));
        }

        public async Task PutItemAsync<T>(string tableName, T entity, CancellationToken cancellationToken = default) where T : class
        {
            using var httpContent = new PutItemHighLevelHttpContent(new HighLevelPutItemRequest {Item = entity, TableName = tableName},
                Config.TableNamePrefix, Config.Metadata);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            await ReadDocumentAsync(response, PutItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);
        }
    }
}