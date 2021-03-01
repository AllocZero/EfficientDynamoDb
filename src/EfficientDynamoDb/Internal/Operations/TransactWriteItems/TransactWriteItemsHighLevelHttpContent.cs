using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.TransactWriteItems
{
    internal sealed class TransactWriteItemsHighLevelHttpContent : DynamoDbHttpContent
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode _node;

        public TransactWriteItemsHighLevelHttpContent(DynamoDbContext context, BuilderNode node) : base("DynamoDB_20120810.TransactWriteItems")
        {
            _context = context;
            _node = node;
        }

        protected override async ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            writer.WritePropertyName("TransactItems");
            
            writer.WriteStartArray();

            var hasPrimitiveNodes = false;
            DdbExpressionVisitor? visitor = null;
            
            var currentNode = _node;
            while (currentNode != null)
            {
                var result = WriteItems(in ddbWriter, ref visitor, currentNode);
                currentNode = result.NextNode;
                hasPrimitiveNodes = hasPrimitiveNodes || result.HasPrimitiveNodes;

                if (ddbWriter.ShouldFlush)
                    await ddbWriter.FlushAsync().ConfigureAwait(false);
            }
            
            writer.WriteEndArray();

            if (hasPrimitiveNodes)
            {
                var writeState = 0;
                foreach (var node in _node)
                {
                    if (node.Type != BuilderNodeType.Primitive)
                        continue;
                    
                    node.WriteValue(in ddbWriter, ref writeState);
                }
            }

            writer.WriteEndObject();
        }
        
        private (BuilderNode? NextNode, bool HasPrimitiveNodes) WriteItems(in DdbWriter ddbWriter, ref DdbExpressionVisitor? visitor, BuilderNode node)
        {
            var hasPrimitiveNodes = false;
            var currentNode = node;
            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            try
            {
                while (currentNode != null)
                {
                    switch (currentNode.Type)
                    {
                        case BuilderNodeType.TransactConditionCheckNode:
                            WriteConditionCheck(in ddbWriter, ref builder, ref visitor, (TransactWriteItemNode) currentNode);
                            break;
                        case BuilderNodeType.TransactDeleteItemNode:
                            WriteDeleteItem(in ddbWriter, ref builder, ref visitor, (TransactWriteItemNode) currentNode);
                            break;
                        case BuilderNodeType.TransactPutItemNode:
                            WritePutItem(in ddbWriter, ref builder, ref visitor, (TransactWriteItemNode) currentNode);
                            break;
                        case BuilderNodeType.TransactUpdateItemNode:
                            WriteUpdateItem(in ddbWriter, ref builder, ref visitor, (TransactWriteItemNode) currentNode);
                            break;
                        default:
                            hasPrimitiveNodes = true;
                            break;
                    }

                    currentNode = currentNode.Next;

                    if (ddbWriter.ShouldFlush)
                        return (currentNode, hasPrimitiveNodes);
                }
            }
            finally
            {
                builder.Dispose();
            }

            return (currentNode, hasPrimitiveNodes);
        }

        private void WriteUpdateItem(in DdbWriter ddbWriter, ref NoAllocStringBuilder builder, ref DdbExpressionVisitor? visitor, TransactWriteItemNode updateNode)
        {
            ddbWriter.JsonWriter.WriteStartObject();
            
            ddbWriter.JsonWriter.WritePropertyName("Update");
            
            ddbWriter.JsonWriter.WriteStartObject();
            
            ddbWriter.JsonWriter.WriteTableName(_context.Config.TableNamePrefix, updateNode.ClassInfo.TableName!);
            
            visitor ??= new DdbExpressionVisitor(_context.Config.Metadata);
            ddbWriter.WriteUpdateItem(_context.Config.Metadata, ref builder, visitor, updateNode.ClassInfo, updateNode.Value);

            ddbWriter.JsonWriter.WriteEndObject();
            
            ddbWriter.JsonWriter.WriteEndObject();
        }

        private void WriteDeleteItem(in DdbWriter ddbWriter, ref NoAllocStringBuilder builder, ref DdbExpressionVisitor? visitor, TransactWriteItemNode deleteNode)
        {
            ddbWriter.JsonWriter.WriteStartObject();
            
            ddbWriter.JsonWriter.WritePropertyName("Delete");
            
            ddbWriter.JsonWriter.WriteStartObject();
            
            ddbWriter.JsonWriter.WriteTableName(_context.Config.TableNamePrefix, deleteNode.ClassInfo.TableName!);
            
            var writeState = 0;
            foreach (var node in deleteNode.Value)
            {
                switch (node.Type)
                {
                    case BuilderNodeType.PrimaryKey:
                        ((PrimaryKeyNodeBase)node).Write(in ddbWriter, deleteNode.ClassInfo, ref writeState);
                        break;
                    case BuilderNodeType.Condition:
                        if (writeState.IsBitSet(NodeBits.Condition))
                            break;

                        visitor ??= new DdbExpressionVisitor(_context.Config.Metadata);
                        ddbWriter.WriteConditionExpression(ref builder, visitor, ((ConditionNode) node).Value, _context.Config.Metadata);

                        writeState = writeState.SetBit(NodeBits.Condition);
                        break;
                    default:
                        node.WriteValue(in ddbWriter, ref writeState);
                        break;
                }
            }

            ddbWriter.JsonWriter.WriteEndObject();
            
            ddbWriter.JsonWriter.WriteEndObject();
        }
        
        private void WritePutItem(in DdbWriter ddbWriter, ref NoAllocStringBuilder builder, ref DdbExpressionVisitor? visitor, TransactWriteItemNode putNode)
        {
            ddbWriter.JsonWriter.WriteStartObject();
            
            ddbWriter.JsonWriter.WritePropertyName("Put");
            
            ddbWriter.JsonWriter.WriteStartObject();

            ddbWriter.JsonWriter.WriteTableName(_context.Config.TableNamePrefix, putNode.ClassInfo.TableName!);
            
            var writeState = 0;
            foreach (var node in putNode.Value)
            {
                switch (node.Type)
                {
                    case BuilderNodeType.Item:
                        if (writeState.IsBitSet(NodeBits.Item))
                            break;
                        
                        var itemNode = ((ItemNode) node);

                        ddbWriter.JsonWriter.WritePropertyName("Item");
                        ddbWriter.WriteEntity(itemNode.EntityClassInfo, itemNode.Value);

                        writeState = writeState.SetBit(NodeBits.Item);
                        break;
                    case BuilderNodeType.Condition:
                        if (writeState.IsBitSet(NodeBits.Condition))
                            break;

                        visitor ??= new DdbExpressionVisitor(_context.Config.Metadata);
                        ddbWriter.WriteConditionExpression(ref builder, visitor, ((ConditionNode) node).Value, _context.Config.Metadata);

                        writeState = writeState.SetBit(NodeBits.Condition);
                        break;
                    default:
                        node.WriteValue(in ddbWriter, ref writeState);
                        break;
                }
            }

            ddbWriter.JsonWriter.WriteEndObject();
            
            ddbWriter.JsonWriter.WriteEndObject();
        }

        private void WriteConditionCheck(in DdbWriter ddbWriter, ref NoAllocStringBuilder builder, ref DdbExpressionVisitor? visitor, TransactWriteItemNode conditionNode)
        {
            ddbWriter.JsonWriter.WriteStartObject();
            
            ddbWriter.JsonWriter.WritePropertyName("ConditionCheck");
            
            ddbWriter.JsonWriter.WriteStartObject();

            ddbWriter.JsonWriter.WriteTableName(_context.Config.TableNamePrefix, conditionNode.ClassInfo.TableName!);

            var writeState = 0;
            foreach (var node in conditionNode.Value)
            {
                switch (node.Type)
                {
                    case BuilderNodeType.PrimaryKey:
                        ((PrimaryKeyNodeBase)node).Write(in ddbWriter, conditionNode.ClassInfo, ref writeState);
                        break;
                    case BuilderNodeType.Condition:
                        if (writeState.IsBitSet(NodeBits.Condition))
                            break;

                        visitor ??= new DdbExpressionVisitor(_context.Config.Metadata);
                        ddbWriter.WriteConditionExpression(ref builder, visitor, ((ConditionNode) node).Value, _context.Config.Metadata);

                        writeState = writeState.SetBit(NodeBits.Condition);
                        break;
                    default:
                        node.WriteValue(in ddbWriter, ref writeState);
                        break;
                }
            }
            
            ddbWriter.JsonWriter.WriteEndObject();
            
            ddbWriter.JsonWriter.WriteEndObject();
        }
    }
}