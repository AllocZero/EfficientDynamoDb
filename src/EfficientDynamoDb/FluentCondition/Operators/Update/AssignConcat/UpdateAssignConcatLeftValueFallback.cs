using System.Linq.Expressions;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.FluentCondition.Operators.Update.AssignConcat
{
    internal sealed class UpdateAssignConcatLeftValueFallback<TEntity, TProperty> : UpdateBase
    {
        private TProperty _leftValue;
        private readonly Expression _right;
        private TProperty _rightFallbackValue;

        public UpdateAssignConcatLeftValueFallback(Expression expression, TProperty leftValue, Expression right, TProperty rightFallbackValue) : base(expression)
        {
            _right = right;
            _rightFallbackValue = rightFallbackValue;
            _leftValue = leftValue;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "SET #a = list_append(:v0, if_not_exists(#b, :v1))"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.Builder);
            
            builder.Append(" = list_append(");
            
            builder.Append(":v");
            builder.Append(valuesCount++);
            
            builder.Append(',');
            
            WriteIfNotExistsBlock<TEntity>(ref builder, visitor, _right, ref valuesCount);
            
            builder.Append(')');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _leftValue, visitor, ref valuesCount);
            builder.Clear();
            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _rightFallbackValue, visitor, ref valuesCount);
        }
    }
}