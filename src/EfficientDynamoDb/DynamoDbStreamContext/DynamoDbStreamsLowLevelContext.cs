using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Operations.Streams;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb
{
    public class DynamoDbStreamsLowLevelContext : IDynamoDbStreamsLowLevelContext
    {
        internal DynamoDbContextConfig Config { get; }
        
        internal HttpApi Api { get; }

        internal DynamoDbStreamsLowLevelContext(DynamoDbContextConfig config, HttpApi api)
        {
            Config = config;
            Api = api;
        }
        
        public async Task<ListStreamsResponse> ListStreamsAsync(ListStreamsRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContent = new ListStreamsHttpContent(request, Config.TableNamePrefix);

            var response = await Api.SendAsync<ListStreamsResponse>(httpContent, cancellationToken).ConfigureAwait(false);
            return response;
        }

        public async Task<GetShardIteratorResponse> GetShardIteratorAsync(GetShardIteratorRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContext = new GetShardIteratorHttpContent(request);

            var response = await Api.SendAsync<GetShardIteratorResponse>(httpContext, cancellationToken).ConfigureAwait(false);
            return response;
        }
        
        public async Task<DescribeStreamResponse> DescribeStreamAsync(DescribeStreamRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContext = new DescribeStreamHttpContent(request);

            var response = await Api.SendAsync<DescribeStreamResponse>(httpContext, cancellationToken).ConfigureAwait(false);
            return response;
        }

        public async Task<GetRecordsResponse> GetRecordsAsync(GetRecordsRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContext = new GetRecordsHttpContent(request);

            var response = await Api.SendAsync(httpContext, cancellationToken).ConfigureAwait(false);
            var result = await DynamoDbLowLevelContext.ReadDocumentAsync(response, GetRecordsParsingOptions.Instance, cancellationToken).ConfigureAwait(false);

            return GetRecordsResponseParser.Parse(result!);
        }
    }
}