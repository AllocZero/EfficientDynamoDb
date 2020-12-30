using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    public class FilterContains<T> : IFilter
    {
        private readonly string _propertyName;
        private readonly T _value;

        internal FilterContains(string propertyName, T value)
        {
            _propertyName = propertyName;
            _value = value;
        }
    }
}