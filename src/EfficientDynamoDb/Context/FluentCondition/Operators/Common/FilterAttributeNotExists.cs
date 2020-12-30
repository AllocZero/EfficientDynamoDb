using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    public class FilterAttributeNotExists : IFilter
    {
        private readonly string _propertyName;

        internal FilterAttributeNotExists(string propertyName)
        {
            _propertyName = propertyName;
        }
    }
}