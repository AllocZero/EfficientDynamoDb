using System;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
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
            new TransactDeleteItemBuilder<TEntity>(new ConditionNode(conditionSetup(Filter.ForEntity<TEntity>()), Node));

        public ITransactDeleteItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure) =>
            new TransactDeleteItemBuilder<TEntity>(new ReturnValuesOnConditionCheckFailureNode(returnValuesOnConditionCheckFailure, Node));
    }
}