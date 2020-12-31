using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal class FilterBetween<TEntity, TProperty> : FilterBase<TEntity>
    {
        private readonly string _propertyName;
        private readonly TProperty _min;
        private readonly TProperty _max;

        internal FilterBetween(string propertyName, TProperty min, TProperty max) : base(propertyName)
        {
            _propertyName = propertyName;
            _min = min;
            _max = max;
        }
    }
}