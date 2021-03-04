using System;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    internal sealed class TransactConditionCheckBuilder<TEntity> : TransactWriteItemBuilder<TEntity>, ITransactConditionCheckBuilder<TEntity> where TEntity : class
    {
        protected override BuilderNodeType NodeType => BuilderNodeType.TransactConditionCheckNode;

        public TransactConditionCheckBuilder()
        {
        }

        public TransactConditionCheckBuilder(BuilderNode? node) : base(node)
        {
        }

        public ITransactConditionCheckBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new TransactConditionCheckBuilder<TEntity>(new PartitionKeyNode<TPk>(pk, Node));

        public ITransactConditionCheckBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk)=>
            new TransactConditionCheckBuilder<TEntity>(new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, Node));

        public ITransactConditionCheckBuilder<TEntity> WithCondition(FilterBase condition) =>
            new TransactConditionCheckBuilder<TEntity>(new ConditionNode(condition, Node));

        public ITransactConditionCheckBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup) =>
            new TransactConditionCheckBuilder<TEntity>(new ConditionNode(conditionSetup(Condition.ForEntity<TEntity>()), Node));

        public ITransactConditionCheckBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure) =>
            new TransactConditionCheckBuilder<TEntity>(new ReturnValuesOnConditionCheckFailureNode(returnValuesOnConditionCheckFailure, Node));
    }
}