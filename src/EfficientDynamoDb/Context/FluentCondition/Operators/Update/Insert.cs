using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Update
{
    internal sealed class UpdateInsert<TEntity, TProperty> : UpdateBase
    {
        private TProperty _value;

        public UpdateInsert(Expression expression, TProperty value) : base(expression)
        {
            _value = value;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "ADD #a :v0"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.GetEncodedExpressionName());
            builder.Append(" :v");
            builder.Append(valuesCount++);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            
            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _value, visitor, ref valuesCount);
        }
    }
    
    internal sealed class UpdateInsert<TEntity> : UpdateBase
    {
        private readonly Expression _valueExpression;

        public UpdateInsert(Expression expression, Expression valueExpression) : base(expression)
        {
            _valueExpression = valueExpression;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "ADD #a #b"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.GetEncodedExpressionName());

            builder.Append(' ');
            
            visitor.Visit<TEntity>(_valueExpression);
            builder.Append(visitor.GetEncodedExpressionName());
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // Do nothing
        }
    }
    
    internal sealed class UpdateInsertFallback<TEntity, TProperty> : UpdateBase
    {
        private readonly Expression _valueExpression;
        private TProperty _fallbackValue;

        public UpdateInsertFallback(Expression expression, Expression valueExpression, TProperty fallbackValue) : base(expression)
        {
            _valueExpression = valueExpression;
            _fallbackValue = fallbackValue;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "ADD #a if_not_exists(#b, :v0)"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.GetEncodedExpressionName());
            
            builder.Append(" if_not_exists(");
            
            visitor.Visit<TEntity>(_valueExpression);
            builder.Append(visitor.GetEncodedExpressionName());

            builder.Append(",:v");
            builder.Append(valuesCount++);
            builder.Append(')');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            
            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _fallbackValue, visitor, ref valuesCount);
        }
    }
}