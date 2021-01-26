using System;
using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Core.AttributeFilters;

namespace EfficientDynamoDb.Context.FluentCondition.Factories
{
    public static class Filter
    {
        public static EntityFilter<TEntity> ForEntity<TEntity>() => EntityFilter<TEntity>.Instance;
    }

    public class EntityFilter<TEntity>
    {
        internal static readonly EntityFilter<TEntity> Instance = new EntityFilter<TEntity>();
        
        public IAttributeFilter On<TProperty>(Expression<Func<TEntity, TProperty>> property) => Filter<TEntity>.On(property);
        
        public ISizeOfAttributeFilter OnSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property) => Filter<TEntity>.OnSizeOf(property);
    }

    public static class Filter<TEntity>
    {
        public static IAttributeFilter On<TProperty>(Expression<Func<TEntity, TProperty>> property) => new AttributeFilter<TEntity>(property, false);

        public static ISizeOfAttributeFilter OnSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property) =>
            new AttributeFilter<TEntity>(property, true);
    }
}