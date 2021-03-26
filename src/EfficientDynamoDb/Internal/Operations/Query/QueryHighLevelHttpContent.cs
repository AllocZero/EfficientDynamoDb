using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Internal.Operations.Query
{
    internal class QueryHighLevelHttpContent : IterableHttpContent
    {
        private readonly DynamoDbContext _context;
        private readonly string _tableName;
        private readonly BuilderNode _node;

        public QueryHighLevelHttpContent(DynamoDbContext context, string tableName, BuilderNode node) : base("DynamoDB_20120810.Query")
        {
            _context = context;
            _tableName = tableName;
            _node = node;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            writer.WriteTableName(_context.Config.TableNamePrefix, _tableName);

            var currentNode = _node;
            var wereExpressionsWritten = false;
            var writeState = 0;

            while (currentNode != null)
            {
                switch (currentNode.Type)
                {
                    case BuilderNodeType.KeyExpression:
                    case BuilderNodeType.FilterExpression:
                    case BuilderNodeType.ProjectedAttributes:
                    {
                        if (wereExpressionsWritten)
                            break;

                        WriteExpressions(in ddbWriter, currentNode);
                        wereExpressionsWritten = true;
                        break;
                    }
                    default:
                    {
                        currentNode.WriteValue(in ddbWriter, ref writeState);
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
            FilterBase? keyExpression = null;
            FilterBase? filterExpression = null;
            BuilderNode? projectedAttributesStart = null;

            BuilderNode? currentNode = node;
            while (currentNode != null && (keyExpression == null || filterExpression == null || projectedAttributesStart == null))
            {
                switch (currentNode.Type)
                {
                    case BuilderNodeType.KeyExpression when keyExpression == null:
                        keyExpression = ((KeyExpressionNode) currentNode).Value;
                        break;
                    case BuilderNodeType.FilterExpression when filterExpression == null:
                        filterExpression = ((FilterExpressionNode) currentNode).Value;
                        break;
                    case BuilderNodeType.ProjectedAttributes when projectedAttributesStart == null:
                        projectedAttributesStart = currentNode;
                        break;
                }
                
                currentNode = currentNode.Next;
            }
            
            var expressionStatementBuilder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            var visitor = new DdbExpressionVisitor(_context.Config.Metadata);
            try
            {
                var expressionValuesCount = 0;
                WriteCondition(writer.JsonWriter, keyExpression!, ref expressionStatementBuilder, visitor, ref expressionValuesCount, "KeyConditionExpression");

                if (filterExpression != null)
                    WriteCondition(writer.JsonWriter, filterExpression, ref expressionStatementBuilder, visitor, ref expressionValuesCount, "FilterExpression");

                if(projectedAttributesStart != null)
                    writer.JsonWriter.WriteProjectedAttributes(projectedAttributesStart, ref expressionStatementBuilder, visitor, _context.Config.Metadata);
                
                if (visitor.CachedAttributeNames.Count > 0)
                    writer.JsonWriter.WriteExpressionAttributeNames(ref expressionStatementBuilder, visitor.CachedAttributeNames);

                if (expressionValuesCount > 0)
                    writer.WriteExpressionAttributeValues(_context.Config.Metadata, visitor, keyExpression, filterExpression);
            }
            finally
            {
                expressionStatementBuilder.Dispose();
            }
        }

        private static void WriteCondition(Utf8JsonWriter writer, FilterBase filterBase, ref NoAllocStringBuilder builder, DdbExpressionVisitor visitor, ref int valuesCount,
            string propertyName)
        {
            filterBase.WriteExpressionStatement(ref builder, ref valuesCount, visitor);
            writer.WriteString(propertyName, builder.GetBuffer());

            builder.Clear();
        }
    }
}