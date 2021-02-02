using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Update.AssignConcat
{
    internal sealed class UpdateAssignConcatAttributes<TEntity> : UpdateBase
    {
        private readonly Expression _left;
        private readonly Expression _right;

        public UpdateAssignConcatAttributes(Expression expression, Expression left, Expression right) : base(expression)
        {
            _left = left;
            _right = right;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "SET #a = list_append(#b,#c)"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.GetEncodedExpressionName());
            
            builder.Append(" = list_append(");
            
            visitor.Visit<TEntity>(_left);
            builder.Append(visitor.GetEncodedExpressionName());
            
            builder.Append(',');

            visitor.Visit<TEntity>(_right);
            builder.Append(visitor.GetEncodedExpressionName());
            
            builder.Append(')');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // Do nothing
        }
    }
}