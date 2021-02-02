using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Update.AssignSum
{
    internal sealed class UpdateAssignLeftValueMathFallback<TEntity, TProperty> : UpdateAssignMathBase
    {
        private TProperty _left;
        private readonly Expression _right;
        private TProperty _rightFallbackValue;

        public UpdateAssignLeftValueMathFallback(Expression expression, AssignMathOperator mathOperator, TProperty left, Expression right, TProperty rightFallbackValue) : base(expression, mathOperator)
        {
            _left = left;
            _right = right;
            _rightFallbackValue = rightFallbackValue;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "SET #a = :v0 + if_not_exists(#b, v1)"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.GetEncodedExpressionName());
            
            builder.Append(" = ");

            builder.Append(":v");
            valuesCount++;
            
            AppendMathOperatorExpression(ref builder);
            
            WriteIfNotExistsBlock<TEntity>(ref builder, visitor, _right, ref valuesCount);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _left, visitor, ref valuesCount);
            builder.Clear();
            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _rightFallbackValue, visitor, ref valuesCount);
        }
    }
}