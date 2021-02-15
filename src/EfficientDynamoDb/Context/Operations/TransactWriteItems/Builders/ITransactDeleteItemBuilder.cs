using System;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    public interface ITransactDeleteItemBuilder<TEntity> : ITransactWriteItemBuilder where TEntity : class
    {
        ITransactDeleteItemBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);

        ITransactDeleteItemBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        ITransactDeleteItemBuilder<TEntity> WithCondition(FilterBase condition);

        ITransactDeleteItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);
        
        ITransactDeleteItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure);
    }
}