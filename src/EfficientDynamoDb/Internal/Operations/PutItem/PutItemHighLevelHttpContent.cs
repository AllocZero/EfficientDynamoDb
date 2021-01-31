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

            while (currentNode != null)
            {
                switch (currentNode.Type)
                {
                    case BuilderNodeType.Item:
                    {
                        var itemNode = ((ItemNode) currentNode);
                        var entityClassInfo = _metadata.GetOrAddClassInfo(itemNode.ItemType);
                        
                        writer.WriteTableName(_tablePrefix, entityClassInfo.TableName!);
                        
                        writer.WritePropertyName("Item");
                        await ddbWriter.WriteEntityAsync(entityClassInfo, itemNode.Value).ConfigureAwait(false);
                        break;
                    }
                    case BuilderNodeType.UpdateCondition:
                    {
                        ddbWriter.WriteConditionExpression(((UpdateConditionNode) currentNode).Value, _metadata);
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
        }
    }
}