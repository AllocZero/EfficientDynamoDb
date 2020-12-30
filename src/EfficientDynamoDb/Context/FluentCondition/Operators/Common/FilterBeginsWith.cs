using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    public class FilterBeginsWith : IFilter
    {
        private readonly string _propertyName;
        private readonly string _prefix;

        internal FilterBeginsWith(string propertyName, string prefix)
        {
            _propertyName = propertyName;
            _prefix = prefix;
        }
    }
}