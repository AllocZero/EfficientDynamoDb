using System;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    internal sealed class TransactDeleteItemBuilder<TEntity> : TransactWriteItemBuilder<TEntity>, ITransactDeleteItemBuilder<TEntity> where TEntity : class
    {
        protected override BuilderNodeType NodeType => BuilderNodeType.TransactDeleteItemNode;

        public TransactDeleteItemBuilder()
        {
        }

        public TransactDeleteItemBuilder(BuilderNode? node) : base(node)
        {
        }

        public ITransactDeleteItemBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new TransactDeleteItemBuilder<TEntity>(new PartitionKeyNode<TPk>(pk, Node));

        public ITransactDeleteItemBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk)=>
            new TransactDeleteItemBuilder<TEntity>(new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, Node));

        public ITransactDeleteItemBuilder<TEntity> WithCondition(FilterBase condition) =>
            new TransactDeleteItemBuilder<TEntity>(new ConditionNode(condition, Node));

        public ITransactDeleteItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup) =>
            new TransactDeleteItemBuilder<TEntity>(new ConditionNode(conditionSetup(Condition.ForEntity<TEntity>()), Node));

        public ITransactDeleteItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure) =>
            new TransactDeleteItemBuilder<TEntity>(new ReturnValuesOnConditionCheckFailureNode(returnValuesOnConditionCheckFailure, Node));
    }
}