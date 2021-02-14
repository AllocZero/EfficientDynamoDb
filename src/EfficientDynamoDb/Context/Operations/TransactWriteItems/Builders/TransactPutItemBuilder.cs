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
        
        public TransactPutItemBuilder(TransactWriteItemsRequestBuilder requestBuilder) : base(requestBuilder)
        {
        }

        public TransactPutItemBuilder(TransactWriteItemsRequestBuilder requestBuilder, BuilderNode? node) : base(requestBuilder, node)
        {
        }

        public ITransactPutItemBuilder<TEntity> WithCondition(FilterBase condition) =>
            new TransactPutItemBuilder<TEntity>(RequestBuilder, new ConditionNode(condition, Node));

        public ITransactPutItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup) =>
            new TransactPutItemBuilder<TEntity>(RequestBuilder, new ConditionNode(conditionSetup(Filter.ForEntity<TEntity>()), Node));

        public ITransactPutItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure) =>
            new TransactPutItemBuilder<TEntity>(RequestBuilder, new ReturnValuesOnConditionCheckFailureNode(returnValuesOnConditionCheckFailure, Node));
    }
}