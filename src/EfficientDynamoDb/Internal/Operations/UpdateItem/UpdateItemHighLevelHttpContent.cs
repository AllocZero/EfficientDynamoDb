using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.UpdateItem
{
    internal sealed class UpdateItemHighLevelHttpContent<TEntity> : DynamoDbHttpContent where TEntity : class
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

        protected override ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            ddbWriter.JsonWriter.WriteStartObject();

            var classInfo = _metadata.GetOrAddClassInfo(typeof(TEntity));
            var currentNode = _node;

            var writeState = 0;
            var hasAdd = false;
            var hasSet = false;
            var hasRemove = false;
            var hasDelete = false;
            BuilderNode? firstUpdateNode = null;
            BuilderNode? lastUpdateNode = null;
            FilterBase? updateCondition = null;

            ddbWriter.JsonWriter.WriteTableName(_tablePrefix, classInfo.TableName!);

            while (currentNode != null)
            {
                switch (currentNode.Type)
                {
                    case BuilderNodeType.PrimaryKey:
                    {
                        ((PrimaryKeyNodeBase) currentNode).Write(in ddbWriter, classInfo, ref writeState);
                        break;
                    }
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
                        updateCondition ??= ((UpdateConditionNode) currentNode).Value;
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
            
            if(firstUpdateNode != null)
                WriteUpdates(in ddbWriter, firstUpdateNode, lastUpdateNode!, hasAdd, hasSet, hasRemove, hasDelete, updateCondition);

            ddbWriter.JsonWriter.WriteEndObject();

            return new ValueTask();
        }

        private void WriteUpdates(in DdbWriter ddbWriter, BuilderNode firstUpdateNode, BuilderNode lastUpdateNode, bool hasAdd, bool hasSet, bool hasRemove, bool hasDelete, FilterBase? updateCondition)
        {
            var expressionValuesCount = 0;
            var visitor = new DdbExpressionVisitor(_metadata);
            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            var isFirst = true;
            
            try
            {
                if (updateCondition != null)
                {
                    updateCondition.WriteExpressionStatement(ref builder, ref expressionValuesCount, visitor);
                    ddbWriter.JsonWriter.WriteString("ConditionExpression", builder.GetBuffer());
                    builder.Clear();
                }
                
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
                
                builder.Clear();
                
                if (visitor.CachedAttributeNames.Count > 0)
                    ddbWriter.JsonWriter.WriteExpressionAttributeNames(ref builder, visitor.CachedAttributeNames);
            }
            finally
            {
                builder.Dispose();
            }

            if (expressionValuesCount > 0)
            {
                expressionValuesCount = 0;
                
                ddbWriter.JsonWriter.WritePropertyName("ExpressionAttributeValues");
                ddbWriter.JsonWriter.WriteStartObject();

                updateCondition?.WriteAttributeValues(in ddbWriter, _metadata, ref expressionValuesCount, visitor);

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