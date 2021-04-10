using System;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    public interface ITransactDeleteItemBuilder<TEntity> : ITransactWriteItemBuilder, ITableBuilder<ITransactDeleteItemBuilder<TEntity>> where TEntity : class
    {
        ITransactDeleteItemBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);

        ITransactDeleteItemBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        ITransactDeleteItemBuilder<TEntity> WithCondition(FilterBase condition);

        ITransactDeleteItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);
        
        ITransactDeleteItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure);
    }
}