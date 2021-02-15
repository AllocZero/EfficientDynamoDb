using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Update.AssignMath
{
    internal sealed class UpdateAssignAttributesMath<TEntity> : UpdateAssignMathBase
    {
        private readonly Expression _left;
        private readonly Expression _right;

        public UpdateAssignAttributesMath(Expression expression, AssignMathOperator mathOperator, Expression left, Expression right) : base(expression,
            mathOperator)
        {
            _left = left;
            _right = right;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "SET #a = #b + #c"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.Builder);
            
            builder.Append(" = ");
            
            visitor.Visit<TEntity>(_left);
            builder.Append(visitor.Builder);
            
            AppendMathOperatorExpression(ref builder);
            
            visitor.Visit<TEntity>(_right);
            builder.Append(visitor.Builder);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // Do nothing
        }
    }
}