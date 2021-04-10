using System;
using System.Linq.Expressions;
using EfficientDynamoDb.FluentCondition;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Query;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    internal sealed class TransactUpdateItemBuilder<TEntity> : TransactWriteItemBuilder<TEntity>, ITransactUpdateItemBuilder<TEntity> where TEntity : class
    {
        protected override BuilderNodeType NodeType => BuilderNodeType.TransactUpdateItemNode;

        BuilderNode? ITableBuilder<ITransactUpdateItemBuilder<TEntity>>.Node => Node;

        ITransactUpdateItemBuilder<TEntity> ITableBuilder<ITransactUpdateItemBuilder<TEntity>>.Create(BuilderNode newNode)
            => new TransactUpdateItemBuilder<TEntity>(newNode);

        public TransactUpdateItemBuilder()
        {
        }

        public TransactUpdateItemBuilder(BuilderNode? node) : base(node)
        {
        }

        public ITransactUpdateItemBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new TransactUpdateItemBuilder<TEntity>(new PartitionKeyNode<TPk>(pk, Node));

        public ITransactUpdateItemBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk)=>
            new TransactUpdateItemBuilder<TEntity>(new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, Node));

        public ITransactUpdateItemBuilder<TEntity> WithCondition(FilterBase condition) =>
            new TransactUpdateItemBuilder<TEntity>(new ConditionNode(condition, Node));

        public ITransactUpdateItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup) =>
            new TransactUpdateItemBuilder<TEntity>(new ConditionNode(conditionSetup(Condition.ForEntity<TEntity>()), Node));

        public ITransactUpdateItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure) =>
            new TransactUpdateItemBuilder<TEntity>(new ReturnValuesOnConditionCheckFailureNode(returnValuesOnConditionCheckFailure, Node));

        public IAttributeUpdate<ITransactUpdateItemBuilder<TEntity>, TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> expression) =>
            new AttributeUpdate<ITransactUpdateItemBuilder<TEntity>, TEntity, TProperty>(this, expression);

        ITransactUpdateItemBuilder<TEntity> IUpdateItemBuilder<ITransactUpdateItemBuilder<TEntity>>.Create(UpdateBase update, BuilderNodeType nodeType) =>
            new TransactUpdateItemBuilder<TEntity>(new UpdateAttributeNode(update, nodeType, Node));
    }
}