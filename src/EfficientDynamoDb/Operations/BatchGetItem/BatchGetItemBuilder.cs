using System;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations.Query;

namespace EfficientDynamoDb.Operations.BatchGetItem
{
    internal sealed class BatchGetItemBuilder<TEntity> : IBatchGetItemBuilder where TEntity : class
    {
        private readonly PrimaryKeyNodeBase? _primaryKeyNode;
        
        public BatchGetItemBuilder()
        {
        }

        private BatchGetItemBuilder(PrimaryKeyNodeBase? primaryKeyNode)
        {
            _primaryKeyNode = primaryKeyNode;
        }

        PrimaryKeyNodeBase IBatchGetItemBuilder.GetPrimaryKeyNode() => _primaryKeyNode ?? throw new DdbException("Can't execute empty batch get item request.");

        Type IBatchGetItemBuilder.GetEntityType() => typeof(TEntity);

        public IBatchGetItemBuilder WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk) =>
            new BatchGetItemBuilder<TEntity>(new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, null));

        public IBatchGetItemBuilder WithPrimaryKey<TPk>(TPk pk)=>
            new BatchGetItemBuilder<TEntity>(new PartitionKeyNode<TPk>(pk, null));
    }
}