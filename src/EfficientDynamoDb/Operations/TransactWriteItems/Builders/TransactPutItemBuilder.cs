using System;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    internal sealed class TransactPutItemBuilder<TEntity> : TransactWriteItemBuilder<TEntity>, ITransactPutItemBuilder<TEntity> where TEntity : class
    {
        protected override BuilderNodeType NodeType => BuilderNodeType.TransactPutItemNode;

        public TransactPutItemBuilder()
        {
        }

        public TransactPutItemBuilder(BuilderNode? node) : base(node)
        {
        }

        public ITransactPutItemBuilder<TEntity> WithCondition(FilterBase condition) =>
            new TransactPutItemBuilder<TEntity>(new ConditionNode(condition, Node));

        public ITransactPutItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup) =>
            new TransactPutItemBuilder<TEntity>(new ConditionNode(conditionSetup(Condition.ForEntity<TEntity>()), Node));

        public ITransactPutItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure) =>
            new TransactPutItemBuilder<TEntity>(new ReturnValuesOnConditionCheckFailureNode(returnValuesOnConditionCheckFailure, Node));
    }
}