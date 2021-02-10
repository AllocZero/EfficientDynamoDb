using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Update.AssignMath
{
    internal sealed class UpdateAssignAttributesMathRightFallback<TEntity, TProperty> : UpdateAssignMathBase
    {
        private readonly Expression _left;
        private readonly Expression _right;
        private TProperty _rightFallbackValue;

        public UpdateAssignAttributesMathRightFallback(Expression expression, AssignMathOperator mathOperator, Expression left, Expression right,
            TProperty rightFallbackValue) : base(expression, mathOperator)
        {
            _left = left;
            _rightFallbackValue = rightFallbackValue;
            _right = right;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "SET #a = #b + if_not_exists(#c, :v0)"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.Builder);

            builder.Append(" = ");
            
            visitor.Visit<TEntity>(_left);
            builder.Append(visitor.Builder);
            
            AppendMathOperatorExpression(ref builder);
            
            WriteIfNotExistsBlock<TEntity>(ref builder, visitor, _right, ref valuesCount);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);

            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _rightFallbackValue, visitor, ref valuesCount);
        }
    }
}