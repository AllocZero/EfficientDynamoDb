using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Converters.Json;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb
{
    public partial class DynamoDbContext : IDynamoDbContext
    {
        private readonly DynamoDbLowLevelContext _lowContext;
        
        internal DynamoDbContextConfig Config => _lowContext.Config;
        DynamoDbContextConfig IDynamoDbContext.Config => Config;
        private HttpApi Api => _lowContext.Api;

        [Obsolete("Going to be removed in 1.0. Use LowLevel instead")]
        // ReSharper disable once ConvertToAutoPropertyWhenPossible
        public DynamoDbLowLevelContext LowContext => _lowContext;

        public IDynamoDbLowLevelContext LowLevel => _lowContext;

        public DynamoDbContext(DynamoDbContextConfig config)
        {
            _lowContext = new DynamoDbLowLevelContext(config, new HttpApi(config, ServiceNames.DynamoDb));
        }

        public T ToObject<T>(Document document) where T : class => document.ToObject<T>(Config.Metadata);

        public Document ToDocument<T>(T entity) where T : class => entity.ToDocument(Config.Metadata);

        async Task<TResponse> IDynamoDbContext.ExecuteAsync<TResponse>(HttpContent httpContent, CancellationToken cancellationToken) where TResponse : class
        {
            using var response = await Api.SendAsync(httpContent, cancellationToken).ConfigureAwait(false);
            return await ReadAsync<TResponse>(response, cancellationToken).ConfigureAwait(false);
        }

        private async ValueTask<TResult> ReadAsync<TResult>(HttpResponseMessage response, CancellationToken cancellationToken = default) where TResult : class
        {
            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            var expectedCrc = response.GetExpectedCrc();
            var classInfo = Config.Metadata.GetOrAddClassInfo(typeof(TResult), typeof(JsonObjectDdbConverter<TResult>));
            var result = await EntityDdbJsonReader.ReadAsync<TResult>(responseStream, classInfo, Config.Metadata, expectedCrc.HasValue, cancellationToken: cancellationToken).ConfigureAwait(false);
            
            if (expectedCrc.HasValue && expectedCrc.Value != result.Crc)
                throw new ChecksumMismatchException();

            return result.Value!;
        }
    }
}