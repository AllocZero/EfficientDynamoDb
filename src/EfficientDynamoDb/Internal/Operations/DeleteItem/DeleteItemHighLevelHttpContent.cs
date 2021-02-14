using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.DeleteItem
{
    internal sealed class DeleteItemHighLevelHttpContent : DynamoDbHttpContent
    {
        private readonly DdbClassInfo _classInfo;
        private readonly string? _tablePrefix;
        private readonly BuilderNode _node;
        private readonly DynamoDbContextMetadata _metadata;

        public DeleteItemHighLevelHttpContent(DdbClassInfo classInfo, string? tablePrefix, BuilderNode node, DynamoDbContextMetadata metadata) : base("DynamoDB_20120810.DeleteItem")
        {
            _classInfo = classInfo;
            _tablePrefix = tablePrefix;
            _node = node;
            _metadata = metadata;
        }
        
        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            writer.WriteTableName(_tablePrefix, _classInfo.TableName!);

            var writeState = 0;
            
            foreach (var node in _node)
            {
                switch (node.Type)
                {
                    case BuilderNodeType.PrimaryKey:
                        ((PrimaryKeyNodeBase) node).Write(in ddbWriter, _classInfo, ref writeState);
                        break;
                    case BuilderNodeType.Condition:
                        if (writeState.IsBitSet(NodeBits.Condition))
                            break;
                        
                        ddbWriter.WriteConditionExpression(((ConditionNode) node).Value, _metadata);

                        writeState = writeState.SetBit(NodeBits.Condition);
                        break;
                    default:
                        node.WriteValue(in ddbWriter, ref writeState);
                        break;
                }
            }
            
            writer.WriteEndObject();
            
            return default;
        }
    }
}