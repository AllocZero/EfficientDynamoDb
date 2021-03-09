using System;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    public readonly struct TransactDeleteItemBuilder<TEntity> : ITransactWriteItemBuilder where TEntity : class
    {
        private readonly BuilderNode? _node;

        BuilderNodeType ITransactWriteItemBuilder.NodeType =>  BuilderNodeType.TransactDeleteItemNode;

        BuilderNode ITransactWriteItemBuilder.GetNode() => _node ?? throw new DdbException("Transact write can't contain an empty operation.");

        Type ITransactWriteItemBuilder.GetEntityType() => typeof(TEntity);

        private TransactDeleteItemBuilder(BuilderNode? node)
        {
            _node = node;
        }

        public TransactDeleteItemBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new TransactDeleteItemBuilder<TEntity>(new PartitionKeyNode<TPk>(pk, _node));

        public TransactDeleteItemBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk)=>
            new TransactDeleteItemBuilder<TEntity>(new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public TransactDeleteItemBuilder<TEntity> WithCondition(FilterBase condition) =>
            new TransactDeleteItemBuilder<TEntity>(new ConditionNode(condition, _node));

        public TransactDeleteItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup) =>
            new TransactDeleteItemBuilder<TEntity>(new ConditionNode(conditionSetup(Condition.ForEntity<TEntity>()), _node));

        public TransactDeleteItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure) =>
            new TransactDeleteItemBuilder<TEntity>(new ReturnValuesOnConditionCheckFailureNode(returnValuesOnConditionCheckFailure, _node));
    }
}