using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    public class FilterAttributeType : IFilter
    {
        private readonly string _propertyName;
        private readonly AttributeType _type;

        internal FilterAttributeType(string propertyName, AttributeType type)
        {
            _propertyName = propertyName;
            _type = type;
        }
    }
}