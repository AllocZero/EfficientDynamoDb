using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Size
{
    public class FilterSizeBetween<T> : IFilter
    {
        private readonly string _propertyName;
        private readonly T _min;
        private readonly T _max;

        internal FilterSizeBetween(string propertyName, T min, T max)
        {
            _propertyName = propertyName;
            _min = min;
            _max = max;
        }
    }
}