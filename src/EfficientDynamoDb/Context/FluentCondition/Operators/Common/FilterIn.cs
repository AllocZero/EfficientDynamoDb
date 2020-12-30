using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    public class FilterIn<T> : IFilter
    {
        private readonly string _propertyName;
        private readonly T[] _values;

        internal FilterIn(string propertyName, params T[] values)
        {
            _propertyName = propertyName;
            _values = values;
        }
    }
}