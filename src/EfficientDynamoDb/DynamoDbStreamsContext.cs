using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Operations.DescribeStream;
using EfficientDynamoDb.Internal.Operations.GetShardIterator;
using EfficientDynamoDb.Internal.Operations.ListStreams;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb
{
    public class DynamoDbStreamsContext
    {
        private readonly DynamoDbContextConfig _config;
        private readonly HttpApi _api;
        
        public DynamoDbStreamsContext(DynamoDbContextConfig config)
        {
            _api = new HttpApi(config, ServiceNames.DynamoDbStreams);
            _config = config;
        }
        
        public async Task<ListStreamsResponse> ListStreamsAsync(ListStreamsRequest request, CancellationToken cancellationToken = default)
        {
            
            using var httpContent = new ListStreamsHttpContent(request, _config.TableNamePrefix);

            var response = await _api.SendAsync<ListStreamsResponse>(httpContent, cancellationToken).ConfigureAwait(false);
            return response;
        }

        public async Task<GetShardIteratorResponse> GetShardIteratorAsync(GetShardIteratorRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContext = new GetShardIteratorHttpContent(request);

            var response = await _api.SendAsync<GetShardIteratorResponse>(httpContext, cancellationToken).ConfigureAwait(false);
            return response;
        }
        
        public async Task<DescribeStreamResponse> DescribeStreamAsync(DescribeStreamRequest request, CancellationToken cancellationToken = default)
        {
            using var httpContext = new DescribeStreamHttpContent(request);

            var response = await _api.SendAsync<DescribeStreamResponse>(httpContext, cancellationToken).ConfigureAwait(false);
            return response;
        }
    }
}