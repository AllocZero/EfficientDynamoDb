using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.Context.Operations.BatchWriteItem;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.BatchWriteItem
{
    internal sealed class BatchWriteItemHighLevelHttpContent : BatchItemHttpContent
    {
        private const int OperationsLimit = 25;
        
        private readonly BuilderNode _node;
        private readonly string? _tableNamePrefix;
        private readonly DynamoDbContext _context;

        public BatchWriteItemHighLevelHttpContent(DynamoDbContext context, BuilderNode node, string? tableNamePrefix) : base("DynamoDB_20120810.BatchWriteItem")
        {
            _node = node;
            _tableNamePrefix = tableNamePrefix;
            _context = context;
        }
        
        protected override async ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            ddbWriter.JsonWriter.WriteStartObject();

            ddbWriter.JsonWriter.WritePropertyName("RequestItems");
            ddbWriter.JsonWriter.WriteStartObject();

            var operationsCount = 0;
            (DdbClassInfo ClassInfo, IBatchWriteBuilder Builder)[] sortedBuilders = ArrayPool<(DdbClassInfo ClassInfo, IBatchWriteBuilder Builder)>.Shared.Rent(OperationsLimit);

            try
            {
                var node = (BatchItemsNode<IBatchWriteBuilder>) _node;

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

            ddbWriter.JsonWriter.WriteEndObject();
        }

        private async Task WriteOperationsAsync(DdbWriter ddbWriter, (DdbClassInfo ClassInfo, IBatchWriteBuilder Builder)[] sortedBuilders, int operationsCount)
        {
            var writer = ddbWriter.JsonWriter;
            
            Array.Sort(sortedBuilders, 0, operationsCount, BatchWriteNodeComparer.Instance);

            string? currentTable = null;
            for (var i = 0; i < operationsCount; i++)
            {
                var (classInfo, builder) = sortedBuilders[i];
                if (currentTable != classInfo.TableName)
                {
                    if (i != 0)
                        writer.WriteEndArray();

                    WriteTableNameAsKey(writer, _tableNamePrefix, classInfo.TableName!);
                    writer.WriteStartArray();

                    currentTable = classInfo.TableName;
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
            
            public int Compare((DdbClassInfo ClassInfo, IBatchWriteBuilder Builder) x, (DdbClassInfo ClassInfo, IBatchWriteBuilder Builder) y) => string.Compare(x.ClassInfo.TableName, y.ClassInfo.TableName, StringComparison.Ordinal);
        }
    }
}