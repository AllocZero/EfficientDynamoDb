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
        
        public FilterBase LessThanOrEqualsTo<T>(T value) => new FilterLessThanOrEqualsTo<TEntity, T>(_expression, value);
        
        public FilterBase EqualsTo<T>(T value) => new FilterEqualsTo<TEntity, T>(_expression, value);
        
        public FilterBase NotEqualsTo<T>(T value) => new FilterNotEqualsTo<TEntity, T>(_expression, value);
        
        public FilterBase GreaterThan<T>(T value) => new FilterGreaterThan<TEntity, T>(_expression, value);
        
        public FilterBase GreaterThanOrEqualsTo<T>(T value) => new FilterGreaterThanOrEqualsTo<TEntity, T>(_expression, value);
        
        public FilterBase Between<T>(T min, T max) => new FilterBetween<TEntity, T>(_expression, min, max);
        
        public FilterBase BeginsWith(string prefix) => new FilterBeginsWith<TEntity>(_expression, prefix);
        
        public FilterBase In<T>(params T[] values) => new FilterIn<TEntity, T>(_expression, values);
        
        public FilterBase Exists() => new FilterAttributeExists<TEntity>(_expression);
        
        public FilterBase NotExists() => new FilterAttributeNotExists<TEntity>(_expression);
        
        public FilterBase Contains<T>(T value) => new FilterContains<TEntity, T>(_expression, value);
        
        public FilterBase OfType(AttributeType type) => new FilterAttributeType<TEntity>(_expression, type);
        
        public FilterSizeOperation<TEntity> Size() => new FilterSizeOperation<TEntity>(_expression);
    }
}