using System;
using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;

namespace EfficientDynamoDb.Context.Operations.TransactWriteItems.Builders
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