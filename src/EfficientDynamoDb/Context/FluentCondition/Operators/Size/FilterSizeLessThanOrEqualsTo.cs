using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Size
{
    internal class FilterSizeLessThanOrEqualsTo<TEntity, TProperty> : FilterBase<TEntity>
    {
        private readonly string _propertyName;
        private readonly TProperty _value;

        internal FilterSizeLessThanOrEqualsTo(string propertyName, TProperty value) : base(propertyName)
        {
            _propertyName = propertyName;
            _value = value;
        }
    }
}