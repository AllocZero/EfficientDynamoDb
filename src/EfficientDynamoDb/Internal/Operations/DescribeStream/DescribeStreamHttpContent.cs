using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Internal.Operations.DescribeStream
{
    internal class DescribeStreamHttpContent : DynamoDbHttpContent
    {
        private readonly DescribeStreamRequest _request;

        public DescribeStreamHttpContent(DescribeStreamRequest request) : base("DynamoDBStreams_20120810.DescribeStream")
        {
            _request = request;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            
            writer.WriteStartObject();

            writer.WriteString("StreamArn", _request.StreamArn);
            if (!string.IsNullOrEmpty(_request.ExclusiveStartShardId))
                writer.WriteString("ExclusiveStartShardId", _request.ExclusiveStartShardId);
            if (_request.Limit > 0)
                writer.WriteNumber("Limit", _request.Limit);
            
            writer.WriteEndObject();
            return default;
        }
    }
}