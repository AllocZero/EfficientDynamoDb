using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Internal.Operations.DeleteItem
{
    internal sealed class DeleteItemHighLevelHttpContent : DynamoDbHttpContent
    {
        private readonly DdbClassInfo _classInfo;
        private readonly DynamoDbContext _context;
        private readonly BuilderNode _node;

        public DeleteItemHighLevelHttpContent(DynamoDbContext context, DdbClassInfo classInfo, BuilderNode node) : base("DynamoDB_20120810.DeleteItem")
        {
            _classInfo = classInfo;
            _context = context;
            _node = node;
        }
        
        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

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
                        
                        ddbWriter.WriteConditionExpression(((ConditionNode) node).Value, _context.Config.Metadata);

                        writeState = writeState.SetBit(NodeBits.Condition);
                        break;
                    case BuilderNodeType.TableName:
                        ((TableNameNode) node).WriteTableName(in ddbWriter, ref writeState, _context.Config.TableNamePrefix);
                        break;
                    default:
                        node.WriteValue(in ddbWriter, ref writeState);
                        break;
                }
            }
            
            if(!writeState.IsBitSet(NodeBits.TableName))
                writer.WriteTableName(_context.Config.TableNamePrefix, _classInfo.TableName!);
            
            writer.WriteEndObject();
            
            return default;
        }
    }
}