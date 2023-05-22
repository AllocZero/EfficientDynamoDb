using System;
using System.Linq.Expressions;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.FluentCondition.Core.AttributeFilters
{
    public interface IAttributeFilter<TEntity, TProperty>
    {
        public FilterBase LessThan(TProperty value);
        public FilterBase LessThan<T>(Expression<Func<TEntity, T>> property);
        public FilterBase LessThanSizeOf<T>(Expression<Func<TEntity, T>> property);
        
        public FilterBase LessThanOrEqualTo(TProperty value);
        public FilterBase LessThanOrEqualTo<T>(Expression<Func<TEntity, T>> property);
        public FilterBase LessThanOrEqualToSizeOf<T>(Expression<Func<TEntity, T>> property);
        
        public FilterBase EqualTo(TProperty value);
        public FilterBase EqualTo<T>(Expression<Func<TEntity, T>> property);
        public FilterBase EqualToSizeOf<T>(Expression<Func<TEntity, T>> property);
        
        public FilterBase NotEqualTo(TProperty value);
        public FilterBase NotEqualTo<T>(Expression<Func<TEntity, T>> property);
        public FilterBase NotEqualToSizeOf<T>(Expression<Func<TEntity, T>> property);
        
        public FilterBase GreaterThan(TProperty value);
        public FilterBase GreaterThan<T>(Expression<Func<TEntity, T>> property);
        public FilterBase GreaterThanSizeOf<T>(Expression<Func<TEntity, T>> property);
        
        public FilterBase GreaterThanOrEqualTo(TProperty value);
        public FilterBase GreaterThanOrEqualTo<T>(Expression<Func<TEntity, T>> property);
        public FilterBase GreaterThanOrEqualToSizeOf<T>(Expression<Func<TEntity, T>> property);
        
        public FilterBase Between(TProperty min, TProperty max);
        public FilterBase Between<T>(Expression<Func<TEntity, T>> minProperty, TProperty max);
        public FilterBase BetweenSizeOfAndValue<T>(Expression<Func<TEntity, T>> minProperty, TProperty max);
        public FilterBase Between<T>(TProperty min, Expression<Func<TEntity, T>> maxProperty);
        public FilterBase BetweenValueAndSizeOf<T>(TProperty min, Expression<Func<TEntity, T>> maxProperty);
        public FilterBase Between<T>(Expression<Func<TEntity, T>> minProperty, Expression<Func<TEntity, T>> maxProperty);
        public FilterBase BetweenSizeOf<T>(Expression<Func<TEntity, T>> minProperty, Expression<Func<TEntity, T>> maxProperty);
        
        public FilterBase BeginsWith(string prefix);
        public FilterBase BeginsWith<T>(Expression<Func<TEntity, T>> property);
        public FilterBase BeginsWithSizeOf<T>(Expression<Func<TEntity, T>> property);
        
        public FilterBase In(params TProperty[] values);
        public FilterBase In(params Expression[] properties);
        public FilterBase InSizeOf(params Expression[] properties);
        
        public FilterBase Exists();
        public FilterBase NotExists();
        
        public FilterBase Contains<T>(T value);
        public FilterBase Contains<T>(Expression<Func<TEntity, T>> property);
        public FilterBase ContainsSizeOf<T>(Expression<Func<TEntity, T>> property);
        
        public FilterBase OfType(AttributeType type);
    }
}