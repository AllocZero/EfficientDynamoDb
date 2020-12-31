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

        public IFilter LessThan<T>(T value) => new FilterLessThan<TEntity, T>(_propertyName, value);
        
        public IFilter LessThanOrEqualsTo<T>(T value) => new FilterLessThanOrEqualsTo<TEntity, T>(_propertyName, value);
        
        public IFilter EqualsTo<T>(T value) => new FilterEqualsTo<TEntity, T>(_propertyName, value);
        
        public IFilter NotEqualsTo<T>(T value) => new FilterNotEqualsTo<TEntity, T>(_propertyName, value);
        
        public IFilter GreaterThan<T>(T value) => new FilterGreaterThan<TEntity, T>(_propertyName, value);
        
        public IFilter GreaterThanOrEqualsTo<T>(T value) => new FilterGreaterThanOrEqualsTo<TEntity, T>(_propertyName, value);
        
        public IFilter Between<T>(T min, T max) => new FilterBetween<TEntity, T>(_propertyName, min, max);
        
        public IFilter BeginsWith<T>(string prefix) => new FilterBeginsWith<TEntity>(_propertyName, prefix);
        
        public IFilter In<T>(params T[] values) => new FilterIn<TEntity, T>(_propertyName, values);
        
        public IFilter Exists() => new FilterAttributeExists<TEntity>(_propertyName);
        
        public IFilter NotExists() => new FilterAttributeNotExists<TEntity>(_propertyName);
        
        public IFilter Contains<T>(T value) => new FilterContains<TEntity, T>(_propertyName, value);
        
        public IFilter OfType(AttributeType type) => new FilterAttributeType<TEntity>(_propertyName, type);
        
        public FilterSizeOperation<TEntity> Size() => new FilterSizeOperation<TEntity>(_propertyName);
    }
}