using System.Linq.Expressions;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.FluentCondition.Operators.Update
{
    internal sealed class UpdateRemoveFromSet<TEntity, TProperty> : UpdateBase
    {
        private TProperty _value;

        public UpdateRemoveFromSet(Expression expression, TProperty value) : base(expression)
        {
            _value = value;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "DELETE #a :v0"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.Builder);
            builder.Append(" :v");
            builder.Append(valuesCount++);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            
            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _value, visitor, ref valuesCount);
        }
    }
    
    internal sealed class UpdateRemoveFromSet<TEntity> : UpdateBase
    {
        private readonly Expression _valueExpression;

        public UpdateRemoveFromSet(Expression expression, Expression valueExpression) : base(expression)
        {
            _valueExpression = valueExpression;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "DELETE #a #b"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.Builder);

            builder.Append(' ');
            
            visitor.Visit<TEntity>(_valueExpression);
            builder.Append(visitor.Builder);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // Do nothing
        }
    }
    
    internal sealed class UpdateRemoveFromSetFallback<TEntity, TProperty> : UpdateBase
    {
        private readonly Expression _valueExpression;
        private TProperty _fallbackValue;

        public UpdateRemoveFromSetFallback(Expression expression, Expression valueExpression, TProperty fallbackValue) : base(expression)
        {
            _valueExpression = valueExpression;
            _fallbackValue = fallbackValue;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "DELETE #a if_not_exists(#b, :v0)"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.Builder);
            
            builder.Append(" if_not_exists(");
            
            visitor.Visit<TEntity>(_valueExpression);
            builder.Append(visitor.Builder);

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