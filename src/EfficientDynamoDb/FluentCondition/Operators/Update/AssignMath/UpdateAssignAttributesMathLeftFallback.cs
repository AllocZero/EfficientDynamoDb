using System.Linq.Expressions;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.FluentCondition.Operators.Update.AssignMath
{
    internal sealed class UpdateAssignAttributesMathLeftFallback<TEntity, TProperty> : UpdateAssignMathBase
    {
        private readonly Expression _left;
        private TProperty _leftFallbackValue;
        private readonly Expression _right;

        public UpdateAssignAttributesMathLeftFallback(Expression expression, AssignMathOperator mathOperator, Expression left, TProperty leftFallbackValue,
            Expression right) : base(expression, mathOperator)
        {
            _left = left;
            _leftFallbackValue = leftFallbackValue;
            _right = right;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "SET #a = if_not_exists(#b, :v0) + #c"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.Builder);

            builder.Append(" = ");
            
            WriteIfNotExistsBlock<TEntity>(ref builder, visitor, _left, ref valuesCount);
            
            AppendMathOperatorExpression(ref builder);
            
            visitor.Visit<TEntity>(_right);
            builder.Append(visitor.Builder);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            
            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _leftFallbackValue, visitor, ref valuesCount);
        }
    }
}