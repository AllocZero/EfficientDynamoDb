using System;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    public interface ITransactConditionCheckBuilder<TEntity> : ITransactWriteItemBuilder, ITableBuilder<ITransactConditionCheckBuilder<TEntity>> where TEntity : class
    {
        ITransactConditionCheckBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);

        ITransactConditionCheckBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        ITransactConditionCheckBuilder<TEntity> WithCondition(FilterBase condition);

        ITransactConditionCheckBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);

        ITransactConditionCheckBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure);
    }
}