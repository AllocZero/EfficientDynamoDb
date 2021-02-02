using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Update.AssignSum
{
    internal sealed class UpdateAssignRightValueMathFallback<TEntity, TProperty> : UpdateAssignMathBase
    {
        private readonly Expression _left;
        private TProperty _leftFallbackValue;
        private TProperty _right;

        public UpdateAssignRightValueMathFallback(Expression expression, AssignMathOperator mathOperator, Expression left, TProperty leftFallbackValue, TProperty right) : base(expression, mathOperator)
        {
            _left = left;
            _leftFallbackValue = leftFallbackValue;
            _right = right;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "SET #a = if_not_exists(#b,:v0) + :v1"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.GetEncodedExpressionName());
            
            builder.Append(" = ");
            
            WriteIfNotExistsBlock<TEntity>(ref builder, visitor, _left, ref valuesCount);
            
            AppendMathOperatorExpression(ref builder);

            builder.Append(":v");
            valuesCount++;
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            
            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _leftFallbackValue, visitor, ref valuesCount);
            builder.Clear();
            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _right, visitor, ref valuesCount);
        }
    }
}