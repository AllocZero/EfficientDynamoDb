using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Requests;
using EfficientDynamoDb.Context.Requests.Query;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Internal.Builder
{
    public class QueryHttpContent : DynamoDbHttpContent
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
                WriteExpressionAttributeNames(writer);
            
            if (_request.ExpressionAttributeValues?.Count > 0)
                WriteExpressionAttributeValues(writer);
            
            if(_request.Limit.HasValue)
                writer.WriteNumber("Limit", _request.Limit.Value);

            if (_request.ProjectionExpression?.Count > 0)
                writer.WriteString("ProjectionExpression", string.Join(",", _request.ProjectionExpression));
            else
                WriteSelect(writer);

            if (!_request.ScanIndexForward)
                writer.WriteBoolean("ScanIndexForward", false);

            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                WriteReturnConsumedCapacity(writer);

            if (_request.ExclusiveStartKey != null)
                WriteExclusiveStartKey(writer);

            if (_request.ConsistentRead)
                writer.WriteBoolean("ConsistentRead", true);

            writer.WriteEndObject();

            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteExclusiveStartKey(Utf8JsonWriter writer)
        {
           writer.WritePropertyName("ExclusiveStartKey");
           
           WriteAttributesDictionary(writer, _request.ExclusiveStartKey!);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteReturnConsumedCapacity(Utf8JsonWriter writer)
        {
            writer.WriteString("ReturnConsumedCapacity", _request.ReturnConsumedCapacity switch
            {
                ReturnConsumedCapacity.Indexes => "INDEXES",
                ReturnConsumedCapacity.Total => "TOTAL",
                ReturnConsumedCapacity.None => "NONE"
            });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteExpressionAttributeNames(Utf8JsonWriter writer)
        {
            writer.WritePropertyName("ExpressionAttributeNames");
                
            writer.WriteStartObject();

            foreach (var pair in _request.ExpressionAttributeNames!)
                writer.WriteString(pair.Key, pair.Value);
                
            writer.WriteEndObject();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteExpressionAttributeValues(Utf8JsonWriter writer)
        {
            writer.WritePropertyName("ExpressionAttributeValues");

            WriteAttributesDictionary(writer, _request.ExpressionAttributeValues!);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteSelect(Utf8JsonWriter writer)
        {
            var selectValue = _request.Select switch
            {
                Select.AllAttributes => "ALL_ATTRIBUTES",
                Select.AllProjectedAttributes => "ALL_PROJECTED_ATTRIBUTES",
                Select.Count => "COUNT",
                Select.SpecificAttributes => "SPECIFIC_ATTRIBUTES"
            };
            
            writer.WriteString("Select", selectValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void WriteAttributesDictionary(Utf8JsonWriter writer, IReadOnlyDictionary<string, AttributeValue> attributesDictionary)
        {
            writer.WriteStartObject();

            foreach (var pair in attributesDictionary)
            {
                writer.WritePropertyName(pair.Key);

                pair.Value.Write(writer);
            }
            
            writer.WriteEndObject();
        }
    }
}