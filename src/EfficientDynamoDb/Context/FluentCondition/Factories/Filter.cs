using System;
using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Factories
{
    public static class Filter
    {
        public static EntityFilter<TEntity> ForEntity<TEntity>() => EntityFilter<TEntity>.Instance;
    }

    public class EntityFilter<TEntity>
    {
        internal static readonly EntityFilter<TEntity> Instance = new EntityFilter<TEntity>();
        
        public AttributeFilter<TEntity> On<TProperty>(Expression<Func<TEntity, TProperty>> property) => Filter<TEntity>.On(property);
    }

    public static class Filter<TEntity>
    {
        public static AttributeFilter<TEntity> On<TProperty>(Expression<Func<TEntity, TProperty>> property)
        {
            return new AttributeFilter<TEntity>(property);
        }
    }
}