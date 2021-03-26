using System;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    public interface ITransactPutItemBuilder<TEntity> : ITransactWriteItemBuilder where TEntity : class
    {
        ITransactPutItemBuilder<TEntity> WithCondition(FilterBase condition);

        ITransactPutItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);
        
        ITransactPutItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure);        
    }
}