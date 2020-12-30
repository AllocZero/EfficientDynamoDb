using EfficientDynamoDb.Context.FluentCondition.Operators.Common;
using EfficientDynamoDb.Context.FluentCondition.Operators.Size;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.FluentCondition.Core
{
    public class AttributeFilter
    {
        private readonly string _propertyName;

        internal AttributeFilter(string propertyName)
        {
            _propertyName = propertyName;
        }

        public FilterLessThan<T> LessThan<T>(T value) => new FilterLessThan<T>(_propertyName, value);
        
        public FilterLessThanOrEqualsTo<T> LessThanOrEqualsTo<T>(T value) => new FilterLessThanOrEqualsTo<T>(_propertyName, value);
        
        public FilterEqualsTo<T> EqualsTo<T>(T value) => new FilterEqualsTo<T>(_propertyName, value);
        
        public FilterNotEqualsTo<T> NotEqualsTo<T>(T value) => new FilterNotEqualsTo<T>(_propertyName, value);
        
        public FilterGreaterThan<T> GreaterThan<T>(T value) => new FilterGreaterThan<T>(_propertyName, value);
        
        public FilterGreaterThanOrEqualsTo<T> GreaterThanOrEqualsTo<T>(T value) => new FilterGreaterThanOrEqualsTo<T>(_propertyName, value);
        
        public FilterBetween<T> Between<T>(T min, T max) => new FilterBetween<T>(_propertyName, min, max);
        
        public FilterBeginsWith BeginsWith<T>(string prefix) => new FilterBeginsWith(_propertyName, prefix);
        
        public FilterIn<T> In<T>(params T[] values) => new FilterIn<T>(_propertyName, values);
        
        public FilterAttributeExists Exists() => new FilterAttributeExists(_propertyName);
        
        public FilterAttributeNotExists NotExists() => new FilterAttributeNotExists(_propertyName);
        
        public FilterContains<T> Contains<T>(T value) => new FilterContains<T>(_propertyName, value);
        
        public FilterAttributeType OfType(AttributeType type) => new FilterAttributeType(_propertyName, type);
        
        public FilterSizeOperation Size() => new FilterSizeOperation(_propertyName);
    }
}