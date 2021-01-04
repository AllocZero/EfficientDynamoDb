using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal class FilterAttributeExists<TEntity> : FilterBase<TEntity>
    {
        private readonly string _propertyName;

        internal FilterAttributeExists(string propertyName) : base(propertyName)
        {
            _propertyName = propertyName;
        }

        protected override void WriteExpressionStatementInternal(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount)
        {
            builder.Append("attribute_exists(#");
            builder.Append(_propertyName);
            builder.Append(')');

            cachedNames.Add(_propertyName);
        }

        protected override void WriteAttributeValuesInternal(Utf8JsonWriter writer, ref int valuesCount)
        {
            // Do nothing
        }
    }
}