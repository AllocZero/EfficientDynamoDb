using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Size
{
    internal sealed class FilterSizeGreaterThan<TEntity, TProperty> : FilterBase<TEntity>
    {
        private TProperty _value;

        internal FilterSizeGreaterThan(string propertyName, TProperty value) : base(propertyName)
        {
            _value = value;
        }
        
        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount)
        {
            // "size(#a) > :v0"
            
            builder.Append("size(#");
            builder.Append(PropertyName);
            builder.Append(") > :v");
            builder.Append(valuesCount++);

            cachedNames.Add(PropertyName);
        }

        internal override void WriteAttributeValues(Utf8JsonWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            builder.Append(":v");
            builder.Append(valuesCount++);

            writer.WritePropertyName(builder.GetBuffer());
            GetPropertyConverter<TProperty>(metadata).Write(writer, ref _value);
        }
    }
}