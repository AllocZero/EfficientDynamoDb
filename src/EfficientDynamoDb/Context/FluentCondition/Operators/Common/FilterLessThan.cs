using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    public class FilterLessThan<T> : IFilter
    {
        private readonly string _propertyName;
        private readonly T _value;

        internal FilterLessThan(string propertyName, T value)
        {
            _propertyName = propertyName;
            _value = value;
        }
    }
}