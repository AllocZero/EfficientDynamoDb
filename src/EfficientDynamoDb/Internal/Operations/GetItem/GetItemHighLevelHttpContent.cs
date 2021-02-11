using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.GetItem
{
    internal sealed class GetItemHighLevelHttpContent : DynamoDbHttpContent
    {
        private readonly DdbClassInfo _classInfo;
        private readonly string? _tablePrefix;
        private readonly DynamoDbContextMetadata _metadata;
        private readonly BuilderNode _node;

        public GetItemHighLevelHttpContent(DdbClassInfo classInfo, string? tablePrefix, DynamoDbContextMetadata metadata, BuilderNode node) : base("DynamoDB_20120810.GetItem")
        {
            _classInfo = classInfo;
            _tablePrefix = tablePrefix;
            _metadata = metadata;
            _node = node;
        }

        protected override ValueTask WriteDataAsync(DdbWriter writer)
        {
            writer.JsonWriter.WriteStartObject();
            
            var writeState = 0;
            var projectionWritten = false;

            writer.JsonWriter.WriteTableName(_tablePrefix, _classInfo.TableName!);
            
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
                        var visitor = new DdbExpressionVisitor(_metadata);

                        writer.JsonWriter.WriteProjectedAttributes(node, ref builder, visitor);
                        
                        if(visitor.CachedAttributeNames.Count > 0)
                            writer.JsonWriter.WriteExpressionAttributeNames(ref builder, visitor.CachedAttributeNames);

                        projectionWritten = true;
                        break;
                    default:
                        node.WriteValue(in writer, ref writeState);
                        break;
                }
            }
            
            writer.JsonWriter.WriteEndObject();

            return new ValueTask();
        }
    }
}