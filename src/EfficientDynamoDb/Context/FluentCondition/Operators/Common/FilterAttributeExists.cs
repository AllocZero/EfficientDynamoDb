using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    public class FilterAttributeExists : IFilter
    {
        private readonly string _propertyName;

        internal FilterAttributeExists(string propertyName)
        {
            _propertyName = propertyName;
        }
    }
}