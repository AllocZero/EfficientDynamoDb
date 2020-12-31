using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal class FilterIn<TEntity, TProperty> : FilterBase<TEntity>
    {
        private readonly string _propertyName;
        private readonly TProperty[] _values;

        internal FilterIn(string propertyName, params TProperty[] values) : base(propertyName)
        {
            _propertyName = propertyName;
            _values = values;
        }
    }
}