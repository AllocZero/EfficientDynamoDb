using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Scan;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Query;

namespace EfficientDynamoDb.Internal.Operations.Scan
{
    public class ScanHttpContent : IterableHttpContent
    {
        private readonly string _tableName;
        private readonly ScanRequest _request;

        public ScanHttpContent(string tableName, ScanRequest request) : base("DynamoDB_20120810.Scan")
        {
            _tableName = tableName;
            _request = request;
        }

        protected override ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();

            writer.WriteString("TableName", _tableName);

            if (_request.IndexName != null)
                writer.WriteString("IndexName", _request.IndexName);

            if (_request.FilterExpression != null)
                writer.WriteString("FilterExpression", _request.FilterExpression);

            if (_request.ExpressionAttributeNames?.Count > 0)
                writer.WriteExpressionAttributeNames(_request.ExpressionAttributeNames);

            if (_request.ExpressionAttributeValues?.Count > 0)
                writer.WriteExpressionAttributeValues(_request.ExpressionAttributeValues);

            if (_request.Limit.HasValue)
                writer.WriteNumber("Limit", _request.Limit.Value);

            if (_request.ProjectionExpression?.Count > 0)
                writer.WriteString("ProjectionExpression", string.Join(",", _request.ProjectionExpression));
            else if (_request.Select.HasValue)
                WriteSelect(writer, _request.Select.Value);

            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                writer.WriteReturnConsumedCapacity(_request.ReturnConsumedCapacity);

            if (_request.ExclusiveStartKey != null)
                WriteExclusiveStartKey(writer, _request.ExclusiveStartKey);

            if (_request.ConsistentRead)
                writer.WriteBoolean("ConsistentRead", true);

            if (_request.Segment.HasValue)
                writer.WriteNumber("Segment", _request.Segment.Value);

            if (_request.TotalSegments.HasValue)
                writer.WriteNumber("TotalSegments", _request.TotalSegments.Value);

            writer.WriteEndObject();

            return default;
        }
    }
}