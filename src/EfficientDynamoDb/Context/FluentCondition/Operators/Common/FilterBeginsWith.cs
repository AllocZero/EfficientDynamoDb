using EfficientDynamoDb.Context.FluentCondition.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal class FilterBeginsWith<TEntity> : FilterBase<TEntity>
    {
        private readonly string _propertyName;
        private readonly string _prefix;

        internal FilterBeginsWith(string propertyName, string prefix) : base(propertyName)
        {
            _propertyName = propertyName;
            _prefix = prefix;
        }
    }
}