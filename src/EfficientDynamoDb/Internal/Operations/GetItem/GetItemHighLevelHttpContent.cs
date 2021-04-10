using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Internal.Operations.GetItem
{
    internal sealed class GetItemHighLevelHttpContent : DynamoDbHttpContent
    {
        private readonly DynamoDbContext _context;
        private readonly DdbClassInfo _classInfo;
        private readonly BuilderNode _node;

        public GetItemHighLevelHttpContent(DynamoDbContext context, DdbClassInfo classInfo, BuilderNode node) : base("DynamoDB_20120810.GetItem")
        {
            _context = context;
            _classInfo = classInfo;
            _node = node;
        }

        protected override ValueTask WriteDataAsync(DdbWriter writer)
        {
            writer.JsonWriter.WriteStartObject();
            
            var writeState = 0;
            var projectionWritten = false;

            foreach (var node in _node)
            {
                switch (node.Type)
                {
                    case BuilderNodeType.PrimaryKey:
                        ((PrimaryKeyNodeBase) node).Write(in writer, _classInfo, ref writeState);
                        break;
                    case BuilderNodeType.ProjectedAttributes:
                        if (projectionWritten)
                            break;
                        
                        // ReSharper disable once StackAllocInsideLoop
                        var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);

                        try
                        {
                            var visitor = new DdbExpressionVisitor(_context.Config.Metadata);

                            writer.JsonWriter.WriteProjectedAttributes(node, ref builder, visitor, _context.Config.Metadata);

                            if (visitor.CachedAttributeNames.Count > 0)
                                writer.JsonWriter.WriteExpressionAttributeNames(ref builder, visitor.CachedAttributeNames);

                            projectionWritten = true;
                        }
                        finally
                        {
                            builder.Dispose();
                        }

                        break;
                    case BuilderNodeType.TableName:
                        ((TableNameNode) node).WriteTableName(in writer, ref writeState, _context.Config.TableNamePrefix);
                        break;
                    default:
                        node.WriteValue(in writer, ref writeState);
                        break;
                }
            }
            
            if(!writeState.IsBitSet(NodeBits.TableName))
                writer.JsonWriter.WriteTableName(_context.Config.TableNamePrefix, _classInfo.TableName!);
            
            writer.JsonWriter.WriteEndObject();

            return new ValueTask();
        }
    }
}