using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    public class FilterBetween<T> : IFilter
    {
        private readonly string _propertyName;
        private readonly T _min;
        private readonly T _max;

        internal FilterBetween(string propertyName, T min, T max)
        {
            _propertyName = propertyName;
            _min = min;
            _max = max;
        }
    }
}