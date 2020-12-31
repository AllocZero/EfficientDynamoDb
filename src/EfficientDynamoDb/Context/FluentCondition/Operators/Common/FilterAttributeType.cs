using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal class FilterAttributeType<TEntity> : FilterBase<TEntity>
    {
        private readonly string _propertyName;
        private readonly AttributeType _type;

        internal FilterAttributeType(string propertyName, AttributeType type) : base(propertyName)
        {
            _propertyName = propertyName;
            _type = type;
        }
    }
}