using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterAttributeNotExists<TEntity> : FilterBase<TEntity>
    {
        internal FilterAttributeNotExists(string propertyName) : base(propertyName)
        {
        }
        
        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount)
        {
            builder.Append("attribute_not_exists(#");
            builder.Append(PropertyName);
            builder.Append(')');

            cachedNames.Add(PropertyName);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount)
        {
            // Do nothing
        }
    }
}