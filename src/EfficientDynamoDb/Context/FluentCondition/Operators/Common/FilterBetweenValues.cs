using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterBetweenValues<TEntity, TProperty> : FilterBase<TEntity>
    {
        private readonly bool _useSize;
        private TProperty _min;
        private TProperty _max;

        public FilterBetweenValues(Expression expression, bool useSize, TProperty min, TProperty max) : base(expression)
        {
            _useSize = useSize;
            _min = min;
            _max = max;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "#a BETWEEN :v1 AND :v2"
            
            visitor.Visit<TEntity>(Expression);
            
            WriteEncodedExpressionName(visitor.Builder, _useSize, ref builder);
            
            builder.Append(" BETWEEN :v");
            builder.Append(valuesCount++);
            
            builder.Append(" AND :v");
            builder.Append(valuesCount++);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            visitor.Visit<TEntity>(Expression);
            var converter = GetPropertyConverter<TProperty>(visitor);
            
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

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
        private readonly bool _useSize;
        private readonly Expression _minExpression;
        private readonly bool _useMinSize;
        private readonly Expression _maxExpression;
        private readonly bool _useMaxSize;

        public FilterBetweenAttributes(Expression expression, bool useSize, Expression minExpression, bool useMinSize, Expression maxExpression, bool useMaxSize) : base(expression)
        {
            _useSize = useSize;
            _minExpression = minExpression;
            _useMinSize = useMinSize;
            _maxExpression = maxExpression;
            _useMaxSize = useMaxSize;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "#a BETWEEN #b AND #c"
            
            visitor.Visit<TEntity>(Expression);
            
            WriteEncodedExpressionName(visitor.Builder, _useSize, ref builder);

            visitor.Visit<TEntity>(_minExpression);
            builder.Append(" BETWEEN ");
            WriteEncodedExpressionName(visitor.Builder, _useMinSize, ref builder);

            visitor.Visit<TEntity>(_maxExpression);
            builder.Append(" AND ");
            WriteEncodedExpressionName(visitor.Builder, _useMaxSize, ref builder);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // Do nothing
        }
    }
    
    internal sealed class FilterBetweenValueAndAttribute<TEntity, TProperty> : FilterBase<TEntity>
    {
        private readonly bool _useSize;
        private TProperty _min;
        private readonly Expression _maxExpression;
        private readonly bool _useMaxSize;

        public FilterBetweenValueAndAttribute(Expression expression, bool useSize, TProperty min, Expression maxExpression, bool useMaxSize) : base(expression)
        {
            _useSize = useSize;
            _min = min;
            _maxExpression = maxExpression;
            _useMaxSize = useMaxSize;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "#a BETWEEN #v1 AND #b"
            
            visitor.Visit<TEntity>(Expression);
            
            WriteEncodedExpressionName(visitor.Builder, _useSize, ref builder);
            builder.Append(" BETWEEN :v");
            builder.Append(valuesCount++);

            visitor.Visit<TEntity>(_maxExpression);
            builder.Append(" AND ");
            WriteEncodedExpressionName(visitor.Builder, _useMaxSize, ref builder);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            visitor.Visit<TEntity>(Expression);
            var converter = GetPropertyConverter<TProperty>(visitor);
            
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            builder.Append(":v");
            builder.Append(valuesCount++);
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            converter.Write(in writer, ref _min);
        }
    }
    
    internal sealed class FilterBetweenAttributeAndValue<TEntity, TProperty> : FilterBase<TEntity>
    {
        private readonly bool _useSize;
        private readonly Expression _minExpression;
        private readonly bool _useMinSize;
        private TProperty _max;

        public FilterBetweenAttributeAndValue(Expression expression, bool useSize, Expression minExpression, bool useMinSize, TProperty max) : base(expression)
        {
            _useSize = useSize;
            _minExpression = minExpression;
            _useMinSize = useMinSize;
            _max = max;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "#a BETWEEN #b AND :v1"
            
            visitor.Visit<TEntity>(Expression);
            
            WriteEncodedExpressionName(visitor.Builder, _useSize, ref builder);

            visitor.Visit<TEntity>(_minExpression);
            builder.Append(" BETWEEN ");
            WriteEncodedExpressionName(visitor.Builder, _useMinSize, ref builder);
            
            builder.Append(" AND :v");
            builder.Append(valuesCount++);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            visitor.Visit<TEntity>(Expression);
            var converter = GetPropertyConverter<TProperty>(visitor);
            
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            builder.Append(":v");
            builder.Append(valuesCount++);
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            converter.Write(in writer, ref _max);
        }
    }
}