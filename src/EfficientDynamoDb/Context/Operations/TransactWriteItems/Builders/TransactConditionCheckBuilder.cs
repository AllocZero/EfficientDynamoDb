using System;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Context.Operations.Query;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    internal sealed class TransactConditionCheckBuilder<TEntity> : TransactWriteItemBuilder<TEntity>, ITransactConditionCheckBuilder<TEntity> where TEntity : class
    {
        protected override BuilderNodeType NodeType => BuilderNodeType.TransactConditionCheckNode;
        
        public TransactConditionCheckBuilder(TransactWriteItemsRequestBuilder requestBuilder) : base(requestBuilder)
        {
        }

        public TransactConditionCheckBuilder(TransactWriteItemsRequestBuilder requestBuilder, BuilderNode? node) : base(requestBuilder, node)
        {
        }

        public ITransactConditionCheckBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk) =>
            new TransactConditionCheckBuilder<TEntity>(RequestBuilder, new PartitionKeyNode<TPk>(pk, Node));

        public ITransactConditionCheckBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk)=>
            new TransactConditionCheckBuilder<TEntity>(RequestBuilder, new PartitionAndSortKeyNode<TPk, TSk>(pk, sk, Node));

        public ITransactConditionCheckBuilder<TEntity> WithCondition(FilterBase condition) =>
            new TransactConditionCheckBuilder<TEntity>(RequestBuilder, new ConditionNode(condition, Node));

        public ITransactConditionCheckBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup) =>
            new TransactConditionCheckBuilder<TEntity>(RequestBuilder, new ConditionNode(conditionSetup(Filter.ForEntity<TEntity>()), Node));

        public ITransactConditionCheckBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure) =>
            new TransactConditionCheckBuilder<TEntity>(RequestBuilder, new ReturnValuesOnConditionCheckFailureNode(returnValuesOnConditionCheckFailure, Node));
    }
}