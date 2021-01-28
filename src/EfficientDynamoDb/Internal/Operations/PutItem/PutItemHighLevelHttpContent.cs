using System.Collections.Generic;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.PutItem;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;
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
                        var item = ((ItemNode) currentNode).Value;
                        var entityClassInfo = _metadata.GetOrAddClassInfo(item.GetType());
                        
                        writer.WriteTableName(_tablePrefix, entityClassInfo.TableName!);
                        
                        writer.WritePropertyName("Item");
                        await ddbWriter.WriteEntityAsync(entityClassInfo, item).ConfigureAwait(false);
                        break;
                    }
                    case BuilderNodeType.UpdateCondition:
                    {
                        WriteCondition(in ddbWriter, ((UpdateConditionNode)currentNode).Value);
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

        private void WriteCondition(in DdbWriter writer, FilterBase condition)
        {
            var expressionValuesCount = 0;

            var visitor = new DdbExpressionVisitor(_metadata);
            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            try
            {
                condition.WriteExpressionStatement(ref builder, ref expressionValuesCount, visitor);
                writer.JsonWriter.WriteString("ConditionExpression", builder.GetBuffer());
            }
            finally
            {
                builder.Dispose();
            }

            if (visitor.CachedAttributeNames.Count > 0)
                writer.JsonWriter.WriteExpressionAttributeNames(visitor.CachedAttributeNames);

            if (expressionValuesCount > 0)
                writer.WriteExpressionAttributeValues(_metadata, visitor, condition);
        }
    }
}