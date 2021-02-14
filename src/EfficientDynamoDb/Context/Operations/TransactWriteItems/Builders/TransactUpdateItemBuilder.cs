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
        
        public TransactUpdateItemBuilder(TransactWriteItemsRequestBuilder requestBuilder) : base(requestBuilder)
        {
        }

        public TransactUpdateItemBuilder(TransactWriteItemsRequestBuilder requestBuilder, BuilderNode? node) : base(requestBuilder, node)
        {
        }
        
        public ITransactUpdateItemBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new TransactUpdateItemBuilder<TEntity>(RequestBuilder, new PartitionKeyNode<TPk>(pk, Node));

        public ITransactUpdateItemBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk)=>
            new TransactUpdateItemBuilder<TEntity>(RequestBuilder, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, Node));

        public ITransactUpdateItemBuilder<TEntity> WithCondition(FilterBase condition) =>
            new TransactUpdateItemBuilder<TEntity>(RequestBuilder, new ConditionNode(condition, Node));

        public ITransactUpdateItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup) =>
            new TransactUpdateItemBuilder<TEntity>(RequestBuilder, new ConditionNode(conditionSetup(Filter.ForEntity<TEntity>()), Node));

        public ITransactUpdateItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure) =>
            new TransactUpdateItemBuilder<TEntity>(RequestBuilder, new ReturnValuesOnConditionCheckFailureNode(returnValuesOnConditionCheckFailure, Node));

        public IAttributeUpdate<TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> expression)
        {
            // TODO:
            throw new NotImplementedException();
        }
    }
}