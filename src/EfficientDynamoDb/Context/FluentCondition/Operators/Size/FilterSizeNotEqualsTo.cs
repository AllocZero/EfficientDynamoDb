using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Size
{
    public class FilterSizeNotEqualsTo<T> : IFilter
    {
        private readonly string _propertyName;
        private readonly T _value;

        internal FilterSizeNotEqualsTo(string propertyName, T value)
        {
            _propertyName = propertyName;
            _value = value;
        }
    }
}