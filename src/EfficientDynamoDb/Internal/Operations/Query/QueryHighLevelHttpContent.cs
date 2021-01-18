using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Factories;
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

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            writer.WriteTableName(_tablePrefix, _request.TableName);

            var expressionStatementBuilder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            var visitor = new DdbExpressionVisitor(_metadata);
            try
            {
                var expressionValuesCount = 0;
                WriteCondition(writer, ref expressionStatementBuilder, visitor, ref expressionValuesCount, "KeyConditionExpression");

                if (_request.IndexName != null)
                    writer.WriteString("IndexName", _request.IndexName);

                if (_request.FilterExpression != null)
                    WriteCondition(writer, ref expressionStatementBuilder, visitor, ref expressionValuesCount, "FilterExpression");

                if (visitor.CachedAttributeNames.Count > 0)
                    writer.WriteExpressionAttributeNames(visitor.CachedAttributeNames);

                if (expressionValuesCount > 0)
                    ddbWriter.WriteExpressionAttributeValues(_metadata, visitor, _request.KeyExpression, _request.FilterExpression);
            }
            finally
            {
                expressionStatementBuilder.Dispose();
            }

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

            if (_request.PaginationToken != null)
                ddbWriter.WritePaginationToken(_request.PaginationToken);

            if (_request.ConsistentRead)
                writer.WriteBoolean("ConsistentRead", true);

            writer.WriteEndObject();

            return default;
        }

        private void WriteCondition(Utf8JsonWriter writer, ref NoAllocStringBuilder builder, DdbExpressionVisitor visitor, ref int valuesCount,
            string propertyName)
        {
            _request.KeyExpression!.WriteExpressionStatement(ref builder, ref valuesCount, visitor);
            writer.WriteString(propertyName, builder.GetBuffer());

            builder.Clear();
        }
    }
}