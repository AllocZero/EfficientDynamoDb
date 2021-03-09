using System;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    public readonly struct TransactConditionCheckBuilder<TEntity> : ITransactWriteItemBuilder where TEntity : class
    {
        private readonly BuilderNode? _node;

        BuilderNodeType ITransactWriteItemBuilder.NodeType =>  BuilderNodeType.TransactConditionCheckNode;

        BuilderNode ITransactWriteItemBuilder.GetNode() => _node ?? throw new DdbException("Transact write can't contain an empty operation.");

        Type ITransactWriteItemBuilder.GetEntityType() => typeof(TEntity);
        
        private TransactConditionCheckBuilder(BuilderNode? node)
        {
            _node = node;
        }

        public TransactConditionCheckBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new TransactConditionCheckBuilder<TEntity>(new PartitionKeyNode<TPk>(pk, _node));

        public TransactConditionCheckBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk)=>
            new TransactConditionCheckBuilder<TEntity>(new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public TransactConditionCheckBuilder<TEntity> WithCondition(FilterBase condition) =>
            new TransactConditionCheckBuilder<TEntity>(new ConditionNode(condition, _node));

        public TransactConditionCheckBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup) =>
            new TransactConditionCheckBuilder<TEntity>(new ConditionNode(conditionSetup(Condition.ForEntity<TEntity>()), _node));

        public TransactConditionCheckBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure) =>
            new TransactConditionCheckBuilder<TEntity>(new ReturnValuesOnConditionCheckFailureNode(returnValuesOnConditionCheckFailure, _node));
    }
}