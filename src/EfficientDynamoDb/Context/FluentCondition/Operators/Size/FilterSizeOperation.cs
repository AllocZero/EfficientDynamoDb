using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Size
{
    public class FilterSizeOperation<TEntity>
    {
        private readonly string _propertyName;

        internal FilterSizeOperation(string propertyName) => _propertyName = propertyName;
        
        public FilterBase LessThan<T>(T value) => new FilterSizeLessThan<TEntity, T>(_propertyName, value);
        
        public FilterBase LessThanOrEqualsTo<T>(T value) => new FilterSizeLessThanOrEqualsTo<TEntity, T>(_propertyName, value);
        
        public FilterBase EqualsTo<T>(T value) => new FilterSizeEqualsTo<TEntity, T>(_propertyName, value);
        
        public FilterBase NotEqualsTo<T>(T value) => new FilterSizeNotEqualsTo<TEntity, T>(_propertyName, value);
        
        public FilterBase GreaterThan<T>(T value) => new FilterSizeGreaterThan<TEntity, T>(_propertyName, value);
        
        public FilterBase GreaterThanOrEqualsTo<T>(T value) => new FilterSizeGreaterThanOrEqualsTo<TEntity, T>(_propertyName, value);
        
        public FilterBase Between<T>(T min, T max) => new FilterSizeBetween<TEntity, T>(_propertyName, min, max);
    }
}