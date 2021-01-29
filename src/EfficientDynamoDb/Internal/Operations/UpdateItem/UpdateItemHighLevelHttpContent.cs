using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.UpdateItem
{
    internal sealed class UpdateItemHighLevelHttpContent: DynamoDbHttpContent
    {
        private readonly BuilderNode? _node;
        private readonly string? _tablePrefix;
        private readonly DynamoDbContextMetadata _metadata;

        public UpdateItemHighLevelHttpContent(string? tablePrefix, DynamoDbContextMetadata metadata, BuilderNode? node)
            : base("DynamoDB_20120810.UpdateItem")
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
                    // TODO: Write other parts
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