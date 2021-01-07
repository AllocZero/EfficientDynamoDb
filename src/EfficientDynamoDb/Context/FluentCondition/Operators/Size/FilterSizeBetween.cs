using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Size
{
    internal sealed class FilterSizeBetween<TEntity, TProperty> : FilterBase<TEntity>
    {
        private TProperty _min;
        private TProperty _max;

        internal FilterSizeBetween(string propertyName, TProperty min, TProperty max) : base(propertyName)
        {
            _min = min;
            _max = max;
        }
        
        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount)
        {
            // "size(#a) BETWEEN :v1 AND :v2"
            builder.Append("size(#");
            builder.Append(PropertyName);
            builder.Append(") BETWEEN :v");
            builder.Append(valuesCount++);
            builder.Append(" AND :v");
            builder.Append(valuesCount++);

            cachedNames.Add(PropertyName);
        }

        internal override void WriteAttributeValues(Utf8JsonWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            var converter = GetPropertyConverter<TProperty>(metadata);
            
            builder.Append(":v");
            builder.Append(valuesCount++);
            writer.WritePropertyName(builder.GetBuffer());
            converter.Write(writer, ref _min);
            
            builder.Clear();
            builder.Append(":v");
            builder.Append(valuesCount++);
            writer.WritePropertyName(builder.GetBuffer());
            converter.Write(writer, ref _max);
        }
    }
}