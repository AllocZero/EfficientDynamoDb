using System;
using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    public readonly struct TransactUpdateItemBuilder<TEntity> : ITransactWriteItemBuilder, IUpdateItemBuilder<TransactUpdateItemBuilder<TEntity>> where TEntity : class
    {
        private readonly BuilderNode? _node;

        BuilderNodeType ITransactWriteItemBuilder.NodeType =>  BuilderNodeType.TransactUpdateItemNode;

        BuilderNode ITransactWriteItemBuilder.GetNode() => _node ?? throw new DdbException("Transact write can't contain an empty operation.");

        Type ITransactWriteItemBuilder.GetEntityType() => typeof(TEntity);

        private TransactUpdateItemBuilder(BuilderNode? node)
        {
            _node = node;
        }

        public TransactUpdateItemBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new TransactUpdateItemBuilder<TEntity>(new PartitionKeyNode<TPk>(pk, _node));

        public TransactUpdateItemBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk)=>
            new TransactUpdateItemBuilder<TEntity>(new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, _node));

        public TransactUpdateItemBuilder<TEntity> WithCondition(FilterBase condition) =>
            new TransactUpdateItemBuilder<TEntity>(new ConditionNode(condition, _node));

        public TransactUpdateItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup) =>
            new TransactUpdateItemBuilder<TEntity>(new ConditionNode(conditionSetup(Condition.ForEntity<TEntity>()), _node));

        public TransactUpdateItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure) =>
            new TransactUpdateItemBuilder<TEntity>(new ReturnValuesOnConditionCheckFailureNode(returnValuesOnConditionCheckFailure, _node));

        public AttributeUpdate<TransactUpdateItemBuilder<TEntity>, TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> expression) =>
            new AttributeUpdate<TransactUpdateItemBuilder<TEntity>, TEntity, TProperty>(this, expression);

        TransactUpdateItemBuilder<TEntity> IUpdateItemBuilder<TransactUpdateItemBuilder<TEntity>>.Create(UpdateBase update, BuilderNodeType nodeType) =>
            new TransactUpdateItemBuilder<TEntity>(new UpdateAttributeNode(update, nodeType, _node));
    }
}