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
using EfficientDynamoDb.Internal.Operations.Shared;

namespace EfficientDynamoDb.Internal.Operations.BatchWriteItem
{
    internal sealed class BatchWriteItemHighLevelHttpContent : BatchItemHttpContent
    {
        private const int OperationsLimit = 25;
        
        private readonly BuilderNode _node;
        private readonly string? _tableNamePrefix;

        public BatchWriteItemHighLevelHttpContent(BuilderNode node, string? tableNamePrefix) : base("DynamoDB_20120810.BatchWriteItem")
        {
            _node = node;
            _tableNamePrefix = tableNamePrefix;
        }
        
        protected override async ValueTask WriteDataAsync(DdbWriter ddbWriter)
        {
            ddbWriter.JsonWriter.WriteStartObject();

            ddbWriter.JsonWriter.WritePropertyName("RequestItems");
            ddbWriter.JsonWriter.WriteStartObject();

            var currentNode = _node;
            var writeState = 0;

            var operationsCount = 0;
            EntityNodeBase[] sortedNodes = ArrayPool<EntityNodeBase>.Shared.Rent(OperationsLimit);

            try
            {
                while (currentNode != null)
                {
                    switch (currentNode.Type)
                    {
                        case BuilderNodeType.PrimaryKey:
                        case BuilderNodeType.Item:
                            if (operationsCount == OperationsLimit)
                                throw new DdbException($"Batch write item request can't contain more than {OperationsLimit} operations.");
                            
                            sortedNodes[operationsCount++] = (EntityNodeBase) currentNode;
                            break;
                        default:
                            currentNode.WriteValue(in ddbWriter, ref writeState);
                            break;
                    }

                    currentNode = currentNode.Next;
                }

                if (operationsCount != 0)
                    await WriteOperationsAsync(ddbWriter, sortedNodes, operationsCount, writeState).ConfigureAwait(false);
            }
            finally
            {
                sortedNodes.AsSpan(0, operationsCount).Clear();
                ArrayPool<EntityNodeBase>.Shared.Return(sortedNodes);
            }

            ddbWriter.JsonWriter.WriteEndObject();

            ddbWriter.JsonWriter.WriteEndObject();
        }

        private async Task WriteOperationsAsync(DdbWriter ddbWriter, EntityNodeBase[] sortedNodes, int operationsCount, int writeState)
        {
            var writer = ddbWriter.JsonWriter;
            
            Array.Sort(sortedNodes, 0, operationsCount, EntityNodeBaseComparer.Instance);

            string? currentTable = null;
            for (var i = 0; i < operationsCount; i++)
            {
                var node = sortedNodes[i];
                if (currentTable != node.EntityClassInfo.TableName)
                {
                    if (i != 0)
                        writer.WriteEndArray();

                    WriteTableNameAsKey(writer, _tableNamePrefix, node.EntityClassInfo.TableName!);
                    writer.WriteStartArray();

                    currentTable = node.EntityClassInfo.TableName;
                }

                switch (node.Type)
                {
                    case BuilderNodeType.Item:
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName("PutRequest");
                        writer.WriteStartObject();

                        writer.WritePropertyName("Item");
                        await ddbWriter.WriteEntityAsync(node.EntityClassInfo, ((ItemNode) node).Value).ConfigureAwait(false);

                        writer.WriteEndObject();
                        writer.WriteEndObject();

                        break;
                    }
                    case BuilderNodeType.PrimaryKey:
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName("DeleteRequest");
                        writer.WriteStartObject();

                        node.WriteValue(in ddbWriter, ref writeState);

                        writer.WriteEndObject();
                        writer.WriteEndObject();

                        break;
                    }
                }
            }
            
            writer.WriteEndArray();
        }

        private sealed class EntityNodeBaseComparer : IComparer<EntityNodeBase>
        {
            public static readonly EntityNodeBaseComparer Instance = new EntityNodeBaseComparer();
            
            public int Compare(EntityNodeBase x, EntityNodeBase y) => string.Compare(x.EntityClassInfo.TableName, y.EntityClassInfo.TableName, StringComparison.Ordinal);
        }
    }
}