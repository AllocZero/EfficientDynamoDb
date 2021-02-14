using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.TransactGetItems
{
    internal sealed class TransactGetItemsHighLevelHttpContent : DynamoDbHttpContent
    {
        private readonly string? _tableNamePrefix;
        private readonly DynamoDbContextMetadata _metadata;
        private readonly BuilderNode _node;

        public TransactGetItemsHighLevelHttpContent(string? tableNamePrefix, DynamoDbContextMetadata metadata, BuilderNode node) : base("DynamoDB_20120810.TransactGetItems")
        {
            _tableNamePrefix = tableNamePrefix;
            _metadata = metadata;
            _node = node;
        }

        protected override async ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            BuilderNode? returnConsumedCapacityNode = null;
            
            writer.WritePropertyName("TransactItems");
            
            writer.WriteStartArray();

            var currentNode = _node;
            while (currentNode != null)
            {
                var result = WriteGetItems(in ddbWriter, currentNode);
                returnConsumedCapacityNode ??= result.ReturnConsumedCapacityNode;
                currentNode = result.NextNode;
                
                if (ddbWriter.ShouldFlush)
                    await ddbWriter.FlushAsync().ConfigureAwait(false);
            }

            writer.WriteEndArray();

            var writeState = 0;
            returnConsumedCapacityNode?.WriteValue(in ddbWriter, ref writeState);

            writer.WriteEndObject();
        }

        private (BuilderNode? NextNode, BuilderNode? ReturnConsumedCapacityNode) WriteGetItems(in DdbWriter ddbWriter, BuilderNode node)
        {
            BuilderNode? returnConsumedCapacityNode = null;
            
            var writer = ddbWriter.JsonWriter;
            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            try
            {
                DdbExpressionVisitor? visitor = null;
                
                var currentNode = node;
                while (currentNode != null)
                {
                    if (currentNode.Type != BuilderNodeType.GetItemNode)
                    {
                        returnConsumedCapacityNode ??= currentNode;
                        currentNode = currentNode.Next;
                        continue;
                    }
                    
                    var getItemNode = (GetItemNode) currentNode;

                    writer.WriteStartObject();

                    writer.WritePropertyName("Get");
                    writer.WriteStartObject();

                    writer.WriteTableName(_tableNamePrefix, getItemNode.ClassInfo.TableName!);

                    var getWriteState = 0;
                    var projectionWritten = false;
                    
                    foreach (var getNode in getItemNode.Value)
                    {
                        switch (getNode.Type)
                        {
                            case BuilderNodeType.PrimaryKey:
                                ((PrimaryKeyNodeBase) getNode).Write(in ddbWriter, getItemNode.ClassInfo, ref getWriteState);
                                break;
                            case BuilderNodeType.ProjectedAttributes:
                                if (projectionWritten)
                                    break;

                                visitor ??= new DdbExpressionVisitor(_metadata);
                                writer.WriteProjectedAttributes(getNode, ref builder, visitor);

                                if (visitor.CachedAttributeNames.Count > 0)
                                    writer.WriteExpressionAttributeNames(ref builder, visitor.CachedAttributeNames);

                                builder.Clear();
                                visitor.Clear();
                                projectionWritten = true;
                                break;
                        }
                    }

                    writer.WriteEndObject();

                    writer.WriteEndObject();

                    currentNode = currentNode.Next;

                    if (ddbWriter.ShouldFlush)
                        return (currentNode, returnConsumedCapacityNode);
                }
            }
            finally
            {
                builder.Dispose();
            }

            return (null, returnConsumedCapacityNode);
        }
    }
}