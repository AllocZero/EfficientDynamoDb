using System;
using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterIn<TEntity, TProperty> : FilterBase<TEntity>
    {
        private readonly TProperty[] _values;

        internal FilterIn(string propertyName, params TProperty[] values) : base(propertyName)
        {
            if (values.Length == 0)
                throw new ArgumentException("Values array can't be empty", nameof(values));

            _values = values;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount)
        {
            // "#a IN (:v0, :v1, :v2)"
            
            builder.Append('#');
            builder.Append(PropertyName);
            builder.Append(" IN (");
            
            for (var i = 0; i < _values.Length; i++)
            {
                if (i > 0)
                    builder.Append(", ");
                
                builder.Append(':');
                builder.Append(valuesCount++);
            }

            builder.Append(')');

            cachedNames.Add(PropertyName);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            var converter = GetPropertyConverter<TProperty>(metadata);

            for (var i = 0; i < _values.Length; i++)
            {
                builder.Append(":v");
                builder.Append(valuesCount++);

                writer.JsonWriter.WritePropertyName(builder.GetBuffer());
                converter.Write(in writer, ref _values[i]);

                builder.Clear();
            }
        }
    }
}