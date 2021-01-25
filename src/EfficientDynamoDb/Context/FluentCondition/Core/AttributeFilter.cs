using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Operators.Common;
using EfficientDynamoDb.Context.FluentCondition.Operators.Size;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.FluentCondition.Core
{
    // TODO: Make two AttributeFilter implementations - default implementation is generic that forces property type as a method parameter and second one is current implementation that allows any parameter type 
    public class AttributeFilter<TEntity>
    {
        private readonly Expression _expression;

        internal AttributeFilter(Expression expression)
        {
            _expression = expression;
        }

        public FilterBase LessThan<T>(T value) => new FilterLessThan<TEntity, T>(_expression, value);
        public FilterBase LessThan(Expression property) => new FilterLessThan<TEntity>(_expression, property);
        
        public FilterBase LessThanOrEqualsTo<T>(T value) => new FilterLessThanOrEqualsTo<TEntity, T>(_expression, value);
        public FilterBase LessThanOrEqualsTo(Expression property) => new FilterLessThanOrEqualsTo<TEntity>(_expression, property);
        
        
        public FilterBase EqualsTo<T>(T value) => new FilterEqualsTo<TEntity, T>(_expression, value);
        public FilterBase EqualsTo(Expression property) => new FilterEqualsTo<TEntity>(_expression, property);
        
        public FilterBase NotEqualsTo<T>(T value) => new FilterNotEqualsTo<TEntity, T>(_expression, value);
        public FilterBase NotEqualsTo(Expression property) => new FilterNotEqualsTo<TEntity>(_expression, property);
        
        public FilterBase GreaterThan<T>(T value) => new FilterGreaterThan<TEntity, T>(_expression, value);
        public FilterBase GreaterThan(Expression property) => new FilterGreaterThan<TEntity>(_expression, property);
        
        public FilterBase GreaterThanOrEqualsTo<T>(T value) => new FilterGreaterThanOrEqualsTo<TEntity, T>(_expression, value);
        public FilterBase GreaterThanOrEqualsTo(Expression property) => new FilterGreaterThanOrEqualsTo<TEntity>(_expression, property);
        
        public FilterBase Between<T>(T min, T max) => new FilterBetweenValues<TEntity, T>(_expression, min, max);
        public FilterBase Between<T>(Expression minProperty, T max) => new FilterBetweenAttributeAndValue<TEntity, T>(_expression, minProperty, max);
        public FilterBase Between<T>(T min, Expression maxProperty) => new FilterBetweenValueAndAttribute<TEntity, T>(_expression, min, maxProperty);
        public FilterBase Between(Expression minProperty, Expression maxProperty) => new FilterBetweenAttributes<TEntity>(_expression, minProperty, maxProperty);
        
        public FilterBase BeginsWith(string prefix) => new FilterBeginsWithValue<TEntity>(_expression, prefix);
        public FilterBase BeginsWith(Expression property) => new FilterBeginsWithAttribute<TEntity>(_expression, property);
        
        public FilterBase In<T>(params T[] values) => new FilterIn<TEntity, T>(_expression, values);
        public FilterBase In(params Expression[] properties) => new FilterIn<TEntity>(_expression, properties);
        
        public FilterBase Exists() => new FilterAttributeExists<TEntity>(_expression);
        
        public FilterBase NotExists() => new FilterAttributeNotExists<TEntity>(_expression);
        
        public FilterBase Contains<T>(T value) => new FilterContains<TEntity, T>(_expression, value);
        public FilterBase Contains(Expression property) => new FilterContains<TEntity>(_expression, property);
        
        public FilterBase OfType(AttributeType type) => new FilterAttributeType<TEntity>(_expression, type);
        
        public FilterSizeOperation<TEntity> Size() => new FilterSizeOperation<TEntity>(_expression);
    }
}