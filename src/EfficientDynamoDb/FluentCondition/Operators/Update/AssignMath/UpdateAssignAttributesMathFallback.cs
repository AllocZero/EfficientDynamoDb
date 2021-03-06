using System.Linq.Expressions;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.FluentCondition.Operators.Update.AssignMath
{
    internal sealed class UpdateAssignAttributesMathFallback<TEntity, TProperty> : UpdateAssignMathBase
    {
        private readonly Expression _left;
        private TProperty _leftFallbackValue;
        private readonly Expression _right;
        private TProperty _rightFallbackValue;

        public UpdateAssignAttributesMathFallback(Expression expression, AssignMathOperator mathOperator, Expression left, TProperty leftFallbackValue,
            Expression right, TProperty rightFallbackValue) : base(expression, mathOperator)
        {
            _left = left;
            _leftFallbackValue = leftFallbackValue;
            _right = right;
            _rightFallbackValue = rightFallbackValue;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "SET #a = if_not_exists(#b, :v0) + if_not_exists(#c, :v1)"

            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.Builder);

            builder.Append(" = ");

            WriteIfNotExistsBlock<TEntity>(ref builder, visitor, _left, ref valuesCount);

            AppendMathOperatorExpression(ref builder);

            WriteIfNotExistsBlock<TEntity>(ref builder, visitor, _right, ref valuesCount);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            
            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _leftFallbackValue, visitor, ref valuesCount);
            builder.Clear();
            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _rightFallbackValue, visitor, ref valuesCount);
        }
    }
}