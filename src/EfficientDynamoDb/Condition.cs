using System;
using System.Linq.Expressions;
using EfficientDynamoDb.FluentCondition.Core.AttributeFilters;

namespace EfficientDynamoDb
{
    public static class Condition
    {
        public static EntityFilter<TEntity> ForEntity<TEntity>() => EntityFilter<TEntity>.Instance;
    }

    public class EntityFilter<TEntity>
    {
        internal static readonly EntityFilter<TEntity> Instance = new EntityFilter<TEntity>();
        
        public IAttributeFilter<TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> property) => Condition<TEntity>.On(property);
        
        public ISizeOfAttributeFilter<TEntity, TProperty> OnSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property) => Condition<TEntity>.OnSizeOf(property);
    }

    public static class Condition<TEntity>
    {
        public static IAttributeFilter<TEntity, TProperty> On<TProperty>(Expression<Func<TEntity, TProperty>> property) => new AttributeFilter<TEntity, TProperty>(property, false);

        public static ISizeOfAttributeFilter<TEntity, TProperty> OnSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property) =>
            new AttributeFilter<TEntity, TProperty>(property, true);
    }
}