using System.Linq.Expressions;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.FluentCondition.Operators.Update.AssignMath
{
    internal sealed class UpdateAssignRightValueMath<TEntity, TProperty> : UpdateAssignMathBase
    {
        private readonly Expression _left;
        private TProperty _right;

        public UpdateAssignRightValueMath(Expression expression, AssignMathOperator mathOperator, Expression left, TProperty right) : base(expression, mathOperator)
        {
            _left = left;
            _right = right;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "SET #a = #b + :v0"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.Builder);
            
            builder.Append(" = ");
            
            visitor.Visit<TEntity>(_left);
            builder.Append(visitor.Builder);

            AppendMathOperatorExpression(ref builder);
            
            builder.Append(":v");
            builder.Append(valuesCount++);
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            
            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _right, visitor, ref valuesCount);
        }
    }
}