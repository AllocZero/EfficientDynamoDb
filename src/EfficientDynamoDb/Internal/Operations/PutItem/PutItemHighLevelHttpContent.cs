using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.PutItem
{
    internal sealed class PutItemHighLevelHttpContent : DynamoDbHttpContent
    {
        private readonly BuilderNode? _node;
        private readonly string? _tablePrefix;
        private readonly DynamoDbContextMetadata _metadata;

        public PutItemHighLevelHttpContent(string? tablePrefix, DynamoDbContextMetadata metadata, BuilderNode? node)
            : base("DynamoDB_20120810.PutItem")
        {
            _node = node;
            _tablePrefix = tablePrefix;
            _metadata = metadata;
        }

        protected override async ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            var currentNode = _node;
            var writeState = 0;

            while (currentNode != null)
            {
                switch (currentNode.Type)
                {
                    case BuilderNodeType.Item:
                    {
                        if (writeState.IsBitSet(NodeBits.Item))
                            break;
                        
                        var itemNode = ((ItemNode) currentNode);
                        
                        writer.WriteTableName(_tablePrefix, itemNode.EntityClassInfo.TableName!);
                        
                        writer.WritePropertyName("Item");
                        await ddbWriter.WriteEntityAsync(itemNode.EntityClassInfo, itemNode.Value).ConfigureAwait(false);

                        writeState = writeState.SetBit(NodeBits.Item);
                        break;
                    }
                    case BuilderNodeType.Condition:
                    {
                        if (writeState.IsBitSet(NodeBits.Condition))
                            break;
                        
                        ddbWriter.WriteConditionExpression(((ConditionNode) currentNode).Value, _metadata);

                        writeState = writeState.SetBit(NodeBits.Condition);
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
        }
    }
}