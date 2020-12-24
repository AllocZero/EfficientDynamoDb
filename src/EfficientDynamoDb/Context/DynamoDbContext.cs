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

        public async Task PutItemAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class
        {
            var classInfo = Config.Metadata.GetOrAddClassInfo(typeof(T));
            using var httpContent = new PutItemHighLevelHttpContent(new HighLevelPutItemRequest {Item = entity, TableName = classInfo.TableName!},
                Config.TableNamePrefix, classInfo);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            await ReadDocumentAsync(response, PutItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);
        }
    }
}