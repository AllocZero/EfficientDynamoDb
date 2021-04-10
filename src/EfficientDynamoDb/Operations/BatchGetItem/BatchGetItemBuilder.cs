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

        protected BatchGetItemBuilder(PrimaryKeyNodeBase primaryKeyNode)
        {
            _primaryKeyNode = primaryKeyNode;
        }

        PrimaryKeyNodeBase IBatchGetItemBuilder.GetPrimaryKeyNode() => _primaryKeyNode ?? throw new DdbException("Can't execute empty batch get item request.");

        Type IBatchGetItemBuilder.GetEntityType() => typeof(TEntity);

        IBatchGetItemBuilder IBatchGetItemBuilder.WithTableName(string tableName)
        {
            throw new NotImplementedException();
        }

        public IBatchGetItemBuilder WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new BatchGetItemBuilder<TEntity>(new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, null));

        public IBatchGetItemBuilder WithPrimaryKey<TPk>(TPk pk)=>
            new BatchGetItemBuilder<TEntity>(new PartitionKeyNode<TPk>(pk, null));

        protected virtual string? GetTableName() => null;
    }

    internal sealed class BatchGetItemWithTableNameBuilder<TEntity> : BatchGetItemBuilder<TEntity> where TEntity : class
    {
        private readonly string _tableName;

        public BatchGetItemWithTableNameBuilder(PrimaryKeyNodeBase primaryKeyNode, string tableName) : base(primaryKeyNode)
        {
            _tableName = tableName;
        }

        protected override string? GetTableName() => _tableName;
    }
}