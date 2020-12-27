using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.GetItem;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.GetItem;
using EfficientDynamoDb.Internal.Operations.PutItem;
using EfficientDynamoDb.Internal.Operations.Query;
using EfficientDynamoDb.Internal.Reader;
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

        public async Task<T?> GetItemAsync<T>(object partitionKey, CancellationToken cancellationToken = default) where T : class
        {
            var classInfo = Config.Metadata.GetOrAddClassInfo(typeof(T));
            using var httpContent = new GetItemHighLevelHttpContent(new GetItemHighLevelRequest(partitionKey) {TableName = classInfo.TableName!},
                Config.TableNamePrefix, classInfo.PartitionKey!);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var document = await ReadDocumentAsync(response, GetItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            return document?.ToObject<T>(Config.Metadata);
        }
        
        public async Task<T?> GetItemAsync<T>(object partitionKey, object sortKey, CancellationToken cancellationToken = default) where T : class
        {
            var classInfo = Config.Metadata.GetOrAddClassInfo(typeof(T));
            using var httpContent = new GetItemHighLevelHttpContent(new GetItemHighLevelRequest(partitionKey, sortKey) {TableName = classInfo.TableName!},
                Config.TableNamePrefix, classInfo.PartitionKey!, classInfo.SortKey!);

            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);
            var document = await ReadDocumentAsync(response, GetItemParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            return document?.ToObject<T>(Config.Metadata);
        }
        
        public async Task<IReadOnlyList<T>> QueryAsync<T>(QueryRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContent = new QueryHttpContent(request, Config.TableNamePrefix);
            
            using var response = await Api.SendAsync(Config, httpContent, cancellationToken).ConfigureAwait(false);

            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            var expectedCrc = GetExpectedCrc(response);
            var result = await EntityDdbJsonReader.ReadAsync<EntityQueryResponse<T>>(responseStream, Config.Metadata, expectedCrc.HasValue, cancellationToken).ConfigureAwait(false);
            
            if (expectedCrc.HasValue && expectedCrc.Value != result.Crc)
                throw new ChecksumMismatchException();

            return result.Value!.Items;
        }
    }
}