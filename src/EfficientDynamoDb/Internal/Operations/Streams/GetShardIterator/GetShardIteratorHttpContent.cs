using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Internal.Operations.Streams
{
    internal class GetShardIteratorHttpContent : DynamoDbHttpContent
    {
        private readonly GetShardIteratorRequest _request;

        public GetShardIteratorHttpContent(GetShardIteratorRequest request) : base("DynamoDBStreams_20120810.GetShardIterator")
        {
            _request = request;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();
            
            writer.WriteString("StreamArn", _request.StreamArn);
            writer.WriteString("ShardId", _request.ShardId);
            writer.WriteEnum("ShardIteratorType", _request.ShardIteratorType);
            if (!string.IsNullOrEmpty(_request.SequenceNumber))
                writer.WriteString("SequenceNumber", _request.SequenceNumber);
            
            writer.WriteEndObject();
            return default;
        }
    }
}