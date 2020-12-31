using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Size
{
    public class FilterSizeOperation<TEntity>
    {
        private readonly string _propertyName;

        internal FilterSizeOperation(string propertyName) => _propertyName = propertyName;
        
        public IFilter LessThan<T>(T value) => new FilterSizeLessThan<TEntity, T>(_propertyName, value);
        
        public IFilter LessThanOrEqualsTo<T>(T value) => new FilterSizeLessThanOrEqualsTo<TEntity, T>(_propertyName, value);
        
        public IFilter EqualsTo<T>(T value) => new FilterSizeEqualsTo<TEntity, T>(_propertyName, value);
        
        public IFilter NotEqualsTo<T>(T value) => new FilterSizeNotEqualsTo<TEntity, T>(_propertyName, value);
        
        public IFilter GreaterThan<T>(T value) => new FilterSizeGreaterThan<TEntity, T>(_propertyName, value);
        
        public IFilter GreaterThanOrEqualsTo<T>(T value) => new FilterSizeGreaterThanOrEqualsTo<TEntity, T>(_propertyName, value);
        
        public IFilter Between<T>(T min, T max) => new FilterSizeBetween<TEntity, T>(_propertyName, min, max);
    }
}