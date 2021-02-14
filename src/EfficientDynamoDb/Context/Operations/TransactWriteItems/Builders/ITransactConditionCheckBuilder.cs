using System;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    public interface ITransactConditionCheckBuilder<TEntity> : ITransactWriteItemBuilder where TEntity : class
    {
        ITransactConditionCheckBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);

        ITransactConditionCheckBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        ITransactConditionCheckBuilder<TEntity> WithCondition(FilterBase condition);

        ITransactConditionCheckBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);

        ITransactConditionCheckBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure);
    }
}