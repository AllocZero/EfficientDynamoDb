using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static partial class Utf8JsonWriterExtensions
    {
        public static void WriteUpdateItem(this in DdbWriter ddbWriter, DynamoDbContextMetadata metadata, ref NoAllocStringBuilder builder, DdbExpressionVisitor visitor, DdbClassInfo classInfo, BuilderNode? node)
        {
            var currentNode = node;

            var writeState = 0;
            var hasAdd = false;
            var hasSet = false;
            var hasRemove = false;
            var hasDelete = false;
            BuilderNode? firstUpdateNode = null;
            BuilderNode? lastUpdateNode = null;
            FilterBase? updateCondition = null;

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
                    case BuilderNodeType.Condition:
                    {
                        updateCondition ??= ((ConditionNode) currentNode).Value;
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
                WriteUpdates(in ddbWriter, metadata, ref builder, visitor, firstUpdateNode, lastUpdateNode!, hasAdd, hasSet, hasRemove, hasDelete, updateCondition);
            
            builder.Clear();
            visitor.Clear();
        }

        private static void WriteUpdates(in DdbWriter ddbWriter, DynamoDbContextMetadata metadata, ref NoAllocStringBuilder builder, DdbExpressionVisitor visitor,
            BuilderNode firstUpdateNode, BuilderNode lastUpdateNode, bool hasAdd, bool hasSet, bool hasRemove, bool hasDelete, FilterBase? updateCondition)
        {
            var expressionValuesCount = 0;
            var isFirst = true;

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
                if (!isFirst)
                    builder.Append(' ');

                WriteSingleUpdateExpression(BuilderNodeType.SetUpdate, firstUpdateNode, lastUpdateNode, "SET", ref builder, ref expressionValuesCount, visitor);
                isFirst = false;
            }

            if (hasRemove)
            {
                if (!isFirst)
                    builder.Append(' ');

                WriteSingleUpdateExpression(BuilderNodeType.RemoveUpdate, firstUpdateNode, lastUpdateNode, "REMOVE", ref builder, ref expressionValuesCount, visitor);
                isFirst = false;
            }

            if (hasDelete)
            {
                if (!isFirst)
                    builder.Append(' ');

                WriteSingleUpdateExpression(BuilderNodeType.DeleteUpdate, firstUpdateNode, lastUpdateNode, "DELETE", ref builder, ref expressionValuesCount, visitor);
            }

            ddbWriter.JsonWriter.WriteString("UpdateExpression", builder.GetBuffer());

            builder.Clear();

            if (visitor.CachedAttributeNames.Count > 0)
                ddbWriter.JsonWriter.WriteExpressionAttributeNames(ref builder, visitor.CachedAttributeNames);

            if (expressionValuesCount > 0)
            {
                expressionValuesCount = 0;

                ddbWriter.JsonWriter.WritePropertyName("ExpressionAttributeValues");
                ddbWriter.JsonWriter.WriteStartObject();

                updateCondition?.WriteAttributeValues(in ddbWriter, metadata, ref expressionValuesCount, visitor);

                if (hasAdd)
                    WriteSingleUpdateAttributeValues(metadata, BuilderNodeType.AddUpdate, firstUpdateNode, lastUpdateNode, in ddbWriter, ref expressionValuesCount, visitor);

                if (hasSet)
                    WriteSingleUpdateAttributeValues(metadata, BuilderNodeType.SetUpdate, firstUpdateNode, lastUpdateNode, in ddbWriter, ref expressionValuesCount, visitor);

                if (hasDelete)
                    WriteSingleUpdateAttributeValues(metadata, BuilderNodeType.DeleteUpdate, firstUpdateNode, lastUpdateNode, in ddbWriter, ref expressionValuesCount, visitor);

                if (hasRemove)
                    WriteSingleUpdateAttributeValues(metadata, BuilderNodeType.RemoveUpdate, firstUpdateNode, lastUpdateNode, in ddbWriter, ref expressionValuesCount, visitor);

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
        
        private static void WriteSingleUpdateAttributeValues(DynamoDbContextMetadata metadata, BuilderNodeType nodeType, BuilderNode firstUpdateNode, BuilderNode lastUpdateNode, in DdbWriter ddbWriter, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var currentNode = firstUpdateNode;
            
            while (true)
            {
                if (currentNode.Type == nodeType)
                    ((UpdateAttributeNode) currentNode).Value.WriteAttributeValues(in ddbWriter, metadata, ref valuesCount, visitor);
                
                if (currentNode == lastUpdateNode)
                    break;

                currentNode = currentNode.Next!;
            }
        }
    }
}