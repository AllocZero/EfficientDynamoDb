using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;
using NotImplementedException = System.NotImplementedException;

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

            var hasAdd = false;
            var hasSet = false;
            var hasRemove = false;
            var hasDelete = false;
            BuilderNode? firstUpdateNode = null;
            BuilderNode? lastUpdateNode = null;

            while (currentNode != null)
            {
                switch (currentNode.Type)
                {
                    case BuilderNodeType.AddUpdate:
                    case BuilderNodeType.SetUpdate:
                    case BuilderNodeType.RemoveUpdate:
                    case BuilderNodeType.DeleteUpdate:
                    {
                        firstUpdateNode ??= currentNode;
                        lastUpdateNode = currentNode;

                        hasAdd = hasAdd || currentNode.Type == BuilderNodeType.AddUpdate;
                        hasSet = hasSet || currentNode.Type == BuilderNodeType.SetUpdate;
                        hasRemove = hasRemove || currentNode.Type == BuilderNodeType.RemoveUpdate;
                        hasDelete = hasDelete || currentNode.Type == BuilderNodeType.DeleteUpdate;
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
            
            if(firstUpdateNode != null)
                WriteUpdates(in ddbWriter, firstUpdateNode, lastUpdateNode!, hasAdd, hasSet, hasRemove, hasDelete);

            writer.WriteEndObject();
        }

        private void WriteUpdates(in DdbWriter ddbWriter, BuilderNode firstUpdateNode, BuilderNode lastUpdateNode, bool hasAdd, bool hasSet, bool hasRemove, bool hasDelete)
        {
            var expressionValuesCount = 0;
            var visitor = new DdbExpressionVisitor(_metadata);
            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            var isFirst = true;
            
            try
            {
                if (hasAdd)
                {
                    WriteSingleUpdateExpression(BuilderNodeType.AddUpdate, firstUpdateNode, lastUpdateNode, "ADD", ref builder, ref expressionValuesCount, visitor);
                    isFirst = false;
                }

                if (hasSet)
                {
                    if(!isFirst)
                        builder.Append(' ');
                    
                    WriteSingleUpdateExpression(BuilderNodeType.SetUpdate, firstUpdateNode, lastUpdateNode, "SET", ref builder, ref expressionValuesCount, visitor);
                    isFirst = false;
                }

                if (hasRemove)
                {
                    if(!isFirst)
                        builder.Append(' ');
                    
                    WriteSingleUpdateExpression(BuilderNodeType.RemoveUpdate, firstUpdateNode, lastUpdateNode, "REMOVE", ref builder, ref expressionValuesCount, visitor);
                    isFirst = false;
                }

                if (hasDelete)
                {
                    if(!isFirst)
                        builder.Append(' ');
                    
                    WriteSingleUpdateExpression(BuilderNodeType.DeleteUpdate, firstUpdateNode, lastUpdateNode, "DELETE", ref builder, ref expressionValuesCount, visitor);
                }
                
                ddbWriter.JsonWriter.WriteString("UpdateExpression", builder.GetBuffer());
            }
            finally
            {
                builder.Dispose();
            }

            if (visitor.CachedAttributeNames.Count > 0)
                ddbWriter.JsonWriter.WriteExpressionAttributeNames(visitor.CachedAttributeNames);

            if (expressionValuesCount > 0)
            {
                expressionValuesCount = 0;
                
                ddbWriter.JsonWriter.WritePropertyName("ExpressionAttributeValues");
                ddbWriter.JsonWriter.WriteStartObject();

                if (hasAdd)
                    WriteSingleUpdateAttributeValues(BuilderNodeType.AddUpdate, firstUpdateNode, lastUpdateNode, in ddbWriter, ref expressionValuesCount, visitor);
                
                if (hasSet)
                    WriteSingleUpdateAttributeValues(BuilderNodeType.SetUpdate, firstUpdateNode, lastUpdateNode, in ddbWriter, ref expressionValuesCount, visitor);
                
                if (hasDelete)
                    WriteSingleUpdateAttributeValues(BuilderNodeType.DeleteUpdate, firstUpdateNode, lastUpdateNode, in ddbWriter, ref expressionValuesCount, visitor);
                
                if (hasRemove)
                    WriteSingleUpdateAttributeValues(BuilderNodeType.RemoveUpdate, firstUpdateNode, lastUpdateNode, in ddbWriter, ref expressionValuesCount, visitor);

                ddbWriter.JsonWriter.WriteEndObject();
            }
        }

        private static void WriteSingleUpdateExpression(BuilderNodeType nodeType, BuilderNode firstUpdateNode, BuilderNode lastUpdateNode, string updateType, ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            builder.Append(updateType);
            builder.Append(' ');

            var isFirst = true;
            var currentNode = firstUpdateNode;
            
            while (true)
            {
                if (currentNode.Type == nodeType)
                {
                    if (!isFirst)
                        builder.Append(',');
                    
                    ((UpdateAttributeNode) currentNode).Value.WriteExpressionStatement(ref builder, ref valuesCount, visitor);
                    
                    isFirst = false;
                }
                
                if (currentNode == lastUpdateNode)
                    break;

                currentNode = currentNode.Next!;
            }
        }
        
        private void WriteSingleUpdateAttributeValues(BuilderNodeType nodeType, BuilderNode firstUpdateNode, BuilderNode lastUpdateNode, in DdbWriter ddbWriter, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var currentNode = firstUpdateNode;
            
            while (true)
            {
                if (currentNode.Type == nodeType)
                    ((UpdateAttributeNode) currentNode).Value.WriteAttributeValues(in ddbWriter, _metadata, ref valuesCount, visitor);
                
                if (currentNode == lastUpdateNode)
                    break;

                currentNode = currentNode.Next!;
            }
        }
    }
}