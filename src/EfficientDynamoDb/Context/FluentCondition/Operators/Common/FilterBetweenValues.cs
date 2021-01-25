using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterBetweenValues<TEntity, TProperty> : FilterBase<TEntity>
    {
        private TProperty _min;
        private TProperty _max;

        public FilterBetweenValues(Expression expression, TProperty min, TProperty max) : base(expression)
        {
            _min = min;
            _max = max;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            // "#a BETWEEN :v1 AND :v2"
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append(visitor.GetEncodedExpressionName());
            
            builder.Append(" BETWEEN :v");
            builder.Append(valuesCount++);
            
            builder.Append(" AND :v");
            builder.Append(valuesCount++);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            var converter = GetPropertyConverter<TProperty>(visitor);
            
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
    
    internal sealed class FilterBetweenAttributes<TEntity> : FilterBase<TEntity>
    {
        private readonly Expression _minExpression;
        private readonly Expression _maxExpression;

        public FilterBetweenAttributes(Expression expression, Expression minExpression, Expression maxExpression) : base(expression)
        {
            _minExpression = minExpression;
            _maxExpression = maxExpression;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            // "#a BETWEEN #b AND #c"
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append(visitor.GetEncodedExpressionName());

            visitor.Visit<TEntity>(_minExpression);
            builder.Append(" BETWEEN ");
            builder.Append(visitor.GetEncodedExpressionName());

            visitor.Visit<TEntity>(_maxExpression);
            builder.Append(" AND ");
            builder.Append(visitor.GetEncodedExpressionName());
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // Do nothing
        }
    }
    
    internal sealed class FilterBetweenValueAndAttribute<TEntity, TProperty> : FilterBase<TEntity>
    {
        private TProperty _min;
        private readonly Expression _maxExpression;

        public FilterBetweenValueAndAttribute(Expression expression, TProperty min, Expression maxExpression) : base(expression)
        {
            _min = min;
            _maxExpression = maxExpression;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            // "#a BETWEEN #v1 AND #b"
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append(visitor.GetEncodedExpressionName());
            builder.Append(" BETWEEN :v");
            builder.Append(valuesCount++);

            visitor.Visit<TEntity>(_maxExpression);
            builder.Append(" AND ");
            builder.Append(visitor.GetEncodedExpressionName());
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            var converter = GetPropertyConverter<TProperty>(visitor);
            
            builder.Append(":v");
            builder.Append(valuesCount++);
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            converter.Write(in writer, ref _min);
        }
    }
    
    internal sealed class FilterBetweenAttributeAndValue<TEntity, TProperty> : FilterBase<TEntity>
    {
        private readonly Expression _minExpression;
        private TProperty _max;

        public FilterBetweenAttributeAndValue(Expression expression, Expression minExpression, TProperty max) : base(expression)
        {
            _minExpression = minExpression;
            _max = max;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            // "#a BETWEEN #b AND :v1"
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append(visitor.GetEncodedExpressionName());

            visitor.Visit<TEntity>(_minExpression);
            builder.Append(" BETWEEN ");
            builder.Append(visitor.GetEncodedExpressionName());
            
            builder.Append(" AND :v");
            builder.Append(valuesCount++);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            var converter = GetPropertyConverter<TProperty>(visitor);
            
            builder.Append(":v");
            builder.Append(valuesCount++);
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            converter.Write(in writer, ref _max);
        }
    }
}