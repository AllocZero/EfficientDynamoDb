using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    public class FilterLessThanOrEqualsTo<T> : IFilter
    {
        private readonly string _propertyName;
        private readonly T _value;

        internal FilterLessThanOrEqualsTo(string propertyName, T value)
        {
            _propertyName = propertyName;
            _value = value;
        }
    }
}