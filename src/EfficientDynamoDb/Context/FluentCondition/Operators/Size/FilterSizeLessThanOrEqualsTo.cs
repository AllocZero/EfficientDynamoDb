using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Size
{
    public class FilterSizeLessThanOrEqualsTo<T> : IFilter
    {
        private readonly string _propertyName;
        private readonly T _value;

        internal FilterSizeLessThanOrEqualsTo(string propertyName, T value)
        {
            _propertyName = propertyName;
            _value = value;
        }
    }
}