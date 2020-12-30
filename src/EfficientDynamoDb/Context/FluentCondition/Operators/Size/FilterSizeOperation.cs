namespace EfficientDynamoDb.Context.FluentCondition.Operators.Size
{
    public class FilterSizeOperation
    {
        private readonly string _propertyName;

        internal FilterSizeOperation(string propertyName) => _propertyName = propertyName;
        
        public FilterSizeLessThan<T> LessThan<T>(T value) => new FilterSizeLessThan<T>(_propertyName, value);
        public FilterSizeLessThanOrEqualsTo<T> LessThanOrEqualsTo<T>(T value) => new FilterSizeLessThanOrEqualsTo<T>(_propertyName, value);
        public FilterSizeEqualsTo<T> EqualsTo<T>(T value) => new FilterSizeEqualsTo<T>(_propertyName, value);
        public FilterSizeNotEqualsTo<T> NotEqualsTo<T>(T value) => new FilterSizeNotEqualsTo<T>(_propertyName, value);
        public FilterSizeGreaterThan<T> GreaterThan<T>(T value) => new FilterSizeGreaterThan<T>(_propertyName, value);
        public FilterSizeGreaterThanOrEqualsTo<T> GreaterThanOrEqualsTo<T>(T value) => new FilterSizeGreaterThanOrEqualsTo<T>(_propertyName, value);
        public FilterSizeBetween<T> Between<T>(T min, T max) => new FilterSizeBetween<T>(_propertyName, min, max);
    }
}