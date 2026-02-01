using System.Collections.Generic;
using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.TransactGetItems;

namespace EfficientDynamoDb.Internal.Operations.TransactGetItems
{
    internal sealed class TransactGetItemsHighLevelHttpContent : DynamoDbHttpContent
    {
        private readonly DynamoDbContext _context;
        private readonly BuilderNode _node;

        public TransactGetItemsHighLevelHttpContent(DynamoDbContext context, BuilderNode node) : base("DynamoDB_20120810.TransactGetItems")
        {
            _context = context;
            _node = node;
        }

        protected override async ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            var writeState = 0;
            var itemsProcessed = false;
           
            
            foreach (var node in _node)
            {
                if (node.Type != BuilderNodeType.BatchItems)
                {
                    node.WriteValue(in ddbWriter, ref writeState);
                    continue;
                }

                if (itemsProcessed)
                    continue;
                
                writer.WritePropertyName("TransactItems");
                writer.WriteStartArray();
                
                var itemsNode = (BatchItemsNode<ITransactGetItemRequestBuilder>) node;
                using var itemsEnumerator = itemsNode.Value.GetEnumerator();
                DdbExpressionVisitor? visitor = null;
                
                while (WriteGetItems(in ddbWriter, ref visitor, itemsEnumerator))
                {
                    if (ddbWriter.ShouldFlush)
                        await ddbWriter.FlushAsync().ConfigureAwait(false);
                }
                
                writer.WriteEndArray();

                itemsProcessed = true;
            }

            writer.WriteEndObject();
        }

        private bool WriteGetItems(in DdbWriter ddbWriter, ref DdbExpressionVisitor? visitor, IEnumerator<ITransactGetItemRequestBuilder> enumerator)
        {
            var writer = ddbWriter.JsonWriter;
            var builder = new NoAllocStringBuilder(stackalloc char[NoAllocStringBuilder.MaxStackAllocSize], true);
            try
            {
                while (enumerator.MoveNext())
                {
                    var classInfo = _context.Config.Metadata.GetOrAddClassInfo(enumerator.Current!.GetEntityType());
                    writer.WriteStartObject();

                    writer.WritePropertyName("Get");
                    writer.WriteStartObject();
                    
                    var getWriteState = 0;
                    var projectionWritten = false;

                    foreach (var getNode in enumerator.Current!.GetNode())
                    {
                        switch (getNode.Type)
                        {
                            case BuilderNodeType.PrimaryKey:
                                ((PrimaryKeyNodeBase) getNode).Write(in ddbWriter, classInfo, ref getWriteState);
                                break;
                            case BuilderNodeType.ProjectedAttributes:
                                if (projectionWritten)
                                    break;

                                visitor ??= new DdbExpressionVisitor(_context.Config.Metadata);
                                writer.WriteProjectedAttributes(getNode, ref builder, visitor, _context.Config.Metadata);

                                if (visitor.CachedAttributeNames.Count > 0)
                                    writer.WriteExpressionAttributeNames(ref builder, visitor.CachedAttributeNames);

                                builder.Clear();
                                visitor.Clear();
                                projectionWritten = true;
                                break;
                            case BuilderNodeType.TableName:
                                ((TableNameNode) getNode).WriteTableName(in ddbWriter, ref getWriteState, _context.Config.TableNameFormatter);
                                break;
                        }
                    }
                    
                    if(!getWriteState.IsBitSet(NodeBits.TableName))
                        writer.WriteTableName(_context.Config.TableNameFormatter, classInfo.TableName!);

                    writer.WriteEndObject();

                    writer.WriteEndObject();

                    if (ddbWriter.ShouldFlush)
                        return true;
                }
            }
            finally
            {
                builder.Dispose();
            }

            return false;
        }
    }
}