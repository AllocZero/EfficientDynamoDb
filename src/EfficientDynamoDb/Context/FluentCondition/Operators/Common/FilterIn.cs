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
        private readonly bool _useSize;
        private readonly TProperty[] _values;
        
        public FilterIn(Expression expression, bool useSize, TProperty[] values) : base(expression)
        {
            if (values.Length == 0)
                throw new ArgumentException("Values array can't be empty", nameof(values));

            _useSize = useSize;
            _values = values;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "#a IN (:v0, :v1, :v2)"
            
            visitor.Visit<TEntity>(Expression);
            
            WriteEncodedExpressionName(visitor.GetEncodedExpressionName(), _useSize, ref builder);
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
    
    internal sealed class FilterIn<TEntity> : FilterBase<TEntity>
    {
        private readonly bool _useSize;
        private readonly Expression[] _valueExpressions;
        private readonly bool _useValueSizes;

        public FilterIn(Expression expression, bool useSize, Expression[] valueExpressions, bool useValueSizes) : base(expression)
        {
            if (valueExpressions.Length == 0)
                throw new ArgumentException("Values array can't be empty", nameof(valueExpressions));

            _useSize = useSize;
            _valueExpressions = valueExpressions;
            _useValueSizes = useValueSizes;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "#a IN (#b, #c, #d)"
            
            visitor.Visit<TEntity>(Expression);
            
            WriteEncodedExpressionName(visitor.GetEncodedExpressionName(), _useSize, ref builder);
            builder.Append(" IN (");
            
            for (var i = 0; i < _valueExpressions.Length; i++)
            {
                if (i > 0)
                    builder.Append(", ");
                
                visitor.Visit<TEntity>(_valueExpressions[i]);
                WriteEncodedExpressionName(visitor.GetEncodedExpressionName(), _useValueSizes, ref builder);
            }

            builder.Append(')');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // Do nothing
        }
    }
}