using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Reader;
using static EfficientDynamoDb.Context.DynamoDbLowLevelContext;

namespace EfficientDynamoDb.Context
{
    public partial class DynamoDbContext
    {
        internal DynamoDbContextConfig Config => LowContext.Config;
        private HttpApi Api => LowContext.Api;
        
        public DynamoDbLowLevelContext LowContext { get; }

        public DynamoDbContext(DynamoDbContextConfig config)
        {
            LowContext = new DynamoDbLowLevelContext(config, new HttpApi(config.HttpClientFactory));
        }

        public T ToObject<T>(Document document) where T : class => document.ToObject<T>(Config.Metadata);

        public Document ToDocument<T>(T entity) where T : class => entity.ToDocument(Config.Metadata);

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