using System.Linq.Expressions;
using EfficientDynamoDb.Context.FluentCondition.Factories;
using EfficientDynamoDb.Internal.Constants;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Context.FluentCondition.Operators.Update.AssignSum
{
    internal sealed class UpdateAssignRightValueSum<TEntity, TProperty> : UpdateBase
    {
        private readonly Expression _left;
        private TProperty _right;

        public UpdateAssignRightValueSum(Expression expression, Expression left, TProperty right) : base(expression)
        {
            _left = left;
            _right = right;
        }

        internal override void WriteExpressionStatement(ref NoAllocStringBuilder builder, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            // "SET #a = #b + :v0"
            
            visitor.Visit<TEntity>(Expression);
            builder.Append(visitor.GetEncodedExpressionName());
            
            builder.Append(" = ");
            
            visitor.Visit<TEntity>(_left);
            builder.Append(visitor.GetEncodedExpressionName());

            builder.Append(" + :v");

            valuesCount++;
        }

        internal override void WriteAttributeValues(in DdbWriter writer, DynamoDbContextMetadata metadata, ref int valuesCount, DdbExpressionVisitor visitor)
        {
            var builder = new NoAllocStringBuilder(stackalloc char[PrimitiveLengths.Int + 2], false);
            
            WriteAttributeValue<TEntity, TProperty>(ref builder, writer, ref _right, visitor, ref valuesCount);
        }
    }
}