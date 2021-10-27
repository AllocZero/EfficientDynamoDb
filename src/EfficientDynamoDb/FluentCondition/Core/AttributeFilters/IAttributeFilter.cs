using System;
using System.Linq.Expressions;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.FluentCondition.Core.AttributeFilters
{
    public interface IAttributeFilter<TEntity>
    {
        public FilterBase LessThan<T>(T value);
        public FilterBase LessThan<TProperty>(Expression<Func<TEntity, TProperty>> property);
        public FilterBase LessThanSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property);
        
        public FilterBase LessThanOrEqualTo<T>(T value);
        public FilterBase LessThanOrEqualTo<TProperty>(Expression<Func<TEntity, TProperty>> property);
        public FilterBase LessThanOrEqualToSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property);
        
        public FilterBase EqualTo<T>(T value);
        public FilterBase EqualTo<TProperty>(Expression<Func<TEntity, TProperty>> property);
        public FilterBase EqualToSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property);
        
        public FilterBase NotEqualTo<T>(T value);
        public FilterBase NotEqualTo<TProperty>(Expression<Func<TEntity, TProperty>> property);
        public FilterBase NotEqualToSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property);
        
        public FilterBase GreaterThan<T>(T value);
        public FilterBase GreaterThan<TProperty>(Expression<Func<TEntity, TProperty>> property);
        public FilterBase GreaterThanSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property);
        
        public FilterBase GreaterThanOrEqualTo<T>(T value);
        public FilterBase GreaterThanOrEqualTo<TProperty>(Expression<Func<TEntity, TProperty>> property);
        public FilterBase GreaterThanOrEqualToSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property);
        
        public FilterBase Between<T>(T min, T max);
        public FilterBase Between<TProperty>(Expression<Func<TEntity, TProperty>> minProperty, TProperty max);
        public FilterBase BetweenSizeOfAndValue<TProperty>(Expression<Func<TEntity, TProperty>> minProperty, TProperty max);
        public FilterBase Between<TProperty>(TProperty min, Expression<Func<TEntity, TProperty>> maxProperty);
        public FilterBase BetweenValueAndSizeOf<TProperty>(TProperty min, Expression<Func<TEntity, TProperty>> maxProperty);
        public FilterBase Between<TProperty>(Expression<Func<TEntity, TProperty>> minProperty, Expression<Func<TEntity, TProperty>> maxProperty);
        public FilterBase BetweenSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> minProperty, Expression<Func<TEntity, TProperty>> maxProperty);
        
        public FilterBase BeginsWith(string prefix);
        public FilterBase BeginsWith<TProperty>(Expression<Func<TEntity, TProperty>> property);
        public FilterBase BeginsWithSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property);
        
        public FilterBase In<T>(params T[] values);
        public FilterBase In<TProperty>(params Expression[] properties);
        public FilterBase InSizeOf<TProperty>(params Expression[] properties);
        
        public FilterBase Exists();
        public FilterBase NotExists();
        
        public FilterBase Contains<T>(T value);
        public FilterBase Contains<TProperty>(Expression<Func<TEntity, TProperty>> property);
        public FilterBase ContainsSizeOf<TProperty>(Expression<Func<TEntity, TProperty>> property);
        
        public FilterBase OfType(AttributeType type);
    }
}