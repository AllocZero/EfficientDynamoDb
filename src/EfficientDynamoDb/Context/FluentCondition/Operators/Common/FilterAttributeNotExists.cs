using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal class FilterAttributeNotExists<TEntity> : FilterBase<TEntity>
    {
        private readonly string _propertyName;

        internal FilterAttributeNotExists(string propertyName) : base(propertyName)
        {
            _propertyName = propertyName;
        }
    }
}