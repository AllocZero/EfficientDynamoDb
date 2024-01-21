using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb
{
    public interface IDynamoDbStreamsLowLevelContext
    {
        Task<ListStreamsResponse> ListStreamsAsync(ListStreamsRequest request, CancellationToken cancellationToken = default);

        Task<GetShardIteratorResponse> GetShardIteratorAsync(GetShardIteratorRequest request, CancellationToken cancellationToken = default);

        Task<DescribeStreamResponse> DescribeStreamAsync(DescribeStreamRequest request, CancellationToken cancellationToken = default);

        Task<GetRecordsResponse> GetRecordsAsync(GetRecordsRequest request, CancellationToken cancellationToken = default);
    }
}