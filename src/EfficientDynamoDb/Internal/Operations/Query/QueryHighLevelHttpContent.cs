using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Operations.Query
{
    internal class QueryHighLevelHttpContent : IterableHttpContent
    {
        private readonly string _tableName;
        private readonly string? _tablePrefix;
        private readonly DynamoDbContextMetadata _metadata;
        private readonly BuilderNode? _node;

        public QueryHighLevelHttpContent(string tableName, string? tablePrefix, DynamoDbContextMetadata metadata, BuilderNode? node) : base("DynamoDB_20120810.Query")
        {
            _tableName = tableName;
            _tablePrefix = tablePrefix;
            _metadata = metadata;
            _node = node;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            writer.WriteTableName(_tablePrefix, _tableName);

            var currentNode = _node;
            var wereExpressionsWritten = false;

            while (currentNode != null)
            {
                switch (currentNode.Type)
                {
                    case BuilderNodeType.KeyExpression when !wereExpressionsWritten:
                    case BuilderNodeType.FilterExpression when !wereExpressionsWritten:
                    {
                        WriteExpressions(in ddbWriter, currentNode);
                        wereExpressionsWritten = true;
                        break;
                    }
                    default:
                    {
                        currentNode.WriteValue(in ddbWriter);
                        break;
                    }
                }
                
                currentNode = currentNode.Next;
            }

            writer.WriteEndObject();

            return default;
        }

        private void WriteExpressions(in DdbWriter writer, BuilderNode node)
        {
            // limit -> key -> filter
            FilterBase? keyExpression = null;
            FilterBase? filterExpression = null;

            BuilderNode? currentNode = node;
            while (currentNode != null && (keyExpression == null || filterExpression == null))
            {
                switch (node.Type)
                {
                    case BuilderNodeType.KeyExpression:
                    {
                        keyExpression = ((KeyExpressionNode) currentNode).Value;
                        break;
                    }
                    case BuilderNodeType.FilterExpression:
                    {
                        keyExpression = ((FilterExpressionNode) currentNode).Value;
                        break;
                    }
                }
                
                currentNode = currentNode.Next;
            }
            
            var expressionStatementBuilder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            var visitor = new DdbExpressionVisitor(_metadata);
            try
            {
                var expressionValuesCount = 0;
                WriteCondition(writer.JsonWriter, keyExpression!, ref expressionStatementBuilder, visitor, ref expressionValuesCount, "KeyConditionExpression");

                if (filterExpression != null)
                    WriteCondition(writer.JsonWriter, filterExpression, ref expressionStatementBuilder, visitor, ref expressionValuesCount, "FilterExpression");

                if (visitor.CachedAttributeNames.Count > 0)
                    writer.JsonWriter.WriteExpressionAttributeNames(visitor.CachedAttributeNames);

                if (expressionValuesCount > 0)
                    writer.WriteExpressionAttributeValues(_metadata, visitor, keyExpression, filterExpression);
            }
            finally
            {
                expressionStatementBuilder.Dispose();
            }
        }

        private void WriteCondition(Utf8JsonWriter writer, FilterBase filterBase, ref NoAllocStringBuilder builder, DdbExpressionVisitor visitor, ref int valuesCount,
            string propertyName)
        {
            filterBase.WriteExpressionStatement(ref builder, ref valuesCount, visitor);
            writer.WriteString(propertyName, builder.GetBuffer());

            builder.Clear();
        }
    }
}