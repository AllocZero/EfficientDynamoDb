using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterBetween<TEntity, TProperty> : FilterBase<TEntity>
    {
        private TProperty _min;
        private TProperty _max;

        internal FilterBetween(string propertyName, TProperty min, TProperty max) : base(propertyName)
        {
            _min = min;
            _max = max;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount)
        {
            // "#a BETWEEN :v1 AND :v2"
            builder.Append('#');
            builder.Append(PropertyName);
            builder.Append(" BETWEEN :v");
            builder.Append(valuesCount++);
            builder.Append(" AND :v");
            builder.Append(valuesCount++);

            cachedNames.Add(PropertyName);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            var converter = GetPropertyConverter<TProperty>(metadata);
            
            builder.Append(":v");
            builder.Append(valuesCount++);
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            converter.Write(in writer, ref _min);
            
            builder.Clear();
            builder.Append(":v");
            builder.Append(valuesCount++);
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            converter.Write(in writer, ref _max);
        }
    }
}