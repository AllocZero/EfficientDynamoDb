using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Update.AssignConcat
{
    internal sealed class UpdateAssignConcatRightValue<TEntity, TProperty> : UpdateBase
    {
        private readonly Expression _left;
        private TProperty _rightValue;

        public UpdateAssignConcatRightValue(Expression expression, Expression left, TProperty rightValue) : base(expression)
        {
            _left = left;
            _rightValue = rightValue;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "SET #a = list_append(#b, :v0)"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.GetEncodedExpressionName());
            
            builder.Append(" = list_append(");
            
            visitor.Visit<TEntity>(_left);
            builder.Append(visitor.GetEncodedExpressionName());

            builder.Append(',');

            builder.Append(":v");
            valuesCount++;
            
            builder.Append(')');
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            
            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _rightValue, visitor, ref valuesCount);
        }
    }
}