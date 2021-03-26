using System;
using System.Linq.Expressions;
using EfficientDynamoDb.FluentCondition;
using EfficientDynamoDb.FluentCondition.Core;
using EfficientDynamoDb.Operations.Shared;

namespace EfficientDynamoDb.Operations.TransactWriteItems.Builders
{
    public interface ITransactUpdateItemBuilder<TEntity> : ITransactWriteItemBuilder, IUpdateItemBuilder<ITransactUpdateItemBuilder<TEntity>> where TEntity : class
    {
        ITransactUpdateItemBuilder<TEntity> WithPrimaryKey<TPk>(TPk pk);

        ITransactUpdateItemBuilder<TEntity> WithPrimaryKey<TPk, TSk>(TPk pk, TSk sk);
        
        ITransactUpdateItemBuilder<TEntity> WithCondition(FilterBase condition);

        ITransactUpdateItemBuilder<TEntity> WithCondition(Func<EntityFilter<TEntity>, FilterBase> conditionSetup);
        
        ITransactUpdateItemBuilder<TEntity> WithReturnValuesOnConditionCheckFailure(ReturnValuesOnConditionCheckFailure returnValuesOnConditionCheckFailure);
        
        IAttributeUpdate<ITransactUpdateItemBuilder<TEntity>, TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> expression);
    }
}