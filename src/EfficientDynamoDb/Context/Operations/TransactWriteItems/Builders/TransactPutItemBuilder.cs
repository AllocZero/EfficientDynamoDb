using System;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    public readonly struct TransactPutItemBuilder<TEntity> : ITransactWriteItemBuilder where TEntity : class
    {
        private readonly BuilderNode? _node;

        BuilderNodeType ITransactWriteItemBuilder.NodeType =>  BuilderNodeType.TransactPutItemNode;

        BuilderNode ITransactWriteItemBuilder.GetNode() => _node ?? throw new DdbException("Transact write can't contain an empty operation.");

        Type ITransactWriteItemBuilder.GetEntityType() => typeof(TEntity);
        
        private TransactPutItemBuilder(BuilderNode? node)
        {
            _node = node;
        }

        public TransactPutItemBuilder<TEntity> WithCondition(FilterBase condition) =>
            new TransactPutItemBuilder<TEntity>(new ConditionNode(condition, _node));

        public TransactPutItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup) =>
            new TransactPutItemBuilder<TEntity>(new ConditionNode(conditionSetup(Condition.ForEntity<TEntity>()), _node));

        public TransactPutItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure) =>
            new TransactPutItemBuilder<TEntity>(new ReturnValuesOnConditionCheckFailureNode(returnValuesOnConditionCheckFailure, _node));
    }
}