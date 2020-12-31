using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal class FilterAttributeExists<TEntity> : FilterBase<TEntity>
    {
        private readonly string _propertyName;

        internal FilterAttributeExists(string propertyName) : base(propertyName)
        {
            _propertyName = propertyName;
        }
    }
}