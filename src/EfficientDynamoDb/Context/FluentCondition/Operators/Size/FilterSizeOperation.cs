using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Size
{
    public class FilterSizeOperation<TEntity>
    {
        private readonly Expression _expression;

        internal FilterSizeOperation(Expression expressionVisitor) => _expression = expressionVisitor;
        
        public FilterBase LessThan<T>(T value) => new FilterSizeLessThan<TEntity, T>(_expression, value);
        
        public FilterBase LessThanOrEqualsTo<T>(T value) => new FilterSizeLessThanOrEqualsTo<TEntity, T>(_expression, value);
        
        public FilterBase EqualsTo<T>(T value) => new FilterSizeEqualsTo<TEntity, T>(_expression, value);
        
        public FilterBase NotEqualsTo<T>(T value) => new FilterSizeNotEqualsTo<TEntity, T>(_expression, value);
        
        public FilterBase GreaterThan<T>(T value) => new FilterSizeGreaterThan<TEntity, T>(_expression, value);
        
        public FilterBase GreaterThanOrEqualsTo<T>(T value) => new FilterSizeGreaterThanOrEqualsTo<TEntity, T>(_expression, value);
        
        public FilterBase Between<T>(T min, T max) => new FilterSizeBetween<TEntity, T>(_expression, min, max);
    }
}