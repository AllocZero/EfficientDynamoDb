using System;
using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterIn<TEntity, TProperty> : FilterBase<TEntity>
    {
        private readonly TProperty[] _values;
        
        public FilterIn(Expression expression, TProperty[] values) : base(expression)
        {
            if (values.Length == 0)
                throw new ArgumentException("Values array can't be empty", nameof(values));
            
            _values = values;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            // "#a IN (:v0, :v1, :v2)"
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append('#');
            builder.Append(visitor.GetEncodedExpressionName());
            builder.Append(" IN (");
            
            for (var i = 0; i < _values.Length; i++)
            {
                if (i > 0)
                    builder.Append(", ");
                
                builder.Append(':');
                builder.Append(valuesCount++);
            }

            builder.Append(')');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            var converter = GetPropertyConverter<TProperty>(visitor);

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