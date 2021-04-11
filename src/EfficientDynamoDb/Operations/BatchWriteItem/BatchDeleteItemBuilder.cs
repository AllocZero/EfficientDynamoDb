using System;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.BatchWriteItem
{
    internal class BatchDeleteItemBuilder : IBatchDeleteItemBuilder
    {
        private readonly PrimaryKeyNodeBase? _primaryKeyNode;
        protected readonly Type EntityType;

        string? IBatchWriteBuilder.TableName => GetTableName();
        
        BuilderNodeType IBatchWriteBuilder.NodeType => BuilderNodeType.PrimaryKey;

        public BatchDeleteItemBuilder(Type entityType)
        {
            EntityType = entityType;
        }
        
        protected BatchDeleteItemBuilder(Type entityType, PrimaryKeyNodeBase? primaryKeyNode)
        {
            EntityType = entityType;
            _primaryKeyNode = primaryKeyNode;
        }

        IBatchDeleteItemBuilder IBatchDeleteItemBuilder.WithTableName(string tableName) => new BatchDeleteItemWithTableNameBuilder(EntityType, _primaryKeyNode, tableName);

        Type IBatchWriteBuilder.GetEntityType() => EntityType;

        public virtual IBatchWriteBuilder WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) => new BatchDeleteItemBuilder(EntityType, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, null));

        public virtual IBatchWriteBuilder WithPrimaryKey<TPk>(TPk pk) => new BatchDeleteItemBuilder(EntityType, new PartitionKeyNode<TPk>(pk, null));

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

        public override IBatchWriteBuilder WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new BatchDeleteItemWithTableNameBuilder(EntityType, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, null), _tableName);

        public override IBatchWriteBuilder WithPrimaryKey<TPk>(TPk pk) => new BatchDeleteItemWithTableNameBuilder(EntityType, new PartitionKeyNode<TPk>(pk, null), _tableName);

        protected override string? GetTableName() => _tableName;
    }
}