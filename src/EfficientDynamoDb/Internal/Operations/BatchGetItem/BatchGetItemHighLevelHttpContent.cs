using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.BatchGetItem;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Internal.Operations.BatchGetItem
{
    internal sealed class BatchGetItemHighLevelHttpContent : BatchItemHttpContent
    {
        private const int OperationsLimit = 100;
        
        private readonly BuilderNode _node;
        private readonly DynamoDbContext _context;

        public BatchGetItemHighLevelHttpContent(DynamoDbContext context, BuilderNode node) : base("DynamoDB_20120810.BatchGetItem")
        {
            _node = node;
            _context = context;
        }

        protected override async ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            var writer = ddbWriter.JsonWriter;
            writer.WriteStartObject();

            writer.WritePropertyName("RequestItems");
            writer.WriteStartObject();
            
            if (_node is BatchItemsNode<IBatchGetItemBuilder> rawItemsNode)
            {
                await WriteItems(ddbWriter, rawItemsNode).ConfigureAwait(false);
            }
            else
            {
                var tablesNode = (BatchItemsNode<IBatchGetTableBuilder>) _node;

                await WriteTables(ddbWriter, tablesNode).ConfigureAwait(false);
            }

            writer.WriteEndObject();

            writer.WriteEndObject();
        }

        private async Task WriteItems(DdbWriter ddbWriter, BatchItemsNode<IBatchGetItemBuilder> itemsNode)
        {
            var writer = ddbWriter.JsonWriter;
            var operationsCount = 0;
            (DdbClassInfo ClassInfo, IBatchGetItemBuilder Builder)[] sortedBuilders = ArrayPool<(DdbClassInfo ClassInfo, IBatchGetItemBuilder Builder)>.Shared.Rent(OperationsLimit);

            try
            {
                foreach (var item in itemsNode.Value)
                {
                    if (operationsCount == OperationsLimit)
                        throw new DdbException($"Batch get item request can't contain more than {OperationsLimit} operations.");

                    var classInfo = _context.Config.Metadata.GetOrAddClassInfo(item.GetEntityType());
                    sortedBuilders[operationsCount++] = (classInfo, item);
                }

                if (operationsCount == 0)
                    return;
                
                Array.Sort(sortedBuilders, 0, operationsCount, BatchGetNodeComparer.Instance);
                
                string? currentTable = null;
                for (var i = 0; i < operationsCount; i++)
                {
                    var (classInfo, builder) = sortedBuilders[i];
                    if (currentTable != classInfo.TableName)
                    {
                        if (i != 0)
                        {
                            writer.WriteEndArray();
                            writer.WriteEndObject();
                        }

                        WriteTableNameAsKey(writer, _context.Config.TableNamePrefix, classInfo.TableName!);
                        writer.WriteStartObject();
                        
                        writer.WritePropertyName("Keys");
                        writer.WriteStartArray();

                        currentTable = classInfo.TableName;
                    }

                    builder.GetPrimaryKeyNode().WriteValueWithoutKey(in ddbWriter, _context.Config.Metadata.GetOrAddClassInfo(builder.GetEntityType()));

                    if (ddbWriter.ShouldFlush)
                        await ddbWriter.FlushAsync().ConfigureAwait(false);
                }
                
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
            finally
            {
                sortedBuilders.AsSpan(0, operationsCount).Clear();
                ArrayPool<(DdbClassInfo ClassInfo, IBatchGetItemBuilder Builder)>.Shared.Return(sortedBuilders);
            }
        }

        private async Task WriteTables(DdbWriter ddbWriter, BatchItemsNode<IBatchGetTableBuilder> tablesNode)
        {
            var writer = ddbWriter.JsonWriter;
            DdbExpressionVisitor? visitor = null;
            
            foreach (var tableBuilder in tablesNode.Value)
            {
                var tableClassInfo = _context.Config.Metadata.GetOrAddClassInfo(tableBuilder.GetTableType());

                WriteTableNameAsKey(writer, _context.Config.TableNamePrefix, tableClassInfo.TableName!);
                writer.WriteStartObject();

              

                var hasProjections = false;
                var itemsWritten = false;
                var writeState = 0;

                foreach (var tableNode in tableBuilder.GetNode())
                {
                    switch (tableNode.Type)
                    {
                        case BuilderNodeType.Primitive:
                            tableNode.WriteValue(in ddbWriter, ref writeState);
                            break;
                        case BuilderNodeType.ProjectedAttributes:
                            hasProjections = true;
                            break;
                        case BuilderNodeType.BatchItems:
                            if (itemsWritten)
                                break;

                            writer.WritePropertyName("Keys");
                            writer.WriteStartArray();
                            
                            var itemsNode = (BatchItemsNode<IBatchGetItemBuilder>) tableNode;

                            foreach (var itemBuilder in itemsNode.Value)
                            {
                                itemBuilder.GetPrimaryKeyNode().WriteValueWithoutKey(in ddbWriter, _context.Config.Metadata.GetOrAddClassInfo(itemBuilder.GetEntityType()));

                                if (ddbWriter.ShouldFlush)
                                    await ddbWriter.FlushAsync().ConfigureAwait(false);
                            }
                            
                            writer.WriteEndArray();

                            itemsWritten = true;
                            break;
                    }
                }
                
                if (hasProjections)
                    writer.WriteProjectionExpression(ref visitor, tableBuilder.GetNode(), _context.Config.Metadata);

                writer.WriteEndObject();
            }
        }
        
        private sealed class BatchGetNodeComparer : IComparer<(DdbClassInfo ClassInfo, IBatchGetItemBuilder Builder)>
        {
            public static readonly BatchGetNodeComparer Instance = new BatchGetNodeComparer();
            
            public int Compare((DdbClassInfo ClassInfo, IBatchGetItemBuilder Builder) x, (DdbClassInfo ClassInfo, IBatchGetItemBuilder Builder) y) => string.Compare(x.ClassInfo.TableName, y.ClassInfo.TableName, StringComparison.Ordinal);
        }
    }
}