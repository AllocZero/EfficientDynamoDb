using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterContains<TEntity, TProperty> : FilterBase<TEntity>
    {
        private TProperty _value;

        public FilterContains(Expression expression, TProperty value) : base(expression) => _value = value;

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            // "contains (#a, :v0)"
            
            visitor.Visit<TEntity>(Expression);

            builder.Append("contains (");
            builder.Append(visitor.GetEncodedExpressionName());
            builder.Append(",:v");
            builder.Append(valuesCount++);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            builder.Append(":v");
            builder.Append(valuesCount++);
            
            writer.JsonWriter.WritePropertyName(builder.GetBuffer());
            GetPropertyConverter<TProperty>(visitor).Write(in writer, ref _value);
        }
    }
    
    internal sealed class FilterContains<TEntity> : FilterBase<TEntity>
    {
        private readonly Expression _valueExpression;

        public FilterContains(Expression expression, Expression valueExpression) : base(expression) => _valueExpression = valueExpression;

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            // "contains (#a,#b)"
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append("contains (");
            builder.Append(visitor.GetEncodedExpressionName());
            builder.Append(",");

            visitor.Visit<TEntity>(_valueExpression);
            builder.Append(visitor.GetEncodedExpressionName());
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // Do nothing
        }
    }
}