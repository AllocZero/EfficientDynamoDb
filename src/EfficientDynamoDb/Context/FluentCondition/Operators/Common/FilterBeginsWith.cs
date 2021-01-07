using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterBeginsWith<TEntity> : FilterBase<TEntity>
    {
        private readonly string _prefix;

        internal FilterBeginsWith(string propertyName, string prefix) : base(propertyName)
        {
            _prefix = prefix;
        }
        
        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount)
        {
            builder.Append("begins_with(#");
            builder.Append(PropertyName);
            builder.Append(",:v");
            
            builder.Append(valuesCount++);
            builder.Append(')');

            cachedNames.Add(PropertyName);
        }

        internal override void WriteAttributeValues(Utf8JsonWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            builder.Append(":v");
            builder.Append(valuesCount++);

            writer.WriteString(builder.GetBuffer(), _prefix);
        }
    }
}