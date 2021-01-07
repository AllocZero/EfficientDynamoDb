using EfficientDynamoDb.Context.FluentCondition.Operators.Common;
using EfficientDynamoDb.Context.FluentCondition.Operators.Size;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.FluentCondition.Core
{
    public class AttributeFilter<TEntity>
    {
        private readonly string _propertyName;

        internal AttributeFilter(string propertyName)
        {
            _propertyName = propertyName;
        }

        public FilterBase LessThan<T>(T value) => new FilterLessThan<TEntity, T>(_propertyName, value);
        
        public FilterBase LessThanOrEqualsTo<T>(T value) => new FilterLessThanOrEqualsTo<TEntity, T>(_propertyName, value);
        
        public FilterBase EqualsTo<T>(T value) => new FilterEqualsTo<TEntity, T>(_propertyName, value);
        
        public FilterBase NotEqualsTo<T>(T value) => new FilterNotEqualsTo<TEntity, T>(_propertyName, value);
        
        public FilterBase GreaterThan<T>(T value) => new FilterGreaterThan<TEntity, T>(_propertyName, value);
        
        public FilterBase GreaterThanOrEqualsTo<T>(T value) => new FilterGreaterThanOrEqualsTo<TEntity, T>(_propertyName, value);
        
        public FilterBase Between<T>(T min, T max) => new FilterBetween<TEntity, T>(_propertyName, min, max);
        
        public FilterBase BeginsWith(string prefix) => new FilterBeginsWith<TEntity>(_propertyName, prefix);
        
        public FilterBase In<T>(params T[] values) => new FilterIn<TEntity, T>(_propertyName, values);
        
        public FilterBase Exists() => new FilterAttributeExists<TEntity>(_propertyName);
        
        public FilterBase NotExists() => new FilterAttributeNotExists<TEntity>(_propertyName);
        
        public FilterBase Contains<T>(T value) => new FilterContains<TEntity, T>(_propertyName, value);
        
        public FilterBase OfType(AttributeType type) => new FilterAttributeType<TEntity>(_propertyName, type);
        
        public FilterSizeOperation<TEntity> Size() => new FilterSizeOperation<TEntity>(_propertyName);
    }
}