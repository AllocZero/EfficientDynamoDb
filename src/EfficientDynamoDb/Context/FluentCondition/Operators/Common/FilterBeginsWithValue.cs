using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Core;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Common
{
    internal sealed class FilterBeginsWithValue<TEntity> : FilterBase<TEntity>
    {
        private readonly string _prefix;

        public FilterBeginsWithValue(Expression expression, string prefix) : base(expression) => _prefix = prefix;

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            // begins_with(#a,:v0)
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append("begins_with(");
            builder.Append(visitor.GetEncodedExpressionName());
            builder.Append(",:v");
            
            builder.Append(valuesCount++);
            builder.Append(')');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            builder.Append(":v");
            builder.Append(valuesCount++);

            writer.JsonWriter.WriteString(builder.GetBuffer(), _prefix);
        }
    }
    
    internal sealed class FilterBeginsWithAttribute<TEntity> : FilterBase<TEntity>
    {
        private readonly Expression _prefixExpression;

        public FilterBeginsWithAttribute(Expression expression, Expression prefixExpression) : base(expression) => _prefixExpression = prefixExpression;

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount,
            DdbExpressionVisitor visitor)
        {
            // begins_with(#a,#b)
            
            visitor.Visit<TEntity>(Expression);
            
            builder.Append("begins_with(");
            builder.Append(visitor.GetEncodedExpressionName());
            builder.Append(",");
            
            visitor.Visit<TEntity>(_prefixExpression);
            builder.Append(visitor.GetEncodedExpressionName());
            builder.Append(')');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // Do nothing
        }
    }
}