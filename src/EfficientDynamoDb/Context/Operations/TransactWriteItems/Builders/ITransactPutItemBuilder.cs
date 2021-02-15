using System;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
{
    public interface ITransactPutItemBuilder<TEntity> : ITransactWriteItemBuilder where TEntity : class
    {
        ITransactPutItemBuilder<TEntity> WithCondition(FilterBase condition);

        ITransactPutItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);
        
        ITransactPutItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure);        
    }
}