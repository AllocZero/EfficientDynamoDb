using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations;

namespace EfficientDynamoDb.Internal.Operations.ListStreams
{
    internal class ListStreamsHttpContent : DynamoDbHttpContent
    {
        private readonly ListStreamsRequest _request;
        private readonly string? _tablePrefix;

        public ListStreamsHttpContent(ListStreamsRequest request, string? tablePrefix) : base("DynamoDBStreams_20120810.ListStreams")
        {
            _request = request;
            _tablePrefix = tablePrefix;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();
            
            if (_request.ExclusiveStartStreamArn != null)
                writer.WriteString("ExclusiveStartStreamArn", _request.ExclusiveStartStreamArn);
            
            if (_request.Limit > 0)
                writer.WriteNumber("Limit", _request.Limit);

            if (_request.TableName != null)
                writer.WriteTableName(_tablePrefix, _request.TableName);
            
            writer.WriteEndObject();

            return default;
        }
    }
}