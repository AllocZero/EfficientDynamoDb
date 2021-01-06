using System;
using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal class FilterIn<TEntity, TProperty> : FilterBase<TEntity>
    {
        private readonly string _propertyName;
        private readonly TProperty[] _values;

        internal FilterIn(string propertyName, params TProperty[] values) : base(propertyName)
        {
            if (values.Length == 0)
                throw new ArgumentException("Values array can't be empty", nameof(values));

            _propertyName = propertyName;
            _values = values;
        }

        protected override void WriteExpressionStatementInternal(ref NoAllocStringBuilder builder, HashSet<string> cachedNames, ref int valuesCount)
        {
            // "#a IN (:v0, :v1, :v2)"
            
            builder.Append('#');
            builder.Append(_propertyName);
            builder.Append(" IN (");
            
            for (var i = 0; i < _values.Length; i++)
            {
                if (i > 0)
                    builder.Append(", ");
                
                builder.Append(':');
                builder.Append(valuesCount++);
            }

            builder.Append(')');

            cachedNames.Add(_propertyName);
        }

        protected override void WriteAttributeValuesInternal(Utf8JsonWriter writer, ref int valuesCount)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            foreach (var value in _values)
            {
                builder.Append(":v");
                builder.Append(valuesCount++);
                writer.WriteString(builder.GetBuffer(), value);

                builder.Clear();
            }
        }
    }
}