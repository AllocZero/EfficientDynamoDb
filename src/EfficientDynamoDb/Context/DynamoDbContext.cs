using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.GetItem;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.GetItem;
using EfficientDynamoDb.Internal.Operations.PutItem;
using EfficientDynamoDb.Internal.Operations.Query;
using EfficientDynamoDb.Internal.Reader;
using static EfficientDynamoDb.Context.DynamoDbLowLevelContext;

namespace EfficientDynamoDb.Context
{
    public class DynamoDbContext
    {
        internal DynamoDbContextConfig Config => LowContext.Config;
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

        public async Task<TEntity?> GetItemAsync<TEntity, TPartitionKey>(TPartitionKey partitionKey, CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var classInfo = Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            var request = new GetItemHighLevelRequest<TPartitionKey>(partitionKey) {TableName = classInfo.TableName!};
            using var httpContent = new GetItemHighLevelHttpContent<TPartitionKey>(request, Config.TableNamePrefix,
                (DdbPropertyInfo<TPartitionKey>) classInfo.PartitionKey!);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<GetItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return result.Item;
        }

        public async Task<TEntity?> GetItemAsync<TEntity, TPartitionKey, TSortKey>(TPartitionKey partitionKey, TSortKey sortKey,
            CancellationToken cancellationToken = default) where TEntity : class
        {
            var classInfo = Config.Metadata.GetOrAddClassInfo(typeof(TEntity));
            var request = new GetItemHighLevelRequest<TPartitionKey, TSortKey>(partitionKey, sortKey) {TableName = classInfo.TableName!};
            using var httpContent = new GetItemHighLevelHttpContent<TPartitionKey, TSortKey>(request, Config.TableNamePrefix,
                (DdbPropertyInfo<TPartitionKey>) classInfo.PartitionKey!, (DdbPropertyInfo<TSortKey>) classInfo.SortKey!);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var result = await ReadAsync<GetItemEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);

            return result.Item;
        }

        public IBasicQueryRequestBuilder Query() => RequestsBuilder.Query(this);
        
        internal async Task<IReadOnlyList<TEntity>> QueryListAsync<TEntity>(QueryHighLevelRequest request, CancellationToken cancellationToken = default) where TEntity : class
        {
            var result = await QueryAsync<TEntity>(request, cancellationToken).ConfigureAwait(false);
            return result.Items; // TODO: Use projection here to no allocate unnecessary memory 
        }

        internal async Task<QueryEntityResponse<TEntity>> QueryAsync<TEntity>(QueryHighLevelRequest request, CancellationToken cancellationToken = default) where TEntity : class
        {
            using var httpContent = new QueryHighLevelHttpContent(request, Config.TableNamePrefix, Config.Metadata);
            
            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            return await ReadAsync<QueryEntityResponse<TEntity>>(response, cancellationToken).ConfigureAwait(false);
        }
        
        private async ValueTask<TResult> ReadAsync<TResult>(HttpResponseMessage response, CancellationToken cancellationToken = default) where TResult : class
        {
            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            var expectedCrc = GetExpectedCrc(response);
            var result = await EntityDdbJsonReader.ReadAsync<TResult>(responseStream, Config.Metadata, expectedCrc.HasValue, cancellationToken).ConfigureAwait(false);
            
            if (expectedCrc.HasValue && expectedCrc.Value != result.Crc)
                throw new ChecksumMismatchException();

            return result.Value!;
        }
    }
}