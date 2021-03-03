using System;
using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    internal sealed class TransactUpdateItemBuilder<TEntity> : TransactWriteItemBuilder<TEntity>, ITransactUpdateItemBuilder<TEntity> where TEntity : class
    {
        protected override BuilderNodeType NodeType => BuilderNodeType.TransactUpdateItemNode;

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
            new TransactUpdateItemBuilder<TEntity>(new ConditionNode(conditionSetup(Filter.ForEntity<TEntity>()), Node));

        public ITransactUpdateItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure) =>
            new TransactUpdateItemBuilder<TEntity>(new ReturnValuesOnConditionCheckFailureNode(returnValuesOnConditionCheckFailure, Node));

        public IAttributeUpdate<ITransactUpdateItemBuilder<TEntity>, TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> expression) =>
            new AttributeUpdate<ITransactUpdateItemBuilder<TEntity>, TEntity, TProperty>(this, expression);

        ITransactUpdateItemBuilder<TEntity> IUpdateItemBuilder<ITransactUpdateItemBuilder<TEntity>>.Create(UpdateBase update, BuilderNodeType nodeType) =>
            new TransactUpdateItemBuilder<TEntity>(new UpdateAttributeNode(update, nodeType, Node));
    }
}