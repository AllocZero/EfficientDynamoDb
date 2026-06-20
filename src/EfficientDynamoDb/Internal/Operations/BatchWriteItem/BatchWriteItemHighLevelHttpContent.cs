using System;
using System.Buffers;
using System.Collections.Generic;
using System.Threading.Tasks;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Operations.BatchWriteItem;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Internal.Operations.BatchWriteItem
{
    internal sealed class BatchWriteItemHighLevelHttpContent : BatchItemHttpContent
    {
        private const int OperationsLimit = 25;
        
        private readonly BuilderNode _node;
        private readonly ITableNameFormatter? _tableNameFormatter;
        private readonly DynamoDbContext _context;

        public BatchWriteItemHighLevelHttpContent(DynamoDbContext context, BuilderNode node, ITableNameFormatter? tableNameFormatter) : base("DynamoDB_20120810.BatchWriteItem")
        {
            _node = node;
            _tableNameFormatter = tableNameFormatter;
            _context = context;
        }
        
        protected override async ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            ddbWriter.JsonWriter.WriteStartObject();

            var currentNode = _node;
            var writeState = 0;
            
            while (currentNode != null)
            {
                if (currentNode.Type == BuilderNodeType.BatchItems)
                    await WriteRequestItemsAsync(ddbWriter, (BatchItemsNode<IBatchWriteBuilder>) currentNode).ConfigureAwait(false);
                else
                    currentNode.WriteValue(in ddbWriter, ref writeState);

                currentNode = currentNode.Next;
            }

            ddbWriter.JsonWriter.WriteEndObject();
        }

        private async ValueTask WriteRequestItemsAsync(DdbWriter ddbWriter, BatchItemsNode<IBatchWriteBuilder> node)
        {
            ddbWriter.JsonWriter.WritePropertyName("RequestItems");
            ddbWriter.JsonWriter.WriteStartObject();

            var operationsCount = 0;
            var sortedBuilders = ArrayPool<(DdbClassInfo ClassInfo, IBatchWriteBuilder Builder)>.Shared.Rent(OperationsLimit);

            try
            {
                foreach (var item in node.Value)
                {
                    if (operationsCount == OperationsLimit)
                        throw new DdbException($"Batch write item request can't contain more than {OperationsLimit} operations.");

                    var classInfo = _context.Config.Metadata.GetOrAddClassInfo(item.GetEntityType());
                    sortedBuilders[operationsCount++] = (classInfo, item);
                }

                if (operationsCount != 0)
                    await WriteOperationsAsync(ddbWriter, sortedBuilders, operationsCount).ConfigureAwait(false);
            }
            finally
            {
                sortedBuilders.AsSpan(0, operationsCount).Clear();
                ArrayPool<(DdbClassInfo ClassInfo, IBatchWriteBuilder Builder)>.Shared.Return(sortedBuilders);
            }

            ddbWriter.JsonWriter.WriteEndObject();
        }

        private async ValueTask WriteOperationsAsync(DdbWriter ddbWriter, (DdbClassInfo ClassInfo, IBatchWriteBuilder Builder)[] sortedBuilders, int operationsCount)
        {
            var writer = ddbWriter.JsonWriter;
            
            Array.Sort(sortedBuilders, 0, operationsCount, BatchWriteNodeComparer.Instance);

            string? currentTable = null;
            for (var i = 0; i < operationsCount; i++)
            {
                var (classInfo, builder) = sortedBuilders[i];
                var tableName = builder.TableName ?? classInfo.TableName;
                if (currentTable != tableName)
                {
                    if (i != 0)
                        writer.WriteEndArray();

                    WriteTableNameAsKey(writer, _tableNameFormatter, tableName!);
                    writer.WriteStartArray();

                    currentTable = tableName;
                }

                switch (builder.NodeType)
                {
                    case BuilderNodeType.Item:
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName("PutRequest");
                        writer.WriteStartObject();

                        writer.WritePropertyName("Item");
                        
                        await ddbWriter.WriteEntityAsync(classInfo, ((BatchPutItemBuilder) builder).Entity).ConfigureAwait(false);

                        writer.WriteEndObject();
                        writer.WriteEndObject();

                        break;
                    }
                    case BuilderNodeType.PrimaryKey:
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName("DeleteRequest");
                        writer.WriteStartObject();

                        var writeState = 0;
                        ((BatchDeleteItemBuilder)builder).GetPrimaryKeyNode().Write(in ddbWriter, classInfo, ref writeState);

                        writer.WriteEndObject();
                        writer.WriteEndObject();

                        break;
                    }
                }
            }
            
            writer.WriteEndArray();
        }

        private sealed class BatchWriteNodeComparer : IComparer<(DdbClassInfo ClassInfo, IBatchWriteBuilder Builder)>
        {
            public static readonly BatchWriteNodeComparer Instance = new BatchWriteNodeComparer();

            public int Compare((DdbClassInfo ClassInfo, IBatchWriteBuilder Builder) x, (DdbClassInfo ClassInfo, IBatchWriteBuilder Builder) y) =>
                string.Compare(x.Builder.TableName ?? x.ClassInfo.TableName, y.Builder.TableName ?? y.ClassInfo.TableName, StringComparison.Ordinal);
        }
    }
}