using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Update.AssignMath
{
    internal sealed class UpdateAssignLeftValueMath<TEntity, TProperty> : UpdateAssignMathBase
    {
        private TProperty _left;
        private readonly Expression _right;

        public UpdateAssignLeftValueMath(Expression expression, AssignMathOperator mathOperator, TProperty left, Expression right) : base(expression,
            mathOperator)
        {
            _left = left;
            _right = right;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "SET #a = :v0 + #b"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.GetEncodedExpressionName());
            
            builder.Append(" = ");

            builder.Append(":v");
            builder.Append(valuesCount++);
            
            AppendMathOperatorExpression(ref builder);
            
            visitor.Visit<TEntity>(_right);
            builder.Append(visitor.GetEncodedExpressionName());
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            
            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _left, visitor, ref valuesCount);
        }
    }
}