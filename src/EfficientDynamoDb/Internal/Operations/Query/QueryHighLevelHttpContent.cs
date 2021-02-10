using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;

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
            var writeState = 0;

            while (currentNode != null)
            {
                switch (currentNode.Type)
                {
                    case BuilderNodeType.KeyExpression:
                    case BuilderNodeType.FilterExpression:
                    case BuilderNodeType.Projection:
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
            // limit -> key -> filter
            FilterBase? keyExpression = null;
            FilterBase? filterExpression = null;
            DdbClassInfo? projectionClassInfo = null;

            BuilderNode? currentNode = node;
            while (currentNode != null && (keyExpression == null || filterExpression == null || projectionClassInfo == null))
            {
                switch (currentNode.Type)
                {
                    case BuilderNodeType.KeyExpression:
                        keyExpression = ((KeyExpressionNode) currentNode).Value;
                        break;
                    case BuilderNodeType.FilterExpression:
                        filterExpression = ((FilterExpressionNode) currentNode).Value;
                        break;
                    case BuilderNodeType.Projection:
                        projectionClassInfo = ((ProjectedAttributesNode) currentNode).Value;
                        break;
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

                if(projectionClassInfo != null)
                    WriteProjectedAttributes(writer.JsonWriter, projectionClassInfo, ref expressionStatementBuilder, visitor.CachedAttributeNames.Count);
                
                if (visitor.CachedAttributeNames.Count > 0 || projectionClassInfo?.AttributesMap.Count > 0)
                    writer.JsonWriter.WriteExpressionAttributeNames(ref expressionStatementBuilder, visitor.CachedAttributeNames, projectionClassInfo?.AttributesMap.Keys);

                if (expressionValuesCount > 0)
                    writer.WriteExpressionAttributeValues(_metadata, visitor, keyExpression, filterExpression);
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

        private static void WriteProjectedAttributes(Utf8JsonWriter writer, DdbClassInfo classInfo, ref NoAllocStringBuilder builder, int startIndex)
        {
            var isFirst = true;
            var index = startIndex;

            foreach (var _ in classInfo.AttributesMap.Keys)
            {
                if (!isFirst)
                    builder.Append(',');

                builder.Append("#f");
                builder.Append(index++);

                isFirst = false;
            }

            if (classInfo.AttributesMap.Count > 0)
                writer.WriteString("ProjectionExpression", builder.GetBuffer());

            builder.Clear();
        }
    }
}