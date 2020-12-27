using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.GetItem;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.GetItem;
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

        public async Task<TResult?> GetItemAsync<TResult, TPartitionKey>(TPartitionKey partitionKey, CancellationToken cancellationToken = default)
            where TResult : class
        {
            var classInfo = Config.Metadata.GetOrAddClassInfo(typeof(TResult));
            var request = new GetItemHighLevelRequest<TPartitionKey>(partitionKey) {TableName = classInfo.TableName!};
            using var httpContent = new GetItemHighLevelHttpContent<TPartitionKey>(request, Config.TableNamePrefix,
                (DdbPropertyInfo<TPartitionKey>) classInfo.PartitionKey!);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var document = await ReadDocumentAsync(response, GetItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            return document?.ToObject<TResult>(Config.Metadata);
        }

        public async Task<TResult?> GetItemAsync<TResult, TPartitionKey, TSortKey>(TPartitionKey partitionKey, TSortKey sortKey,
            CancellationToken cancellationToken = default) where TResult : class
        {
            var classInfo = Config.Metadata.GetOrAddClassInfo(typeof(TResult));
            var request = new GetItemHighLevelRequest<TPartitionKey, TSortKey>(partitionKey, sortKey) {TableName = classInfo.TableName!};
            using var httpContent = new GetItemHighLevelHttpContent<TPartitionKey, TSortKey>(request, Config.TableNamePrefix,
                (DdbPropertyInfo<TPartitionKey>) classInfo.PartitionKey!, (DdbPropertyInfo<TSortKey>) classInfo.SortKey!);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var document = await ReadDocumentAsync(response, GetItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            return document?.ToObject<TResult>(Config.Metadata);
        }
    }
}