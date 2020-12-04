using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.Query
{
    internal class QueryHttpContent : IterableHttpContent
    {
        private readonly string _tableName;
        private readonly QueryRequest _request;

        public QueryHttpContent(string tableName, QueryRequest request) : base("DynamoDB_20120810.Query")
        {
            _tableName = tableName;
            _request = request;
        }

        protected override ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();
            
            writer.WriteString("TableName", _tableName);
            writer.WriteString("KeyConditionExpression", _request.KeyConditionExpression);
            
            if(_request.IndexName != null)
                writer.WriteString("IndexName", _request.IndexName);
            
            if (_request.FilterExpression != null)
                writer.WriteString("FilterExpression", _request.FilterExpression);

            if (_request.ExpressionAttributeNames?.Count > 0)
                writer.WriteExpressionAttributeNames(_request.ExpressionAttributeNames);
            
            if (_request.ExpressionAttributeValues?.Count > 0)
                writer.WriteExpressionAttributeValues(_request.ExpressionAttributeValues);
            
            if(_request.Limit.HasValue)
                writer.WriteNumber("Limit", _request.Limit.Value);

            if (_request.ProjectionExpression?.Count > 0)
                writer.WriteString("ProjectionExpression", string.Join(",", _request.ProjectionExpression));
            else if(_request.Select.HasValue)
                WriteSelect(writer, _request.Select.Value);

            if (!_request.ScanIndexForward)
                writer.WriteBoolean("ScanIndexForward", false);

            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                writer.WriteReturnConsumedCapacity(_request.ReturnConsumedCapacity);

            if (_request.ExclusiveStartKey != null)
                WriteExclusiveStartKey(writer, _request.ExclusiveStartKey);

            if (_request.ConsistentRead)
                writer.WriteBoolean("ConsistentRead", true);

            writer.WriteEndObject();

            return default;
        }
    }
}