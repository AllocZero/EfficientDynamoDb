using System;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.BatchWriteItem
{
    internal class BatchDeleteItemBuilder : IBatchDeleteItemBuilder
    {
        private readonly Type _entityType;
        private readonly PrimaryKeyNodeBase? _primaryKeyNode;

        string? IBatchWriteBuilder.TableName => GetTableName();
        
        BuilderNodeType IBatchWriteBuilder.NodeType => BuilderNodeType.PrimaryKey;

        public BatchDeleteItemBuilder(Type entityType)
        {
            _entityType = entityType;
        }
        
        protected BatchDeleteItemBuilder(Type entityType, PrimaryKeyNodeBase? primaryKeyNode)
        {
            _entityType = entityType;
            _primaryKeyNode = primaryKeyNode;
        }

        IBatchDeleteItemBuilder IBatchDeleteItemBuilder.WithTableName(string tableName) => new BatchDeleteItemWithTableNameBuilder(_entityType, _primaryKeyNode, tableName);

        Type IBatchWriteBuilder.GetEntityType() => _entityType;

        public IBatchWriteBuilder WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) => new BatchDeleteItemBuilder(_entityType, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, null));

        public IBatchWriteBuilder WithPrimaryKey<TPk>(TPk pk) => new BatchDeleteItemBuilder(_entityType, new PartitionKeyNode<TPk>(pk, null));

        protected virtual string? GetTableName() => null;

        internal PrimaryKeyNodeBase GetPrimaryKeyNode() => _primaryKeyNode ?? throw new DdbException("Can't execute empty batch delete item request.");
    }

    internal sealed class BatchDeleteItemWithTableNameBuilder : BatchDeleteItemBuilder
    {
        private readonly string _tableName;

        public BatchDeleteItemWithTableNameBuilder(Type entityType, PrimaryKeyNodeBase? primaryKeyNode, string tableName) : base(entityType, primaryKeyNode)
        {
            _tableName = tableName;
        }

        protected override string? GetTableName() => _tableName;
    }
}