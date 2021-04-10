using System;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.BatchGetItem
{
    internal class BatchGetItemBuilder<TEntity> : IBatchGetItemBuilder where TEntity : class
    {
        private readonly PrimaryKeyNodeBase? _primaryKeyNode;

        string? IBatchGetItemBuilder.TableName => GetTableName();

        public BatchGetItemBuilder()
        {
        }

        protected BatchGetItemBuilder(PrimaryKeyNodeBase? primaryKeyNode)
        {
            _primaryKeyNode = primaryKeyNode;
        }

        PrimaryKeyNodeBase IBatchGetItemBuilder.GetPrimaryKeyNode() => _primaryKeyNode ?? throw new DdbException("Can't execute empty batch get item request.");

        Type IBatchGetItemBuilder.GetEntityType() => typeof(TEntity);

        IBatchGetItemBuilder IBatchGetItemBuilder.WithTableName(string tableName) => new BatchGetItemWithTableNameBuilder<TEntity>(_primaryKeyNode, tableName);

        public virtual IBatchGetItemBuilder WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new BatchGetItemBuilder<TEntity>(new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, null));

        public virtual IBatchGetItemBuilder WithPrimaryKey<TPk>(TPk pk) =>
            new BatchGetItemBuilder<TEntity>(new PartitionKeyNode<TPk>(pk, null));

        protected virtual string? GetTableName() => null;
    }

    internal sealed class BatchGetItemWithTableNameBuilder<TEntity> : BatchGetItemBuilder<TEntity> where TEntity : class
    {
        private readonly string _tableName;

        public BatchGetItemWithTableNameBuilder(PrimaryKeyNodeBase? primaryKeyNode, string tableName) : base(primaryKeyNode)
        {
            _tableName = tableName;
        }

        public override IBatchGetItemBuilder WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new BatchGetItemWithTableNameBuilder<TEntity>(new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, null), _tableName);

        public override IBatchGetItemBuilder WithPrimaryKey<TPk>(TPk pk) =>
            new BatchGetItemWithTableNameBuilder<TEntity>(new PartitionKeyNode<TPk>(pk, null), _tableName);

        protected override string? GetTableName() => _tableName;
    }
}