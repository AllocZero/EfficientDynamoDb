using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal class FilterGreaterThanOrEqualsTo<TEntity, TProperty> : FilterBase<TEntity>
    {
        private readonly string _propertyName;
        private readonly TProperty _value;

        internal FilterGreaterThanOrEqualsTo(string propertyName, TProperty value) : base(propertyName)
        {
            _propertyName = propertyName;
            _value = value;
        }
    }
}