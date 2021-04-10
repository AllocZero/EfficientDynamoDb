using System;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.BatchWriteItem
{
    internal sealed class BatchDeleteItemBuilder : IBatchDeleteItemBuilder
    {
        private readonly Type _entityType;
        private readonly PrimaryKeyNodeBase? _primaryKeyNode;

        string? IBatchWriteBuilder.TableName { get; set; }

        public BatchDeleteItemBuilder(Type entityType)
        {
            _entityType = entityType;
        }
        
        private BatchDeleteItemBuilder(Type entityType, PrimaryKeyNodeBase? primaryKeyNode)
        {
            _entityType = entityType;
            _primaryKeyNode = primaryKeyNode;
        }

        BuilderNodeType IBatchWriteBuilder.NodeType => BuilderNodeType.PrimaryKey;

        Type IBatchWriteBuilder.GetEntityType() => _entityType;

        public IBatchWriteBuilder WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) => new BatchDeleteItemBuilder(_entityType, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, null));

        public IBatchWriteBuilder WithPrimaryKey<TPk>(TPk pk) => new BatchDeleteItemBuilder(_entityType, new PartitionKeyNode<TPk>(pk, null));

        internal PrimaryKeyNodeBase GetPrimaryKeyNode() => _primaryKeyNode ?? throw new DdbException("Can't execute empty batch delete item request.");
    }
}