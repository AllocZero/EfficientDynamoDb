using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Query;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Internal.Operations.Scan
{
    internal sealed class ScanHighLevelHttpContent : IterableHttpContent
    {
        private readonly DynamoDbContext _context;
        private readonly string? _tableName;
        private readonly BuilderNode? _node;

        public ScanHighLevelHttpContent(DynamoDbContext context, string? tableName, BuilderNode? node) : base("DynamoDB_20120810.Scan")
        {
            _context = context;
            _tableName = tableName;
            _node = node;
        }

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            var writeState = 0;
            
            if (_node != null)
            {
                FilterBase? filterExpression = null;
                BuilderNode? projectedAttributesStart = null;
                
                foreach (var node in _node)
                {
                    switch (node.Type)
                    {
                        case BuilderNodeType.FilterExpression:
                            filterExpression ??= ((FilterExpressionNode)node).Value;
                            break;
                        case BuilderNodeType.ProjectedAttributes:
                            projectedAttributesStart ??= node;
                            break;
                        case BuilderNodeType.TableName:
                            ((TableNameNode) node).WriteTableName(in ddbWriter, ref writeState, _context.Config.TableNamePrefix);
                            break;
                        default:
                            node.WriteValue(in ddbWriter, ref writeState);
                            break;
                    }
                }
                
                if(filterExpression != null || projectedAttributesStart != null)
                    WriteExpressions(in ddbWriter, filterExpression, projectedAttributesStart);
            }

            if (!writeState.IsBitSet(NodeBits.TableName))
                writer.WriteTableName(_context.Config.TableNamePrefix,
                    _tableName ?? throw new DdbException("Table name has to be specified either using the DynamoDbTable attribute or WithTableName extension method."));

            writer.WriteEndObject();

            return default;
        }

        private void WriteExpressions(in DdbWriter writer, FilterBase? filterExpression, BuilderNode? projectedAttributesStart)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            var visitor = new DdbExpressionVisitor(_context.Config.Metadata);
            try
            {
                var expressionValuesCount = 0;

                if (filterExpression != null)
                {
                    filterExpression.WriteExpressionStatement(ref builder, ref expressionValuesCount, visitor);
                    writer.JsonWriter.WriteString("FilterExpression", builder.GetBuffer());

                    builder.Clear();
                }

                if (projectedAttributesStart != null)
                    writer.JsonWriter.WriteProjectedAttributes(projectedAttributesStart, ref builder, visitor, _context.Config.Metadata);

                if (visitor.CachedAttributeNames.Count > 0)
                    writer.JsonWriter.WriteExpressionAttributeNames(ref builder, visitor.CachedAttributeNames);

                if (expressionValuesCount > 0)
                    writer.WriteExpressionAttributeValues(_context.Config.Metadata, visitor, filterExpression);
            }
            finally
            {
                builder.Dispose();
            }
        }
    }
}