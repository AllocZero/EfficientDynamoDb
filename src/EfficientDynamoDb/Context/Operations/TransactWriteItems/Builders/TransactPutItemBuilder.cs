using System;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
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
            new TransactPutItemBuilder<TEntity>(new ConditionNode(conditionSetup(Filter.ForEntity<TEntity>()), Node));

        public ITransactPutItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure) =>
            new TransactPutItemBuilder<TEntity>(new ReturnValuesOnConditionCheckFailureNode(returnValuesOnConditionCheckFailure, Node));
    }
}