using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Operations.Query
{
    internal class QueryHighLevelHttpContent : IterableHttpContent
    {
        private readonly QueryHighLevelRequest _request;
        private readonly string? _tablePrefix;
        private readonly DynamoDbContextMetadata _metadata;

        public QueryHighLevelHttpContent(QueryHighLevelRequest request, string? tablePrefix, DynamoDbContextMetadata metadata) : base("DynamoDB_20120810.Query")
        {
            _request = request;
            _tablePrefix = tablePrefix;
            _metadata = metadata;
        }

        protected override ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();

            writer.WriteTableName(_tablePrefix, _request.TableName);

            var expressionStatementBuilder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            var cachedExpressionNames = new HashSet<string>();
            var expressionValuesCount = 0;
            WriteCondition(writer, ref expressionStatementBuilder, cachedExpressionNames, ref expressionValuesCount, "KeyConditionExpression");
            
            if(_request.IndexName != null)
                writer.WriteString("IndexName", _request.IndexName);

            if (_request.FilterExpression != null)
                WriteCondition(writer, ref expressionStatementBuilder, cachedExpressionNames, ref expressionValuesCount, "FilterExpression");

            if (cachedExpressionNames.Count > 0)
                writer.WriteExpressionAttributeNames(cachedExpressionNames);

            if (expressionValuesCount > 0)
                writer.WriteExpressionAttributeValues(_metadata, _request.KeyExpression, _request.FilterExpression);
            
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

        private void WriteCondition(Utf8JsonWriter writer, ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount,
            string propertyName)
        {
            _request.KeyExpression!.WriteExpressionStatement(ref builder, cachedNames, ref valuesCount);
            writer.WriteString(propertyName, builder.GetBuffer());

            builder.Clear();
        }
    }
}