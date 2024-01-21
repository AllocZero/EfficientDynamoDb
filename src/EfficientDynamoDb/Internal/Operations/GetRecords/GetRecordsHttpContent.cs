using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Internal.Operations.GetRecords
{
    internal sealed class GetRecordsHttpContent : DynamoDbHttpContent
    {
        private readonly GetRecordsRequest _request;

        public GetRecordsHttpContent(GetRecordsRequest request) : base("DynamoDBStreams_20120810.GetRecords")
        {
            _request = request;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            writer.WriteString("ShardIterator", _request.ShardIterator);
            if (_request.Limit > 0)
                writer.WriteNumber("Limit", _request.Limit);

            writer.WriteEndObject();
            return default;
        }
    }
}