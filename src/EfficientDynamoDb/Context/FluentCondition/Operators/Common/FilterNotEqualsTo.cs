using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal class FilterNotEqualsTo<TEntity, TProperty> : FilterBase<TEntity>
    {
        private readonly string _propertyName;
        private readonly TProperty _value;

        internal FilterNotEqualsTo(string propertyName, TProperty value) : base(propertyName)
        {
            _propertyName = propertyName;
            _value = value;
        }
    }
}